using MQTTnet.Client;

namespace VgAutoDrill.AgentHub.Mqtt;

internal interface IMqttMessageHandler
{
    Task<object> Handle(MqttApplicationMessageReceivedEventArgs applicationMessageArgs);
}
