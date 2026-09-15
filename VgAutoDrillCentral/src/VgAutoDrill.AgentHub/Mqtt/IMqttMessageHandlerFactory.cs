namespace VgAutoDrill.AgentHub.Mqtt;

internal interface IMqttMessageHandlerFactory
{
    IMqttMessageHandler CreateHandler(string topic);
}
