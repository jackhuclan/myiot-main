using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace VgAutoDrill.Infrastructure.Plugin;

public static class HostApplicationBuilderExtensions
{
    public static void AddPlugin(this IHostApplicationBuilder hostApplicationBuilder)
    {
        var options = hostApplicationBuilder.Configuration.GetSection(nameof(PluginSetupOptions)).Get<PluginSetupOptions>();
        if (options == null) return;

        var plugins = PluginSetupSelector.LoadFrom(options);
        if (plugins.Any())
        {
            foreach (var plugin in plugins)
            {
                plugin.Install(hostApplicationBuilder.Services, hostApplicationBuilder.Configuration);
            }
        }
    }
}
