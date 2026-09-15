namespace VgAutoDrill.AgentHub.Mqtt;

internal interface IMqttMessageDispatcher
{
    IMqttMessageHandler this[string topic] { get; }
    IMqttMessageHandler SelectHandler(string topic);
    Task Start();
}
