using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Mes;

public class QueryScheduleRequest
{
    public DateTime? EndTime { get; set; }
    public virtual bool? HasAgvSetted { get; set; }
    public virtual bool? HasRawItemSetted { get; set; }
    public virtual InteractionSequence? InteractionSequence { get; set; }
    public virtual bool? IsAllPanelSent { get; set; }
    /// <summary>
    /// 是否是辅助设备请求
    /// </summary>
    public virtual bool? IsAuxiliary { get; set; }

    /// <summary>
    /// 是否为先执行
    /// </summary>
    public virtual bool? IsMaster { get; set; }

    public virtual int? IsUrgent { get; set; }
    public virtual string? ItemCode { get; set; }
    public virtual string? ItemName { get; set; }
    public virtual bool OrderByIDDesc { get; set; }
    public virtual List<DeviceKind>? RequestDeviceKindList { get; set; }
    public string? RequireDeviceId { get; set; }
    public virtual string? RouteCode { get; set; }
    public virtual string? RoutingKey { get; set; }
    public virtual List<ScheduledTaskStatus>? ScheduledTaskStatusList { get; set; }
    public string? SourceDeviceId { get; set; }
    public DateTime? StartTime { get; set; }
    public virtual string? TaskId { get; set; }
    public string? TraceId { get; set; }
}
