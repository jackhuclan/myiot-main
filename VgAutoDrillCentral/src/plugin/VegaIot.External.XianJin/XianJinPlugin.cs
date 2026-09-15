using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.XianJin;

public class XianJinPlugin : IPluginSetupItem
{
    public XianJinPlugin(PluginSetupDescriptor pluginSetupDescriptor)
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

        services.Configure<XianJinOptions>(configuration.GetSection(nameof(XianJinOptions)));
        services.AddControllers().AddApplicationPart(typeof(XianJinPlugin).Assembly);
    }
}
