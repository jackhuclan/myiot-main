using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Central.Core.Mes;

public class WorkOrderTask
{
    public string? Code { get; set; }

    public virtual string? ItemCode { get; set; }

    public virtual string? BatchCode { get; set; }

    public DateTime? StartTime { get; set; }

    public virtual int? Duration { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual TaskStatusEnum? TaskStatus { get; set; }

    public virtual decimal? NowWadCount { get; set; }
    public virtual string? RouteCode { get; set; }

    public virtual string? WorkStationCode { get; set; }
}
