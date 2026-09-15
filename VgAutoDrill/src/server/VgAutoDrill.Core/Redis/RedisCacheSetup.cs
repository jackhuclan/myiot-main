using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Core
{
    public static class RedisCacheSetup
    {
        public static void AddRedisCacheSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection(RedisCacheOptions.Options);
            services.Configure<RedisCacheOptions>(configurationSection);
            RedisCacheOptions? redisCacheOptions = configuration.GetSection(RedisCacheOptions.Options).Get<RedisCacheOptions>();
            if (redisCacheOptions == null)
            {
                throw new NotImplementedException($"redisCacheOptions is not defined in the configure file.");
            }

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisCacheOptions.ConnectionString;
                //options.InstanceName = redisCacheOptions.InstanceName;
                //options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
                //{
                //    Password = redisCacheOptions.Password,
                //};
            });
        }
    }
}
