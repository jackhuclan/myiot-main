using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Handler.Alarm;

internal interface IDeviceAlarmDispatcher
{
    void AddPolicy(DeviceAlarmDispatchPolicy policy);
    List<DeviceAlarmDispatchPolicy> SelectMatchedPolicies(DeviceAlarmReportRequest deviceAlarmReportRequest);
}
