using StackExchange.Redis;

namespace VgAutoDrill.Admin.Application.Redis;

public interface IRedisClient
{
    bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None);
    bool StringSet(RedisKey key, RedisValue value);
    RedisValue StringGet(RedisKey key, CommandFlags flags = CommandFlags.None);
    bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None);
    bool LockRelease(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None);
    bool LockTake(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None);
    long ClearKeys(RedisKey keyWord);
    List<string> FindKeys(RedisKey keyWord);
    void Subscribe<TRedisPacket>(string channel, Action<TRedisPacket> handler, Action<Exception>? exceptionHandler = null);
    void Unsubscribe(string channel);
    void Publish<TRedisPacket>(string channel, TRedisPacket message);
    Task<bool> TryPublish<TRedisPacket>(string channel, TRedisPacket message, TimeSpan? timeSpan = default);
}
