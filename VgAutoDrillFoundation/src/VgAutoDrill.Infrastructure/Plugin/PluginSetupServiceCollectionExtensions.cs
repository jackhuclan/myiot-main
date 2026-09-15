using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VgAutoDrill.Infrastructure.Plugin;

public static class PluginSetupServiceCollectionExtensions
{
    public static void AddPlugin(this IServiceCollection services, IConfigurationManager configuration)
    {
        var options = configuration.GetSection(nameof(PluginSetupOptions)).Get<PluginSetupOptions>();
        if (options == null) return;

        var plugins = PluginSetupSelector.LoadFrom(options);
        if (plugins.Any())
        {
            foreach (var plugin in plugins)
            {
                plugin.Install(services, configuration);
            }
        }
    }
}
