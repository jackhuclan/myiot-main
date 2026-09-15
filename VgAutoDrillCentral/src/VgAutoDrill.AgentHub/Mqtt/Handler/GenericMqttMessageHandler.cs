using MQTTnet.Client;

namespace VgAutoDrill.AgentHub.Mqtt.Handler;

internal class GenericMqttMessageHandler : IMqttMessageHandler
{
    public Task<object> Handle(MqttApplicationMessageReceivedEventArgs applicationMessageArgs) => throw new NotImplementedException();
}
