using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.Chongda;

/// <summary>
/// 崇达钻机设备对外数据上报接口
/// </summary>
public class ChongdaDrillDataReport : IExternalDataReport
{
    private readonly ChongdaOptions _chongdaOptions;
    private readonly ILogger<ChongdaDrillDataReport> _logger;
    private readonly IHttpRequestInvoker _httpRequestInvoker;

    public Device ReportingDevice { get; }

    public ChongdaDrillDataReport(IOptions<ChongdaOptions> options,
        IHttpRequestInvoker httpRequestInvoker,
        ILoggerFactory loggerFactory,
        Device device)
    {
        _chongdaOptions = options.Value;
        _logger = loggerFactory.CreateLogger<ChongdaDrillDataReport>();
        _httpRequestInvoker = httpRequestInvoker;
        ReportingDevice = device;
    }

    public Task OnDeviceEventReport(DeviceEventReportRequest request)
    {
        return Task.CompletedTask;
    }

    public Task OnDevicePropertiesReport(DevicePropertiesReportRequest request)
    {
        return Task.CompletedTask;
    }

    public Task OnDeviceServiceReport(DeviceServiceInvokeRequest request)
    {
        return Task.CompletedTask;
    }

    public Task OnDeviceStatusReport(DeviceStatusReportRequest request)
    {
        return Task.CompletedTask;
    }

    public Task OnDeviceAlarmReport(DeviceAlarmReportRequest request)
    {
        return Task.CompletedTask;
    }

    public Task OnDevicePanelChanged(DevicePanelChangedRequest request)
    {
        return Task.CompletedTask;
    }

    public Task OnDeviceCutterTrayChanged(DeviceCutterTrayChangedRequest request)
    {
        return Task.CompletedTask;
    }

    public Task OnDeviceBrokenToolFault(DeviceBrokenToolFaultRequest request)
    {
        return Task.CompletedTask;
    }
}
