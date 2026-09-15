using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;
using VgEAPClient.Common;

namespace VgEAPClient.Recipe.JiangZhouChongDa;

internal class JiangZhouChongDaSetup : IPluginSetupItem
{
    public JiangZhouChongDaSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IWorkOrderRecipeLoader, JiangZhouChongDaRecipeLoader>();
    }
}
