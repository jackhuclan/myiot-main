using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Handler.Alarm;

internal class DeviceAlarmDispatchPolicy
{
    /// <summary>
    /// 分发策略的名称
    /// </summary>
    public string PolicyName { get; set; } = string.Empty;
    public Predicate<DeviceAlarmReportRequest> DispatchPredicate { get; set; } = (obj) => true;
    public ICommonAlarmHandler? ExcuteHandler { get; set; }
}
