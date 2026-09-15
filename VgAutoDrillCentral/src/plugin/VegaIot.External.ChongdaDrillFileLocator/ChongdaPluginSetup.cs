using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.ChongdaDrillFileLocator;

public class ChongdaPluginSetup : IPluginSetupItem
{
    public ChongdaPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.chongda.json");
            configurationBuilder.AddJsonFile(ajwJsonPath, false);
        }

        services.Configure<DrillFilePathOptions>(configuration.GetSection(nameof(DrillFilePathOptions)));
        services.AddSingleton<IDrillFilePathLocator, ChongdaDrillFileLocator>();
    }
}
