using System.Text.Json;
using MQTTnet.Packets;
using MQTTnet.Server;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Reporter;
using VgAutoDrill.Central.Core.Reporter.Event;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Server;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.WebApi.Core;

public class MqttHandler
{
    private readonly MqttServer _mqttServer;
    private readonly IAsyncTaskWaiter _taskWaiter;
    private readonly IDeviceManager _deviceManager;
    private readonly IDeviceLoginReporter _deviceLoginReporter;
    private readonly IDeviceCutterReporter _deviceCutterReporter;
    private readonly IDevicePanelReporter _devicePanelReporter;
    private readonly IDeviceEventReporter _deviceEventReporter;
    private readonly IDeviceStatusReporter _deviceStatusReporter;
    private readonly IDevicePropertyReporter _devicePropertyReporter;
    private readonly IDeviceServiceReporter _deviceServiceReporter;
    private readonly IDeviceAlarmReporter _deviceAlarmReporter;
    private readonly ILogger<MqttHandler> _logger;

    public MqttHandler(MqttServer mqttServer,
        IAsyncTaskWaiter taskWaiter,
        IDeviceManager deviceManager,
        IDeviceLoginReporter deviceLoginReporter,
        IDeviceCutterReporter deviceCutterReporter,
        IDevicePanelReporter devicePanelReporter,
        IDeviceEventReporter deviceEventReporter,
        IDeviceStatusReporter deviceStatusReporter,
        IDevicePropertyReporter devicePropertyReporter,
        IDeviceServiceReporter deviceServiceReporter,
        IDeviceAlarmReporter deviceAlarmReporter,
        ILoggerFactory loggerFactory)
    {
        _mqttServer = mqttServer;
        _taskWaiter = taskWaiter;
        _deviceManager = deviceManager;
        _deviceLoginReporter = deviceLoginReporter;
        _deviceCutterReporter = deviceCutterReporter;
        _devicePanelReporter = devicePanelReporter;
        _deviceEventReporter = deviceEventReporter;
        _deviceStatusReporter = deviceStatusReporter;
        _devicePropertyReporter = devicePropertyReporter;
        _deviceServiceReporter = deviceServiceReporter;
        _deviceAlarmReporter = deviceAlarmReporter;
        _logger = loggerFactory.CreateLogger<MqttHandler>();
    }

    public Task ValidatingConnectionAsync(ValidatingConnectionEventArgs arg)
    {
        _logger.LogInformation("Server_ValidatingConnectionAsync:" + arg.ClientId);
        return Task.CompletedTask;
    }

    public async Task ClientConnectedAsync(ClientConnectedEventArgs arg)
    {
        _logger.LogInformation("Server_ClientConnectedAsync:" + arg.ClientId);

        await _mqttServer.SubscribeAsync(arg.ClientId, Topics.Upstream.OnlineTopic);
        await _mqttServer.SubscribeAsync(arg.ClientId, Topics.Upstream.EventTopic);
        await _mqttServer.SubscribeAsync(arg.ClientId, Topics.Upstream.StatusTopic);
        await _mqttServer.SubscribeAsync(arg.ClientId, Topics.Upstream.AlarmTopic);
        await _mqttServer.SubscribeAsync(arg.ClientId, Topics.Upstream.PropertyTopic);
        await _mqttServer.SubscribeAsync(arg.ClientId, Topics.Upstream.ServiceTopic);
        await _mqttServer.SubscribeAsync(arg.ClientId, Topics.Upstream.PanelTopic);
        await _mqttServer.SubscribeAsync(arg.ClientId, Topics.Upstream.CutterTopic);
    }

    public async Task ClientDisconnectedAsync(ClientDisconnectedEventArgs arg)
    {
        _logger.LogInformation("Server_ClientDisconnectedAsync:" + arg.ClientId);
        await _deviceManager.RemoveDeviceByClient(arg.ClientId);
    }

    public async Task InterceptingInboundPacketAsync(InterceptingPacketEventArgs arg)
    {
        switch (arg.Packet)
        {
            case MqttPublishPacket:
                await HandleMqttPublishPacket(arg);
                break;
        }
    }

    public async Task HandleMqttPublishPacket(InterceptingPacketEventArgs arg)
    {
        MqttPublishPacket packet = (MqttPublishPacket)arg.Packet;
        _logger.LogDebug("InterceptingInboundPacketAsync:" + packet.Topic);

        switch (packet.Topic)
        {
            case Topics.Upstream.OnlineTopic:
                var onlineRequest = JsonSerializer.Deserialize<OnlineRequest>(packet.PayloadSegment.ToArray());
                var onlineResponse = await _deviceLoginReporter.Report(onlineRequest, arg);
                packet.PayloadSegment = JsonSerializer.Serialize(onlineResponse).GetBytes();
                break;

            case Topics.Upstream.EventTopic:
                var deviceEventReportRequest = JsonSerializer.Deserialize<DeviceEventReportRequest>(packet.PayloadSegment.ToArray());
                var deviceEventReportResponse = await _deviceEventReporter.Report(deviceEventReportRequest);
                packet.PayloadSegment = JsonSerializer.Serialize(deviceEventReportResponse).GetBytes();
                break;

            case Topics.Upstream.PanelTopic:
                var devicePanelChangedRequest = JsonSerializer.Deserialize<DevicePanelChangedRequest>(packet.PayloadSegment.ToArray());
                var devicePanelChangedResponse = await _devicePanelReporter.Report(devicePanelChangedRequest);
                packet.PayloadSegment = JsonSerializer.Serialize(devicePanelChangedResponse).GetBytes();
                break;

            case Topics.Upstream.StatusTopic:
                var deviceStatusReportRequest = JsonSerializer.Deserialize<DeviceStatusReportRequest>(packet.PayloadSegment.ToArray());
                var deviceStatusReportResponse = await _deviceStatusReporter.Report(deviceStatusReportRequest);
                packet.PayloadSegment = JsonSerializer.Serialize(deviceStatusReportResponse).GetBytes();
                break;

            case Topics.Upstream.PropertyTopic:
                var devicePropertiesReportRequest = JsonSerializer.Deserialize<DevicePropertiesReportRequest>(packet.PayloadSegment.ToArray());
                var devicePropertiesReportResponse = await _devicePropertyReporter.Report(devicePropertiesReportRequest);
                packet.PayloadSegment = JsonSerializer.Serialize(devicePropertiesReportResponse).GetBytes();
                break;

            case Topics.Upstream.ServiceTopic:
                var deviceServiceInvokeRequest = JsonSerializer.Deserialize<DeviceServiceInvokeRequest>(packet.PayloadSegment.ToArray());
                var deviceServiceInvokeResponse = await _deviceServiceReporter.Report(deviceServiceInvokeRequest);
                packet.PayloadSegment = JsonSerializer.Serialize(deviceServiceInvokeResponse).GetBytes();
                break;

            case Topics.Upstream.CutterTopic:
                var deviceCutterTrayChangedRequest = JsonSerializer.Deserialize<DeviceCutterTrayChangedRequest>(packet.PayloadSegment.ToArray());
                var deviceCutterTrayChangedResponse = await _deviceCutterReporter.Report(deviceCutterTrayChangedRequest);
                packet.PayloadSegment = JsonSerializer.Serialize(deviceCutterTrayChangedResponse).GetBytes();
                break;

            case Topics.Upstream.AlarmTopic:
                var deviceAlarmReportRequest = JsonSerializer.Deserialize<DeviceAlarmReportRequest>(packet.PayloadSegment.ToArray());
                var deviceAlarmReportResponse = await _deviceAlarmReporter.Report(deviceAlarmReportRequest);
                packet.PayloadSegment = JsonSerializer.Serialize(deviceAlarmReportResponse).GetBytes();
                break;

            default:
                await _taskWaiter.HandleInterceptingInboundPacketAsync(arg);
                break;
        }
    }
}
