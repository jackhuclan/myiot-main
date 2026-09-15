using MQTTnet.Client;

namespace VgAutoDrill.AgentHub.Mqtt.Handler;

internal class AlarmMqttMessageHandler : IMqttMessageHandler
{
    public AlarmMqttMessageHandler() { }
    public Task<object> Handle(MqttApplicationMessageReceivedEventArgs applicationMessageArgs) => throw new NotImplementedException();
}
