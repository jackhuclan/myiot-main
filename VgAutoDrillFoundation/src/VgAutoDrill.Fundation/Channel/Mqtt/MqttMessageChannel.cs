using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Channel.Mqtt;

/// <inheritdoc/>
public class MqttMessageChannel : IMessageChannel
{
    private readonly ILogger<MqttMessageChannel> _logger;
    private readonly ClickHouseAccessor _clickHouseAccessor;
    private readonly IMqttMessagePublisher _mqttMessagePublisher;
    private readonly IDeviceProvider _deviceProvider;

    public event Func<DeviceEventReportRequest, Task>? OnDeviceEventReport;
    public event Func<DevicePropertiesReportRequest, Task>? OnDevicePropertiesReport;
    public event Func<DeviceServiceInvokeRequest, Task>? OnDeviceServiceReport;
    public event Func<DeviceStatusReportRequest, Task>? OnDeviceStatusReport;
    public event Func<DeviceAlarmReportRequest, Task>? OnDeviceAlarmReport;
    public event Func<DevicePanelChangedRequest, Task>? OnDevicePanelChanged;
    public event Func<DeviceCutterTrayChangedRequest, Task>? OnDeviceCutterTrayChanged;
    public event Func<DeviceBrokenToolFaultRequest, Task>? OnDeviceBrokenToolFault;

    /// <summary>
    /// constuct MqttHubExporter
    /// </summary>
    /// <param name="options"></param>
    /// <param name="mqttMessagePublisher"></param>
    /// <param name="deviceProvider"></param>
    /// <param name="loggerFactory"></param>
    public MqttMessageChannel(IOptions<ClickHouseOptions> options,
        IMqttMessagePublisher mqttMessagePublisher,
        IDeviceProvider deviceProvider,
        ILoggerFactory loggerFactory)
    {
        _clickHouseAccessor = new ClickHouseAccessor(options, loggerFactory);
        _logger = loggerFactory.CreateLogger<MqttMessageChannel>();
        _mqttMessagePublisher = mqttMessagePublisher;
        _deviceProvider = deviceProvider;
    }

    /// <inheritdoc/>
    public async Task<DeviceEventReportResponse> DeviceEventReport(DeviceEventReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.EventId);

        var serviceTopic = Topics.Upstream.EventTopic;
        var replyTopic = serviceTopic + "/reply";
        var response = await _mqttMessagePublisher.Publish<DeviceEventReportRequest, DeviceEventReportResponse>(serviceTopic, replyTopic, request);
        if (response == null) response = new DeviceEventReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceEventReport?.Invoke(request);

        _ = _clickHouseAccessor.DeviceEventReport(request, response);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DevicePropertiesReportResponse> DevicePropertiesReport(DevicePropertiesReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var serviceTopic = Topics.Upstream.PropertyTopic;
        var replyTopic = serviceTopic + "/reply";
        var response = await _mqttMessagePublisher.Publish<DevicePropertiesReportRequest, DevicePropertiesReportResponse>(serviceTopic, replyTopic, request);
        if (response == null) response = new DevicePropertiesReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDevicePropertiesReport?.Invoke(request);

        response = new DevicePropertiesReportResponse()
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty
        };

        await _clickHouseAccessor.DevicePropertiesReport(request, response);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeviceServiceInvokeResponse> DeviceServiceReport(DeviceServiceInvokeRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetDeviceId);

        var serviceTopic = Topics.Upstream.ServiceTopic;
        var replyTopic = serviceTopic + "/reply";
        var response = await _mqttMessagePublisher.Publish<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(serviceTopic, replyTopic, request);

        if (response == null) response = new DeviceServiceInvokeResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceServiceReport?.Invoke(request);

        _ = _clickHouseAccessor.DeviceServiceReport(request, response);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeviceStatusReportResponse> DeviceStatusReport(DeviceStatusReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var serviceTopic = Topics.Upstream.StatusTopic;
        var replyTopic = serviceTopic + "/reply";
        var response = await _mqttMessagePublisher.Publish<DeviceStatusReportRequest, DeviceStatusReportResponse>(serviceTopic, replyTopic, request);

        if (response == null) response = new DeviceStatusReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceStatusReport?.Invoke(request);

        _ = _clickHouseAccessor.DeviceStatusReport(request, response);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeviceAlarmReportResponse> DeviceAlarmReport(DeviceAlarmReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var serviceTopic = Topics.Upstream.AlarmTopic;
        var replyTopic = serviceTopic + "/reply";
        var response = await _mqttMessagePublisher.Publish<DeviceAlarmReportRequest, DeviceAlarmReportResponse>(serviceTopic, replyTopic, request);

        if (response == null) response = new DeviceAlarmReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceAlarmReport?.Invoke(request);

        response = new DeviceAlarmReportResponse()
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty
        };

        _ = _clickHouseAccessor.DeviceAlarmReport(request, response);

        return response;
    }

    public async Task<DevicePanelChangedResponse> DevicePanelChangedReport(DevicePanelChangedRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var serviceTopic = Topics.Upstream.PanelTopic;
        var replyTopic = serviceTopic + "/reply";
        var response = await _mqttMessagePublisher.Publish<DevicePanelChangedRequest, DevicePanelChangedResponse>(serviceTopic, replyTopic, request);

        if (response == null) response = new DevicePanelChangedResponse() { Code = ErrorCodes.Sys.FAIL };

        response = new DevicePanelChangedResponse()
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty
        };

        return response;
    }

    public async Task<DeviceCutterTrayChangedResponse> DeviceCutterTrayChangedReport(DeviceCutterTrayChangedRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var serviceTopic = Topics.Upstream.CutterTopic;
        var replyTopic = serviceTopic + "/reply";
        var response = await _mqttMessagePublisher.Publish<DeviceCutterTrayChangedRequest, DeviceCutterTrayChangedResponse>(serviceTopic, replyTopic, request);

        if (response == null) response = new DeviceCutterTrayChangedResponse() { Code = ErrorCodes.Sys.FAIL };

        response = new DeviceCutterTrayChangedResponse()
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty
        };

        return response;
    }

    public Task<DeviceBrokenToolFaultResponse> DeviceBrokenToolFaultReport(DeviceBrokenToolFaultRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = new DeviceBrokenToolFaultResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceBrokenToolFault?.Invoke(request);

        return Task.FromResult(response);
    }
}
