using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VgAutoDrill.Infrastructure.Plugin;

public static class HostBuilderExtensions
{
    public static void AddPlugin(this IHostBuilder hostBuilder, IServiceCollection services, HostBuilderContext hostBuilderContext)
    {
        var options = hostBuilderContext.Configuration.GetSection(nameof(PluginSetupOptions)).Get<PluginSetupOptions>();
        if (options == null) return;

        var plugins = PluginSetupSelector.LoadFrom(options);
        if (plugins.Any())
        {
            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetFileProvider(hostBuilderContext.HostingEnvironment.ContentRootFileProvider)
                        .AddConfiguration(hostBuilderContext.Configuration);

            foreach (var plugin in plugins)
            {
                plugin.Install(services, configBuilder);
            }

            configBuilder.Build();
            hostBuilderContext.Configuration = configBuilder.Build();
        }
    }
}
