using StackExchange.Redis;

namespace VgAutoDrill.Central.WebApi.Core.Redis
{
    public interface IRedisClient
    {
        bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None);
        bool StringSet(RedisKey key, RedisValue value);
        RedisValue StringGet(RedisKey key, CommandFlags flags = CommandFlags.None);
        bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None);
        bool LockRelease(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None);
        bool LockTake(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None);
    }
}
