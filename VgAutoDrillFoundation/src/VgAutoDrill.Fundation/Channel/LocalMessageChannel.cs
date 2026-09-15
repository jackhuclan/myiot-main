using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.OpenAPI;

namespace VgAutoDrill.Fundation.Channel;

/// <summary>
/// 本地模式，用于本地调试或者只向外部监听者发送数据
/// </summary>
internal class LocalMessageChannel : IMessageChannel
{
    private readonly ILogger<LocalMessageChannel> _logger;
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
    /// 
    /// </summary>
    /// <param name="httpRequestInvoker"></param>
    /// <param name="deviceProvider"></param>
    /// <param name="loggerFactory"></param>
    public LocalMessageChannel(IHttpRequestInvoker httpRequestInvoker,
        IDeviceProvider deviceProvider,
        ILoggerFactory loggerFactory)
    {
        _deviceProvider = deviceProvider;
        _logger = loggerFactory.CreateLogger<LocalMessageChannel>();
    }

    /// <inheritdoc/>
    public Task<DeviceEventReportResponse> DeviceEventReport(DeviceEventReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.EventId);

        var response = new DeviceEventReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceEventReport?.Invoke(request);

        return Task.FromResult(response);
    }

    /// <inheritdoc/>
    public Task<DevicePropertiesReportResponse> DevicePropertiesReport(DevicePropertiesReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = new DevicePropertiesReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDevicePropertiesReport?.Invoke(request);

        return Task.FromResult(response);
    }

    /// <inheritdoc/>
    public Task<DeviceServiceInvokeResponse> DeviceServiceReport(DeviceServiceInvokeRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.TargetDeviceId);

        var response = new DeviceServiceInvokeResponse()
        {
            Code = ErrorCodes.Sys.FAIL,
            Message = $"DeviceServiceReport http return null response",
        };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceServiceReport?.Invoke(request);

        return Task.FromResult(response);
    }

    /// <inheritdoc/>
    public Task<DeviceStatusReportResponse> DeviceStatusReport(DeviceStatusReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = new DeviceStatusReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceStatusReport?.Invoke(request);

        return Task.FromResult(response);
    }

    /// <inheritdoc/>
    public Task<DeviceAlarmReportResponse> DeviceAlarmReport(DeviceAlarmReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = new DeviceAlarmReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceAlarmReport?.Invoke(request);

        return Task.FromResult(response);
    }

    public Task<DevicePanelChangedResponse> DevicePanelChangedReport(DevicePanelChangedRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = new DevicePanelChangedResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDevicePanelChanged?.Invoke(request);

        return Task.FromResult(response);
    }

    public Task<DeviceCutterTrayChangedResponse> DeviceCutterTrayChangedReport(DeviceCutterTrayChangedRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = new DeviceCutterTrayChangedResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceCutterTrayChanged?.Invoke(request);

        return Task.FromResult(response);
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
