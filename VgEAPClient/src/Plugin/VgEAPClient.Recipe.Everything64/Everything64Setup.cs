using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;
using VgEAPClient.Common;

namespace VgEAPClient.Recipe.Everything64;

internal class Everything64Setup : IPluginSetupItem
{
    public Everything64Setup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IWorkOrderRecipeLoader, Everything64RecipeLoader>();
    }
}
