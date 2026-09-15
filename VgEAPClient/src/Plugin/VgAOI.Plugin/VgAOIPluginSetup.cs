using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Infrastructure.Plugin;
using VgEAPClient.Common.Communication.Outbound;

namespace VgAOI.Plugin;

public class VgAOIPluginSetup : IPluginSetupItem
{
    public VgAOIPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<VgAOIOptions>(configuration.GetSection(nameof(VgAOIOptions)));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IEQPDataReporter, VgAOIHttpDataReporter>());
    }
}
