using MQTTnet.Diagnostics;

namespace VgAutoDrill.Fundation.Mqtt.Client;

public interface IMqttClientLogger
{
    void OutputMqttLog(MqttNetLogMessagePublishedEventArgs e);
}
