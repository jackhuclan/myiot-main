using System.Text.Json;
using MQTTnet.Client;
using VgAutoDrill.Central.Core.Reporter;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.AgentHub.Mqtt.Handler;

internal class StatusMqttMessageHandler : IMqttMessageHandler
{
    private readonly IDeviceStatusReporter _deviceStatusHandler;

    public StatusMqttMessageHandler(IDeviceStatusReporter deviceStatusHandler)
    {
        _deviceStatusHandler = deviceStatusHandler;
    }

    public async Task<object> Handle(MqttApplicationMessageReceivedEventArgs args)
    {
        var deviceStatusReportRequest = JsonSerializer.Deserialize<DeviceStatusReportRequest>(args.ApplicationMessage.PayloadSegment);
        var response = await _deviceStatusHandler.Report(deviceStatusReportRequest);
        args.ApplicationMessage.PayloadSegment = JsonSerializer.Serialize(response).GetBytes();
        return response;
    }
}
