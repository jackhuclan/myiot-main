using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.ShenzhenKinwong;

public class KingwongPluginSetup : IPluginSetupItem
{
    public KingwongPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IPanelBarCodeValidator, KinwongPanelBarCodeValidator>();
    }
}
