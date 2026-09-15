using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Infrastructure.Plugin;
using VgEAPClient.Common.Communication.Outbound;

namespace VgSQLServerStore.Plugin;

public class VgSQLServerStorePluginSetup : IPluginSetupItem
{
    public VgSQLServerStorePluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
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

        services.Configure<VgSQLServerStoreOptions>(configuration.GetSection(nameof(VgSQLServerStoreOptions)));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpDataReporter, VgSQLServerStoreDataReporter>());
    }
}
