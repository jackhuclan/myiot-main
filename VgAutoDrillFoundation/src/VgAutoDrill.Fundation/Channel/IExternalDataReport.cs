using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Channel;

public interface IExternalDataReport
{
    /// <summary>
    /// 上报设备
    /// </summary>
    Device ReportingDevice { get; }
    /// <summary>
    /// 设备告警上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task OnDeviceAlarmReport(DeviceAlarmReportRequest request);
    /// <summary>
    /// 设备事件上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task OnDeviceEventReport(DeviceEventReportRequest request);
    /// <summary>
    /// 设备属性/采集参数上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task OnDevicePropertiesReport(DevicePropertiesReportRequest request);
    /// <summary>
    /// 设备服务/设备直接发生交互、服务调用上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task OnDeviceServiceReport(DeviceServiceInvokeRequest request);
    /// <summary>
    /// 设备状态上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task OnDeviceStatusReport(DeviceStatusReportRequest request);
    /// <summary>
    /// 设备板料上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task OnDevicePanelChanged(DevicePanelChangedRequest request);
    /// <summary>
    /// 设备刀具上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task OnDeviceCutterTrayChanged(DeviceCutterTrayChangedRequest request);
    /// <summary>
    /// 设备刀具故障上报
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task OnDeviceBrokenToolFault(DeviceBrokenToolFaultRequest request);
}
