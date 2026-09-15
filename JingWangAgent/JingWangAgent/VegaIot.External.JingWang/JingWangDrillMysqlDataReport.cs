using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.JingWang;

/// <summary>
/// 景旺钻机设备对外数据上报接口，导出到景旺mysql数据库
/// </summary>
public class JingWangDrillMysqlDataReport : IExternalDataReport
{
    private readonly JingWangExternalMysqlOptions _chongdaOptions;
    private readonly ILogger<JingWangDrillMysqlDataReport> _logger;
    private readonly IHttpRequestInvoker _httpRequestInvoker;

    public Device ReportingDevice { get; }

    public JingWangDrillMysqlDataReport(IOptions<JingWangExternalMysqlOptions> options,
        IHttpRequestInvoker httpRequestInvoker,
        ILoggerFactory loggerFactory,
        Device device)
    {
        _chongdaOptions = options.Value;
        _logger = loggerFactory.CreateLogger<JingWangDrillMysqlDataReport>();
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

    public Task OnDevicePanelChanged(DevicePanelChangedRequest request) => throw new NotImplementedException();

    public Task OnDeviceCutterTrayChanged(DeviceCutterTrayChangedRequest request) => throw new NotImplementedException();
}
