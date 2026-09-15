using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace VgAutoDrill.Admin.Application.Redis;

public class RedisClient : IRedisClient
{
    private readonly RedisCacheOptions redisOptions;
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private readonly IServer _server;

    public RedisClient(IOptions<RedisCacheOptions> redisCacheOptions)
    {
        redisOptions = redisCacheOptions.Value;
        ConfigurationOptions configurationOptions = ConfigurationOptions.Parse(redisOptions.ConnectionString);
        _redis = ConnectionMultiplexer.Connect(configurationOptions);
        _server = _redis.GetServer(_redis.GetEndPoints()[0]);
        _db = _redis.GetDatabase();
    }

    public void Subscribe<TRedisPacket>(string channel, Action<TRedisPacket> handler, Action<Exception>? exceptionHandler = null)
    {
        var redisChannel = new RedisChannel(channel, RedisChannel.PatternMode.Auto);
        var replyChannel = new RedisChannel(channel + "__reply", RedisChannel.PatternMode.Auto);
        _redis.GetSubscriber().Subscribe(channel, (c, v) =>
        {
            if (!v.HasValue
            || string.IsNullOrWhiteSpace(v.ToString()))
                return;

            try
            {
                var obj = JsonSerializer.Deserialize<TRedisPacket>(v.ToString());
                if (obj == null) return;
                handler.Invoke(obj);
                _db.Publish(replyChannel, true);
            }
            catch (Exception ex)
            {
                exceptionHandler?.Invoke(ex);
                _db.Publish(replyChannel, false);
            }
        });
    }

    public void Unsubscribe(string channel)
    {
        var redisChannel = new RedisChannel(channel, RedisChannel.PatternMode.Auto);
        _redis.GetSubscriber().Unsubscribe(channel);
    }

    public void Publish<TRedisPacket>(string channel, TRedisPacket message)
    {
        var redisChannel = new RedisChannel(channel, RedisChannel.PatternMode.Auto);
        var encodedMessage = JsonSerializer.Serialize(message);
        _db.Publish(redisChannel, new RedisValue(encodedMessage));
    }

    public async Task<bool> TryPublish<TRedisPacket>(string channel, TRedisPacket message, TimeSpan? timeSpan = default)
    {
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(timeSpan ?? TimeSpan.FromSeconds(5));
        CancellationToken cancellationToken = tokenSource.Token;

        var promise = new TaskCompletionSource<bool>();
        var redisChannel = new RedisChannel(channel, RedisChannel.PatternMode.Auto);
        var replyChannel = new RedisChannel(channel + "__reply", RedisChannel.PatternMode.Auto);
        var encodedMessage = JsonSerializer.Serialize(message);
        _redis.GetSubscriber().Subscribe(replyChannel, (c, v) =>
        {
            if (promise != null && promise.Task != null && !promise.Task.IsCompleted)
            {
                promise.SetResult(v.HasValue && (bool)v);
            }
        });

        cancellationToken.Register(() =>
        {
            if (promise != null && promise.Task != null && !promise.Task.IsCompleted)
            {
                promise.SetResult(false);
            }
        });

        await _db.PublishAsync(redisChannel, new RedisValue(encodedMessage));
        return await promise.Task;
    }

    public bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
    {
        return _db.KeyDelete(key, flags);
    }

    public bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None)
    {
        return _db.KeyExists(key, flags);
    }

    public List<string> FindKeys(RedisKey keyWord)
    {
        var keys = _server.Keys(pattern: $"*{keyWord}*");

        var result = new List<string>();
        keys.ToList().ForEach(key => { result.Add(key); });

        return result;
    }

    public long ClearKeys(RedisKey keyWord)
    {
        var keys = _server.Keys(pattern: $"*{keyWord}*");

        return _db.KeyDelete(keys.ToArray());
    }

    public bool LockRelease(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
    {
        return _db.LockRelease(key, value, flags);
    }

    public bool LockTake(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
    {
        return _db.LockTake(key, value, expiry, flags);
    }

    public RedisValue StringGet(RedisKey key, CommandFlags flags = CommandFlags.None)
    {
        return _db.StringGet(key, flags);
    }

    public bool StringSet(RedisKey key, RedisValue value)
    {
        return _db.StringSet(key, value);
    }
}
