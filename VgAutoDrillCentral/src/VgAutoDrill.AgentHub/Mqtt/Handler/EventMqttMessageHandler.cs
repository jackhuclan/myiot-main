using System.Text.Json;
using MQTTnet.Client;
using VgAutoDrill.Central.Core.Reporter.Event;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.AgentHub.Mqtt.Handler;

internal class EventMqttMessageHandler : IMqttMessageHandler
{
    private readonly IDeviceEventReporter _deviceEventHandler;

    public EventMqttMessageHandler(IDeviceEventReporter deviceEventHandler)
    {
        _deviceEventHandler = deviceEventHandler;
    }

    public async Task<object> Handle(MqttApplicationMessageReceivedEventArgs args)
    {
        var deviceEventReportRequest = JsonSerializer.Deserialize<DeviceEventReportRequest>(args.ApplicationMessage.PayloadSegment);
        ThrowHelper.ThrowArgumentNullException(deviceEventReportRequest);

        var response = await _deviceEventHandler.Report(deviceEventReportRequest);
        args.ApplicationMessage.PayloadSegment = JsonSerializer.Serialize(response).GetBytes();
        return response;
    }
}
