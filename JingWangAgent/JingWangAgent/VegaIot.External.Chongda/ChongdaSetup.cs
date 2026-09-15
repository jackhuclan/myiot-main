using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.Chongda;

public class ChongdaSetup : IPluginSetupItem
{
    public ChongdaSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.Chongda.json");
            configurationBuilder.AddJsonFile(jsonPath, false);
        }
        services.Configure<ChongdaOptions>(configuration.GetSection(nameof(ChongdaOptions)));
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IExternalDataReportFactory, ChongdaExternalDataReportFactory>());
    }
}
