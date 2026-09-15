using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.AgentHub.Mqtt;

internal class MqttMessageDispatcher : IMqttMessageDispatcher
{
    private readonly Dictionary<string, IMqttMessageHandler> services = new();
    private readonly IMqttClient _mqttClient;
    private readonly IMqttMessageHandlerFactory _agentHandlerFactory;
    private const string ALL_VEGA_MQTT_MSG = "vega/mqtt/#";

    public MqttMessageDispatcher(IMqttClient mqttClient,
        IMqttMessageHandlerFactory agentHandlerFactory)
    {
        _mqttClient = mqttClient;
        _agentHandlerFactory = agentHandlerFactory;
        services.Add(ALL_VEGA_MQTT_MSG, _agentHandlerFactory.CreateHandler(ALL_VEGA_MQTT_MSG));
    }

    public IMqttMessageHandler this[string topic]
    {
        get { return services[topic]; }
    }

    public async Task Start()
    {
        await SubscribeService(Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.OnlineTopicTemplate), _agentHandlerFactory.CreateHandler(Topics.Upstream.V2.OnlineTopicTemplate));
        await SubscribeService(Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.EventTopicTemplate), _agentHandlerFactory.CreateHandler(Topics.Upstream.V2.EventTopicTemplate));
        await SubscribeService(Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.StatusTopicTemplate), _agentHandlerFactory.CreateHandler(Topics.Upstream.V2.StatusTopicTemplate));
        await SubscribeService(Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.AlarmTopicTemplate), _agentHandlerFactory.CreateHandler(Topics.Upstream.V2.AlarmTopicTemplate));
        await SubscribeService(Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.PropertyTopicTemplate), _agentHandlerFactory.CreateHandler(Topics.Upstream.V2.PropertyTopicTemplate));
        //await SubscribeService(Topics.Downstream.GetSubscribeTopic(Topics.Downstream.V2.ServiceInvokeTopicTemplate), _agentHandlerFactory.CreateHandler(Topics.Downstream.V2.ServiceInvokeTopicTemplate));//pass through service inteception
    }

    public IMqttMessageHandler SelectHandler(string topic)
    {
        var subscribeTopics = new List<string>()
        {
            //Topics.Downstream.GetSubscribeTopic(Topics.Downstream.V2.ServiceInvokeTopicTemplate),//pass through service inteception
            Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.OnlineTopicTemplate),
            Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.EventTopicTemplate),
            Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.StatusTopicTemplate),
            Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.AlarmTopicTemplate),
            Topics.Upstream.GetSubscribeTopic(Topics.Upstream.V2.PropertyTopicTemplate)
        };

        foreach (var subscribeTopic in subscribeTopics)
        {
            if (MqttTopicFilterComparer.Compare(topic, subscribeTopic) == MqttTopicFilterCompareResult.IsMatch)
            {
                return services[subscribeTopic];
            }
        }

        return services[ALL_VEGA_MQTT_MSG];
    }

    private async Task SubscribeService(string serviceTopic, IMqttMessageHandler? mqttMessageHandler)
    {
        if (string.IsNullOrEmpty(serviceTopic)) return;
        if (mqttMessageHandler == null) return;

        if (services.TryAdd(serviceTopic, mqttMessageHandler))
        {
            await _mqttClient.SubscribeAsync(serviceTopic, MqttQualityOfServiceLevel.AtLeastOnce);
        }
    }
}
