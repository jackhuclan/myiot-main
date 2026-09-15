using MQTTnet.Client;

namespace VgAutoDrill.AgentHub.Mqtt.Handler;

internal class PropertyMqttMessageHandler : IMqttMessageHandler
{
    public PropertyMqttMessageHandler() { }
    public Task<object> Handle(MqttApplicationMessageReceivedEventArgs args) => throw new NotImplementedException();
}
