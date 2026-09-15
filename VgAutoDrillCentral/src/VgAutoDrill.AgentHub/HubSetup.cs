using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client;
using VgAutoDrill.AgentHub.Mqtt;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Mqtt.Client;

namespace VgAutoDrill.AgentHub;

public static class HubSetup
{
    public static void AddHub(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCentralCore(configuration);

        services.Configure<MqttServerConnectionOptions>(configuration.GetSection(nameof(MqttServerConnectionOptions)));
        services.AddMqttClient();
        services.AddHostedService<HubMqttClient>();
        services.AddSingleton<IMqttMessageDispatcher, MqttMessageDispatcher>();
        services.AddSingleton<IMqttMessageHandlerFactory, MqttMessageHandlerFactory>();
        services.AddSingleton<IDeviceServiceInvoker, HubDeviceServiceInvoker>();
        services.AddSingleton<IMqttMessagePublisher, MqttMessagePublisher>();
    }

    private static void AddMqttClient(this IServiceCollection services)
    {
        var mqttFactory = new MqttFactory();
        IMqttClient mqttClient = mqttFactory.CreateMqttClient();
        services.AddSingleton(mqttClient);
    }
}
