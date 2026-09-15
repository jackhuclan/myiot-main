using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Application.Redis;

namespace VgAutoDrill.Admin.WebApi.Configurations;

public static class AdminApplicationSetup
{
    public static void AddAdminSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisCacheOptions>(configuration.GetSection(RedisCacheOptions.Options));

        RedisCacheOptions? redisCacheOptions = configuration.GetSection(RedisCacheOptions.Options).Get<RedisCacheOptions>();
        if (redisCacheOptions == null)
        {
            ThrowHelper.ThrowArgumentException(nameof(RedisCacheOptions));
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisCacheOptions.ConnectionString;
            //options.InstanceName = redisCacheOptions.InstanceName;
            options.ConfigurationOptions = StackExchange.Redis.ConfigurationOptions.Parse(redisCacheOptions.ConnectionString);
        });

        services.AddSingleton<IRedisClient, RedisClient>();
    }
}
