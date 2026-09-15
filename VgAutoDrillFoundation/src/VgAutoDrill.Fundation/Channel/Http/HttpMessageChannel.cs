using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.OpenAPI;

namespace VgAutoDrill.Fundation.Channel.Http;

/// <inheritdoc/>
internal class HttpMessageChannel : IMessageChannel
{
    private readonly ILogger<HttpMessageChannel> _logger;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly IDeviceProvider _deviceProvider;
    private readonly CentralWebOptions _centralWebOptions;
    private readonly ClickHouseAccessor _clickHouseAccessor;

    public event Func<DeviceEventReportRequest, Task>? OnDeviceEventReport;
    public event Func<DevicePropertiesReportRequest, Task>? OnDevicePropertiesReport;
    public event Func<DeviceServiceInvokeRequest, Task>? OnDeviceServiceReport;
    public event Func<DeviceStatusReportRequest, Task>? OnDeviceStatusReport;
    public event Func<DeviceAlarmReportRequest, Task>? OnDeviceAlarmReport;
    public event Func<DevicePanelChangedRequest, Task>? OnDevicePanelChanged;
    public event Func<DeviceCutterTrayChangedRequest, Task>? OnDeviceCutterTrayChanged;
    public event Func<DeviceBrokenToolFaultRequest, Task>? OnDeviceBrokenToolFault;

    /// <summary>
    /// constuct ClickhouseExporter
    /// </summary>
    /// <param name="options"></param>
    /// <param name="webOptions"></param>
    /// <param name="httpRequestInvoker"></param>
    /// <param name="deviceProvider"></param>
    /// <param name="loggerFactory"></param>
    public HttpMessageChannel(IOptions<ClickHouseOptions> options,
        IOptions<CentralWebOptions> webOptions,
        IHttpRequestInvoker httpRequestInvoker,
        IDeviceProvider deviceProvider,
        ILoggerFactory loggerFactory)
    {
        _centralWebOptions = webOptions.Value;
        _httpRequestInvoker = httpRequestInvoker;
        _deviceProvider = deviceProvider;
        _clickHouseAccessor = new ClickHouseAccessor(options, loggerFactory);
        _logger = loggerFactory.CreateLogger<HttpMessageChannel>();
    }

    /// <inheritdoc/>
    public async Task<DeviceEventReportResponse> DeviceEventReport(DeviceEventReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.EventId);

        var response = await _httpRequestInvoker.PostAsJsonAsync<DeviceEventReportRequest, DeviceEventReportResponse>(_centralWebOptions.EventReport, request);
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

        var response = await _httpRequestInvoker.PostAsJsonAsync<DevicePropertiesReportRequest, DevicePropertiesReportResponse>(_centralWebOptions.PropertiesReport, request);
        if (response == null) response = new DevicePropertiesReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDevicePropertiesReport?.Invoke(request);

        response = new DevicePropertiesReportResponse()
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty
        };

        _ = _clickHouseAccessor.DevicePropertiesReport(request, response);

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

        var serviceUrl = _centralWebOptions.ServiceInvokeV2;

        switch (request.ServiceId)
        {
            default:
                break;
            case Topics.Services.PREPARE_LOAD_MATERIAL_SERVICE_ID:
            case Topics.Services.PREPARE_UNLOAD_MATERIAL_SERVICE_ID:
                serviceUrl = _centralWebOptions.ServicePrepare;
                break;
            case Topics.Services.INVOKE_LOAD_MATERIAL_SERVICE_ID:
            case Topics.Services.INVOKE_UNLOAD_MATERIAL_SERVICE_ID:
                serviceUrl = _centralWebOptions.ServiceInvoke;
                break;
            case Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID:
            case Topics.Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID:
                serviceUrl = _centralWebOptions.ServiceComplete;
                break;
        }

        var response = await _httpRequestInvoker.PostAsJsonAsync<DeviceServiceInvokeRequest, DeviceServiceInvokeResponse>(serviceUrl, request);
        if (response == null) response = new DeviceServiceInvokeResponse()
        {
            Code = ErrorCodes.Sys.FAIL,
            Message = $"DeviceServiceReport http return null response",
        };

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

        var requestContent = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            IgnoreReadOnlyProperties = true,
        });
        _logger.LogDebug($"requestContent,{requestContent}");

        var response = await _httpRequestInvoker.PostAsJsonAsync<DeviceStatusReportRequest, DeviceStatusReportResponse>(_centralWebOptions.StatusReport, request);
        if (response == null) response = new DeviceStatusReportResponse() { Code = ErrorCodes.Sys.FAIL };

        _ = _clickHouseAccessor.DeviceStatusReport(request, response);

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceStatusReport?.Invoke(request);

        return response;
    }

    /// <inheritdoc/>
    public async Task<DeviceAlarmReportResponse> DeviceAlarmReport(DeviceAlarmReportRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = await _httpRequestInvoker.PostAsJsonAsync<DeviceAlarmReportRequest, DeviceAlarmReportResponse>(_centralWebOptions.AlarmReport, request);
        if (response == null) response = new DeviceAlarmReportResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceAlarmReport?.Invoke(request);

        _ = _clickHouseAccessor.DeviceAlarmReport(request, response);

        return response;
    }

    public async Task<DevicePanelChangedResponse> DevicePanelChangedReport(DevicePanelChangedRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = await _httpRequestInvoker.PostAsJsonAsync<DevicePanelChangedRequest, DevicePanelChangedResponse>(_centralWebOptions.PanelReport, request);
        if (response == null) response = new DevicePanelChangedResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDevicePanelChanged?.Invoke(request);

        return response;
    }

    public async Task<DeviceCutterTrayChangedResponse> DeviceCutterTrayChangedReport(DeviceCutterTrayChangedRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = await _httpRequestInvoker.PostAsJsonAsync<DeviceCutterTrayChangedRequest, DeviceCutterTrayChangedResponse>(_centralWebOptions.CutterReport, request);
        if (response == null) response = new DeviceCutterTrayChangedResponse() { Code = ErrorCodes.Sys.FAIL };

        var device = _deviceProvider.GetDevice(request.DeviceId);
        if (device != null && device.DeviceDescriptor.ExternalDataReportEnabled)
            OnDeviceCutterTrayChanged?.Invoke(request);

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
