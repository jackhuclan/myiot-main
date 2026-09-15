using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace VgAutoDrill.Central.WebApi.Core.Redis
{
    public class RedisClient : IRedisClient
    {
        private readonly RedisCacheOptions redisOptions;
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase _db;

        public RedisClient(IOptions<RedisCacheOptions> redisCacheOptions)
        {
            redisOptions = redisCacheOptions.Value;
            redis = ConnectionMultiplexer.Connect(redisOptions.ConfigurationOptions);
            _db = redis.GetDatabase();
        }

        public bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return _db.KeyDelete(key, flags);
        }

        public bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            return _db.KeyExists(key, flags);
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
}
