using System.Text;
using Microsoft.Extensions.Options;
using Moq;

namespace UnitTest.VgAutoDrill.Infrastructure;

public class RedisClientTest
{
    [Fact]
    public async Task TestCancelTryPublish()
    {
        var mock = new Mock<IOptions<RedisCacheOptions>>();
        mock.Setup(x => x.Value)
            .Returns(new RedisCacheOptions
            {
                ConnectionString = "localhost:6379,connectTimeout=3000,connectRetry=1,syncTimeout=3000,DefaultDatabase=1,password=",
            });

        var redisClient = new RedisClient(mock.Object);

        redisClient.Subscribe<TRedisPacket>("a.b.c", packet =>
        {
            Assert.Equal("zhangsan", packet.Name);
            Assert.Equal("ABC", Encoding.ASCII.GetString(packet.Data));
            Thread.Sleep(5000);
        });

        bool b = await redisClient.TryPublish("a.b.c", new TRedisPacket
        {
            Name = "zhangsan",
            Data = Encoding.ASCII.GetBytes("ABC")
        });

        Assert.False(b);
    }

    [Fact]
    public async Task TestTryPublishAndSubscribeFromOneRedisDatabase()
    {
        var mock = new Mock<IOptions<RedisCacheOptions>>();
        mock.Setup(x => x.Value)
            .Returns(new RedisCacheOptions
            {
                ConnectionString = "localhost:6379,connectTimeout=3000,connectRetry=1,syncTimeout=3000,DefaultDatabase=1,password=",
            });

        var redisClient = new RedisClient(mock.Object);

        for (var i = 0; i < 10; i++)
        {
            redisClient.Subscribe<TRedisPacket>("a.b.c", packet =>
            {
                Assert.Equal("zhangsan", packet.Name);
                Assert.Equal("ABC", Encoding.ASCII.GetString(packet.Data));
            });

            bool b = await redisClient.TryPublish("a.b.c", new TRedisPacket
            {
                Name = "zhangsan",
                Data = Encoding.ASCII.GetBytes("ABC")
            });

            Assert.True(b);
        }
    }

    [Fact]
    public void TestPublishAndSubscribeFromOneRedisDatabase()
    {
        var mock = new Mock<IOptions<RedisCacheOptions>>();
        mock.Setup(x => x.Value)
            .Returns(new RedisCacheOptions
            {
                ConnectionString = "localhost:6379,connectTimeout=3000,connectRetry=1,syncTimeout=3000,DefaultDatabase=1,password=",
            });

        var redisClient = new RedisClient(mock.Object);
        redisClient.Subscribe<TRedisPacket>("a.b.c", packet =>
        {
            Assert.Equal("zhangsan", packet.Name);
            Assert.Equal("ABC", Encoding.ASCII.GetString(packet.Data));
        });

        redisClient.Publish("a.b.c", new TRedisPacket
        {
            Name = "zhangsan",
            Data = Encoding.ASCII.GetBytes("ABC")
        });
    }

    [Fact]
    public void TestPublishAndSubscribeFromDifferentRedisDatabase()
    {
        var mock1 = new Mock<IOptions<RedisCacheOptions>>();
        mock1.Setup(x => x.Value)
            .Returns(new RedisCacheOptions
            {
                ConnectionString = "localhost:6379,connectTimeout=3000,connectRetry=1,syncTimeout=3000,DefaultDatabase=1,password=",
            });

        var mock2 = new Mock<IOptions<RedisCacheOptions>>();
        mock2.Setup(x => x.Value)
            .Returns(new RedisCacheOptions
            {
                ConnectionString = "localhost:6379,connectTimeout=3000,connectRetry=1,syncTimeout=3000,DefaultDatabase=2,password=",
            });

        var redisClient1 = new RedisClient(mock1.Object);
        var redisClient2 = new RedisClient(mock2.Object);

        redisClient2.Subscribe<TRedisPacket>("a.b.c", packet =>
        {
            Assert.Equal("zhangsan", packet.Name);
            Assert.Equal("ABC", Encoding.ASCII.GetString(packet.Data));
        });

        redisClient2.Subscribe<TRedisPacket>("a.b.c", packet =>
        {
            Assert.Equal("zhangsan", packet.Name);
            Assert.Equal("ABC", Encoding.ASCII.GetString(packet.Data));
        });

        redisClient1.Publish("a.b.c", new TRedisPacket
        {
            Name = "zhangsan",
            Data = Encoding.ASCII.GetBytes("ABC")
        });
    }
}
