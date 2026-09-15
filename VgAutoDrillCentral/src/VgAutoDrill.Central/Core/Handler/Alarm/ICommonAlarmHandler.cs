using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Handler.Alarm;

internal interface ICommonAlarmHandler
{
    /// <summary>
    /// 处理alarmRequest
    /// </summary>
    /// <param name="alarmRequest"></param>
    /// <returns></returns>
    Task HandleDeviceAlarmReportRequest(DeviceAlarmReportRequest alarmRequest);
    /// <summary>
    /// 保存处理之后的告警请求handledRequest
    /// </summary>
    /// <param name="handledRequest"></param>
    /// <returns></returns>
    Task SaveHandledDeviceAlarmReportRequest(DeviceAlarmReportRequest handledRequest);
}
