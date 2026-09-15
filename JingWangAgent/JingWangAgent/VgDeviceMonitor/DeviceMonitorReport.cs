using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceMonitor;

internal class DeviceMonitorReport : IExternalDataReport
{
    private readonly ILogger<DeviceMonitorReport> _logger;

    public Device ReportingDevice { get; }

    public DeviceMonitorReport(ILoggerFactory loggerFactory,
        Device device)
    {
        _logger = loggerFactory.CreateLogger<DeviceMonitorReport>();
        ReportingDevice = device;
    }

    public Task OnDeviceAlarmReport(DeviceAlarmReportRequest request)
    {
        _logger.LogInformation(request.ToString());
        return Task.CompletedTask;
    }

    public Task OnDeviceBrokenToolFault(DeviceBrokenToolFaultRequest request)
    {
        _logger.LogInformation(request.ToString());
        return Task.CompletedTask;
    }

    public Task OnDeviceCutterTrayChanged(DeviceCutterTrayChangedRequest request)
    {
        _logger.LogInformation(request.ToString());
        return Task.CompletedTask;
    }

    public Task OnDeviceEventReport(DeviceEventReportRequest request)
    {
        _logger.LogInformation(request.ToString());
        return Task.CompletedTask;
    }

    public Task OnDevicePanelChanged(DevicePanelChangedRequest request)
    {
        _logger.LogInformation(request.ToString());
        return Task.CompletedTask;
    }

    public Task OnDevicePropertiesReport(DevicePropertiesReportRequest request)
    {
        _logger.LogInformation(request.ToString());
        return Task.CompletedTask;
    }

    public Task OnDeviceServiceReport(DeviceServiceInvokeRequest request)
    {
        _logger.LogInformation(request.ToString());
        return Task.CompletedTask;
    }

    public Task OnDeviceStatusReport(DeviceStatusReportRequest request)
    {
        _logger.LogInformation(request.ToString());
        return Task.CompletedTask;
    }
}
