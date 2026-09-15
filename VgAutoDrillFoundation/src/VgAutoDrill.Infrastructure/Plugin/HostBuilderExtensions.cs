using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace VgAutoDrill.Infrastructure.Plugin;

public class HostBuilderExtensions
{
    public static Action<HostBuilderContext, IServiceCollection> ConfigurePlugins = (hostBuilderContext, services) =>
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

            hostBuilderContext.Configuration = configBuilder.Build();
        }
    };
}
