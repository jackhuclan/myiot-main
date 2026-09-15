using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VegaIot.External.Hik;

public class TransferJob
{
    public long Id { get; set; }
    public string? Code { get; set; }
    public int? IsUrgent { get; set; }
    public InteractionSequence? InteractionSequence { get; set; }
    public ScheduledTaskStatus? ScheduledTaskStatus { get; set; }

    /// <summary>
    /// 类型：空仓、生料、熟料、首件
    /// </summary>
    public TransportationKind? TransportationKind { get; set; }

    public string? InternalLotNo { get; set; }
    public string? ExternalLotNo { get; set; }

    /// <summary>
    /// 库房，库位分区编号
    /// </summary>
    public string? PartitionCode { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? ForkCode { get; set; }

    public string? RelatedDrillTrace { get; set; }

    /// <summary>
    /// 转运任务描述
    /// </summary>
    public string? TransferDesc { get; set; }

    /// <summary>
    /// 熟料数量
    /// </summary>
    public virtual int? ClinkerCount { get; set; }

    /// <summary>
    /// 生料数量
    /// </summary>
    /// <returns></returns>
    public virtual int? RawCount { get; set; }

    /// <summary>
    /// 料仓号
    /// </summary>
    public string? SiloCode { get; set; }

    public string? MasterRouteCode { get; set; }
    public long? MasterScheduleId { get; set; }
    public string? RelatedDrillScheduleIds { get; set; }
    public string? MasterLocationCode { get; set; }
    public string? OtherForkCode { get; set; }
    public bool? IsCancel { get; set; }
    public virtual long? JobId { get; set; }
    public virtual long? PlanId { get; set; }
    public virtual string? HkResponse { get; set; }
    public virtual string? HkResponseKey { get; set; }
    public DeviceKind MasterDeviceKind { get; set; }
    public DeviceKind AgvKind { get; set; }
    public string AllocatedAgv { get; set; } = string.Empty;
    public string? StartLocationCode { get; set; }

    public string? StartDeviceId { get; set; }

    public virtual long? StartScheduleId { get; set; }

    public string? EndLocationCode { get; set; }

    public string? EndDeviceId { get; set; }

    public virtual long? EndScheduleId { get; set; }
    public int TransferBehavior { get; set; }
    public string? RelateDeviceCode { get; set; }
    public virtual DateTime? AllocateTime { get; set; }
    public virtual DateTime? RunningTime { get; set; }
    public virtual DateTime? CompletedTime { get; set; }
    public virtual DateTime? FailedTime { get; set; }
    public virtual DateTime? CanceledTime { get; set; }
    public string? HikResponseKey { get; set; }

    public string? ForkLocationScheduleId { get; set; }

    public string? DeviceLocationScheduleId { get; set; }

    public virtual DateTime? CreateTime { get; set; }
}
