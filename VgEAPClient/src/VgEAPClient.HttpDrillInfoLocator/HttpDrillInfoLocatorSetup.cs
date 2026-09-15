using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;
using VgEAPClient.Common;

namespace VgEAPClient.HttpDrillInfoLocator;

internal class HttpDrillInfoLocatorSetup : IPluginSetupItem
{
    public HttpDrillInfoLocatorSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IWorkOrderRecipeLoader, HttpDrillInfoLocatorReporter>();
    }
}
