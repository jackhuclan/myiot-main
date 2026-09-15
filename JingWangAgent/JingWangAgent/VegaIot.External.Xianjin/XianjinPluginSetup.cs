using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.Xianjin
{
    public class XianjinPluginSetup : IPluginSetupItem
    {
        public XianjinPluginSetup(PluginSetupDescriptor pluginSetupDescriptor)
        {
            PluginDescriptor = pluginSetupDescriptor;
        }

        public PluginSetupDescriptor PluginDescriptor { get; }

        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is IConfigurationBuilder configurationBuilder)
            {
                var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.xianjin.json");
                configurationBuilder.AddJsonFile(ajwJsonPath, false);
            }

            services.Configure<FtpDrillFilePathOptions>(configuration.GetSection(nameof(FtpDrillFilePathOptions)));
            services.AddSingleton<IDrillFilePathLocator, XianjinDrillFileLocator>();
        }
    }
}
