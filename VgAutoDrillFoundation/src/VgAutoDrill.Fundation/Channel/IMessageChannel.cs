using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Channel;

/// <summary>
/// define exported data
/// </summary>
public interface IMessageChannel
{
    event Func<DeviceEventReportRequest, Task>? OnDeviceEventReport;
    event Func<DevicePropertiesReportRequest, Task>? OnDevicePropertiesReport;
    event Func<DeviceServiceInvokeRequest, Task>? OnDeviceServiceReport;
    event Func<DeviceStatusReportRequest, Task>? OnDeviceStatusReport;
    event Func<DeviceAlarmReportRequest, Task>? OnDeviceAlarmReport;
    event Func<DevicePanelChangedRequest, Task>? OnDevicePanelChanged;
    event Func<DeviceCutterTrayChangedRequest, Task>? OnDeviceCutterTrayChanged;
    event Func<DeviceBrokenToolFaultRequest, Task>? OnDeviceBrokenToolFault;

    /// <summary>
    /// export event data
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DeviceEventReportResponse> DeviceEventReport(DeviceEventReportRequest request);
    /// <summary>
    /// export property data
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DevicePropertiesReportResponse> DevicePropertiesReport(DevicePropertiesReportRequest request);
    /// <summary>
    /// export service data
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> DeviceServiceReport(DeviceServiceInvokeRequest request);
    /// <summary>
    /// export status data
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DeviceStatusReportResponse> DeviceStatusReport(DeviceStatusReportRequest request);
    /// <summary>
    /// export alarm data
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DeviceAlarmReportResponse> DeviceAlarmReport(DeviceAlarmReportRequest request);

    /// <summary>
    /// occured when device's panel list changed
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DevicePanelChangedResponse> DevicePanelChangedReport(DevicePanelChangedRequest request);

    /// <summary>
    /// occured when device's cutter tray list changed
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DeviceCutterTrayChangedResponse> DeviceCutterTrayChangedReport(DeviceCutterTrayChangedRequest request);

    /// <summary>
    /// occured when device's broken tool fault
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DeviceBrokenToolFaultResponse> DeviceBrokenToolFaultReport(DeviceBrokenToolFaultRequest request);
}
