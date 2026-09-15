using MQTTnet.Protocol;

namespace VgAutoDrill.Central.Core;

public class DeviceServiceInvocationArgs
{
    public string MessageId { get; set; } = string.Empty;
    public string RequestTopic { get; set; } = string.Empty;
    public string ResponseTopic { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public int Retries { get; set; } = 0;
    public string Reason { get; set; } = string.Empty;
    public string? RoutingKey { get; set; }
    public DateTime FirstInvocationTimestamp { get; set; } = DateTime.Now;
    public DateTime LastInvocationTimestamp { get; set; } = DateTime.Now;
    public MqttQualityOfServiceLevel ServiceLevel { get; set; } = MqttQualityOfServiceLevel.AtMostOnce;
}
