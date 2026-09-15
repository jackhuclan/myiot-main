using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.AgentHub.Mqtt.Handler;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.AgentHub.Mqtt;

internal class MqttMessageHandlerFactory : IMqttMessageHandlerFactory
{
    private readonly IServiceProvider _provider;

    public MqttMessageHandlerFactory(IServiceProvider provider)
    {
        this._provider = provider;
    }

    public IMqttMessageHandler CreateHandler(string topic)
    {
        switch (topic)
        {
            case Topics.Upstream.V2.OnlineTopicTemplate:
                return (IMqttMessageHandler)ActivatorUtilities.CreateInstance(_provider, typeof(OnlineMqttMessageHandler));
            case Topics.Upstream.V2.EventTopicTemplate:
                return (IMqttMessageHandler)ActivatorUtilities.CreateInstance(_provider, typeof(EventMqttMessageHandler));
            case Topics.Upstream.V2.StatusTopicTemplate:
                return (IMqttMessageHandler)ActivatorUtilities.CreateInstance(_provider, typeof(StatusMqttMessageHandler));
            case Topics.Upstream.V2.AlarmTopicTemplate:
                return (IMqttMessageHandler)ActivatorUtilities.CreateInstance(_provider, typeof(AlarmMqttMessageHandler));
            case Topics.Upstream.V2.PropertyTopicTemplate:
                return (IMqttMessageHandler)ActivatorUtilities.CreateInstance(_provider, typeof(PropertyMqttMessageHandler));
            case Topics.Downstream.V2.ServiceInvokeTopicTemplate:
                return (IMqttMessageHandler)ActivatorUtilities.CreateInstance(_provider, typeof(ServiceMqttMessageHandler));
            default:
                return (IMqttMessageHandler)ActivatorUtilities.CreateInstance(_provider, typeof(GenericMqttMessageHandler));
        }

    }
}
