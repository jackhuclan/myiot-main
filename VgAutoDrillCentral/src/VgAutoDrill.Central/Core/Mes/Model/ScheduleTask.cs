using VgAutoDrill.Central.Core.AutoMapper;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Mes.Model;

public class ScheduleTask
{
    private ScheduledTaskStatus? _scheduledTaskStatus;

    public event Func<ScheduleTask, Task> OnStatusChanged;

    public event Func<ScheduleTask, Task> OnTimeout;

    private volatile bool _appointed = false;
    private volatile string _appointedMessage = string.Empty;
    private volatile bool _trackInAppointed = false;
    private volatile bool _trackOutAppointed = false;

    [IgnoreMap]
    public bool Appointed
    {
        get
        {
            return _appointed;
        }
        set
        {
            _appointed = value;
            if (!value)
            {
                _appointedMessage = string.Empty;
            }
        }
    }
    [IgnoreMap]
    public string AppointedMessage { get => _appointedMessage; set => _appointedMessage = value; }

    [IgnoreMap]
    public bool TrackInAppointed { get => _trackInAppointed; set => _trackInAppointed = value; }

    [IgnoreMap]
    public bool TrackOutAppointed { get => _trackOutAppointed; set => _trackOutAppointed = value; }

    /// <summary>
    /// 能够调度agv的任务
    /// </summary>
    [IgnoreMap]
    public bool IsNotStarted => ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.Created
        || ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.PartCompleted;

    /// <summary>
    /// 能够调度agv的任务
    /// </summary>
    [IgnoreMap]
    public bool IsCompleted => ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.Completed
        || ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.Canceled
        || ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.Failed;

    public virtual long Id { get; set; }
    public virtual string? Code { get; set; }
    public virtual string? TaskId { get; set; }
    public virtual string? ItemCode { get; set; }
    public virtual string? CallerDeviceId { get; set; }
    public virtual string? AllocatedAgv { get; set; }

    public virtual ScheduledTaskStatus? ScheduledTaskStatus
    {
        get => _scheduledTaskStatus;
        set
        {
            bool statusChanged = false;
            if (_scheduledTaskStatus != value)
                statusChanged = true;

            _scheduledTaskStatus = value;

            if (statusChanged)
                OnStatusChanged?.Invoke(this);
        }
    }

    public virtual int? TotalRawCount { get; set; }
    public virtual int? IsUrgent { get; set; }
    public virtual string? RouteCode { get; set; }

    /// <summary>
    /// 是否是辅助设备请求
    /// </summary>
    public virtual bool? IsAuxiliary { get; set; }

    /// <summary>
    /// 是否为先执行
    /// </summary>
    public virtual bool? IsMaster { get; set; }

    public virtual long? MasterScheduleId { get; set; }
    public virtual InteractionSequence? InteractionSequence { get; set; }
    public virtual string? RequestInteractionBehaviorName { get; set; }
    public virtual string? Remark { get; set; }
    public virtual DeviceKind? RequestDeviceKind { get; set; }

    /// <summary>
    /// 库位编号
    /// </summary>
    public virtual string? LocationCode { get; set; }

    /// <summary>
    /// 创建时间
    ///</summary>
    public virtual DateTime CreateTime { get; set; }

    public virtual DateTime? AllocateTime { get; set; }

    public virtual DateTime? RunningTime { get; set; }

    public virtual DateTime? CompletedTime { get; set; }

    public virtual DateTime? FailedTime { get; set; }

    public virtual DateTime? CanceledTime { get; set; }
    public virtual string? RequestJson { get; set; }
    public virtual string? RoutingKey { get; set; }

    /// <summary>
    /// 是否已全部下发
    /// </summary>
    public virtual bool IsAllPanelSent { get; set; }

    [IgnoreMap]
    public ScheduledTaskStatusUpdateSource UpdateSource { get; set; } = ScheduledTaskStatusUpdateSource.FromDB;

    [IgnoreMap]
    public Location? TrackLocation { get; set; }

    /// <summary>
    /// 调度实时日志
    /// </summary>
    [IgnoreMap]
    public string RealtimeScheduleLog { get; set; }
}
