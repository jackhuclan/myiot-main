using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VgAutoDrill.Central.Core.Handler;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.ChongdaDrillFileLocator;

public class KinwongPluginSetup : IPluginSetupItem
{
    public KinwongPluginSetup() { PluginName = "KingWongPluginSetup"; }

    public string PluginName { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.kingwong.json");
            configurationBuilder.AddJsonFile(ajwJsonPath, false);
        }

        services.Configure<DrillFilePathOptions>(configuration.GetSection(nameof(DrillFilePathOptions)));
        services.TryAddSingleton(ServiceDescriptor.Singleton<IDrillFilePathLocator, KinwongDrillFileLocator>());
    }
}
