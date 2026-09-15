using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Calculator;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.StdAgv;

public class StdAgvPluginSetup : IPluginSetupItem
{
    public StdAgvPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var stdJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.std.json");
            configurationBuilder.AddJsonFile(stdJsonPath, false);
        }
        services.AddHostedService<StdAgvScheduler>();
        services.AddHostedService<StdManualAgvScheduler>();
        services.AddHostedService<StdAgvTaskMonitor>();
        services.Configure<StdAgvAgentOptions>(configuration.GetSection(nameof(StdAgvAgentOptions)));
        services.Configure<StdAgvSchedulerOptions>(configuration.GetSection(nameof(StdAgvSchedulerOptions)));
        services.AddSingleton<StdAgvConfig>();
        services.AddSingleton<ICalcutorUseAPI, StdCalcutorUseAPI>();
        services.AddControllers().AddApplicationPart(typeof(StdAgvPluginSetup).Assembly);
    }
}
