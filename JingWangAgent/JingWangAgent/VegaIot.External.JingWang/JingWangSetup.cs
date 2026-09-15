using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.JingWang;

public class JingWangSetup : IPluginSetupItem
{
    public JingWangSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.JingWang.json");
            configurationBuilder.AddJsonFile(jsonPath, false);
        }
        services.Configure<JingWangExternalMysqlOptions>(configuration.GetSection(nameof(JingWangExternalMysqlOptions)));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IExternalDataReportFactory, JingWangExternalMysqlDataReportFactory>());
    }
}
