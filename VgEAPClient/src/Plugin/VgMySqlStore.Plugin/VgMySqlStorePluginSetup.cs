using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Infrastructure.Plugin;
using VgEAPClient.Common.Communication.Outbound;

namespace VgMySqlStore.Plugin;

public class VgMySqlStorePluginSetup : IPluginSetupItem
{
    public VgMySqlStorePluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.aoi.json");
            configurationBuilder.AddJsonFile(ajwJsonPath, false);
        }

        services.Configure<VgMySqlStoreOptions>(configuration.GetSection(nameof(VgMySqlStoreOptions)));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpDataReporter, VgMySqlStoreDataReporter>());
    }
}
