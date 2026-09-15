using System.Text.Json.Serialization;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Schedule;

public class ScheduleRequirement : IComparable<ScheduleRequirement>
{
    public ScheduleRequirement() { }
    public static ScheduleRequirement Empty = new();
    /// <summary>
    /// 呼叫的钻机deviceid
    /// </summary>
    public string CallerDeviceId { get; set; }
    /// <summary>
    /// 呼叫物料类型
    /// </summary>
    public MaterialKind MaterialKind { get; set; }
    /// <summary>
    /// 呼叫生料code
    /// </summary>
    public string RequireUndrilledItemCode { get; set; } = string.Empty;
    /// <summary>
    /// 呼叫熟料code
    /// </summary>
    public string RequireDrilledItemCode { get; set; } = string.Empty;
    /// <summary>
    /// 需要的熟料数量
    /// </summary>
    public int RequireDrilledItemQty { get; set; }
    /// <summary>
    /// 最小需要的熟料数量
    /// </summary>
    public int MinRequiredDrilledQty { get; set; }

    /// <summary>
    /// 需要的生料数量
    /// </summary>
    public int RequireUndrilledItemQty { get; set; }
    /// <summary>
    /// 最小需要的生料数量
    /// </summary>
    public int MinRequiredUndrilledQty { get; set; }
    /// <summary>
    /// 原始调度任务
    /// </summary>
    [JsonIgnore]
    public DrillScheduleTask OriginalSchedule { get; set; }
    public string RouteCode { get; set; }
    /// <summary>
    /// 是否是首件
    /// </summary>
    public bool IsFirst { get; set; }
    /// <summary>
    /// 有生料
    /// </summary>
    public bool HasUndrilled { get; set; }

    /// <summary>
    /// 钻机上现有生料料号
    /// </summary>
    public string? PayloadUndrilledItemCode { get; set; }

    /// <summary>
    /// 钻机上现有熟料料号
    /// </summary>
    public string? PayloadDrilledItemCode { get; set; }

    /// <summary>
    /// 钻机上现有生料数量
    /// </summary>
    public int PayloadUndrilledPanelsCount { get; set; }
    /// <summary>
    /// 有熟料
    /// </summary>
    public bool HasDrilled { get; set; }
    /// <summary>
    /// 钻机上现有熟料数量
    /// </summary>
    public int PayloadDrilledPanelsCount { get; set; }
    /// <summary>
    /// 有首件
    /// </summary>
    public bool HasFirst { get; set; }
    /// <summary>
    /// 首件数量
    /// </summary>
    public int PayloadFirstPanelsCount { get; set; }

    public InteractionSequence InteractionSequence { get; set; }
    public string? RequestInteractionBehaviorName { get; set; }
    public string? InteractionSequenceName { get; set; }
    public string? ScheduledTaskStatusName { get; set; }
    public InteractionBehavior Behavior { get; set; }
    public DeviceKind RequiredDeviceKind { get; set; }
    public int Score { get; set; } = 0;

    public int IsPartCompletedValue => this.OriginalSchedule != null && this.OriginalSchedule.ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.PartCompleted ? 1 : 0;
    public int IsUrgentValue => this.OriginalSchedule != null && this.OriginalSchedule.IsUrgent.HasValue ? this.OriginalSchedule.IsUrgent.Value : 0;

    public int CompareTo(ScheduleRequirement? other)
    {
        if (other == null)
            return -1;

        if (this.IsPartCompletedValue != other.IsPartCompletedValue)
        {
            return other.IsPartCompletedValue.CompareTo(this.IsPartCompletedValue);
        }
        else if (this.IsUrgentValue != other.IsUrgentValue)
        {
            return other.IsUrgentValue.CompareTo(this.IsUrgentValue);
        }
        else if (this.Score != other.Score)
        {
            return other.Score.CompareTo(this.Score);
        }
        else if (this.OriginalSchedule.Id != other.OriginalSchedule.Id)
        {
            return this.OriginalSchedule.Id.CompareTo(other.OriginalSchedule.Id);
        }

        return 0;
    }
}
