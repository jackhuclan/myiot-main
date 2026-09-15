using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Fundation.Iot.Transportation;

public class QueryTransportationRequest
{
    public string? WarehouseCode { get; set; }
    /// <summary>
    /// 调度任务状态
    /// </summary>
    public virtual List<ScheduledTaskStatus>? ScheduledTaskStatusList { get; set; }
    /// <summary>
    /// 类型：空仓、生料、熟料、首件
    /// </summary>
    public virtual TransportationKind? TransportationKind { get; set; }
    /// <summary>
    /// 是否紧急
    /// </summary>
    public virtual int? IsUrgent { get; set; }
    /// <summary>
    /// 交互序列
    /// </summary>
    public virtual InteractionSequence? InteractionSequence { get; set; }
    /// <summary>
    /// 内部编号
    /// </summary>
    public virtual string? InternalLotNo { get; set; }

    /// <summary>
    /// 外部编号
    /// </summary>
    public virtual string? ExternalLotNo { get; set; }
    /// <summary>
    /// 创建时间-开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }
}
