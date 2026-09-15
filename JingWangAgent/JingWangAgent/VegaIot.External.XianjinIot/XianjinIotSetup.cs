using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Infrastructure.Plugin;

namespace VegaIot.External.XianjinIot;

public class XianjinIotSetup : IPluginSetupItem
{
    public XianjinIotSetup(PluginSetupDescriptor pluginSetupDescriptor)
    {
        PluginDescriptor = pluginSetupDescriptor;
    }

    public PluginSetupDescriptor PluginDescriptor { get; }

    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is IConfigurationBuilder configurationBuilder)
        {
            var ajwJsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.xianjiniot.json");
            configurationBuilder.AddJsonFile(ajwJsonPath, false);
        }

        services.Configure<XianJinIotOptions>(configuration.GetSection(nameof(XianJinIotOptions)));
        services.AddSingleton<IClientMqttListener, XianJinIotClientMqttListener>();
        services.AddSingleton<IClientMqttApplicationMessageListener, XianjinIClientMqttApplicationMessageListner>();
    }
}
