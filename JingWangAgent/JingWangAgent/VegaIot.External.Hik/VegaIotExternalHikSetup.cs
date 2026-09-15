using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.Hik;

public class VegaIotExternalHikSetup : IPluginSetupItem
{
    public VegaIotExternalHikSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHostedService<HikAgvTaskMonitor>();
    }
}
