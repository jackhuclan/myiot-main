using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.HikAgv;

public class HikAgvPluginSetup : IPluginSetupItem
{
    public HikAgvPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var hikJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "conf", "appsettings.hik.json");
            configurationBuilder.AddJsonFile(hikJsonPath, false);
        }
        services.AddHostedService<HikAgvScheduler>();
        services.AddHostedService<HikAgvTaskMonitor>();
        services.Configure<HikAgvAgentOptions>(configuration.GetSection(nameof(HikAgvAgentOptions)));
        services.Configure<HikAgvSchedulerOptions>(configuration.GetSection(nameof(HikAgvSchedulerOptions)));
        services.AddSingleton<HikAgvConfig>();
        services.AddControllers().AddApplicationPart(typeof(HikAgvPluginSetup).Assembly);
    }
}
