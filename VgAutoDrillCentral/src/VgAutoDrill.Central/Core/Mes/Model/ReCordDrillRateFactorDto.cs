namespace VgAutoDrill.Central.Core.Mes.Model;

public class ReCordDrillRateFactorDto
{
    /// <summary>
    /// 设备编号
    /// </summary>
    public virtual string? DeviceId { get; set; }
    /// <summary> 
    /// 因素
    /// </summary>
    public virtual Admin.Model.CentralModels.DrillRateFactorReason? Reason { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public virtual DateTime? StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public virtual DateTime? EndTime { get; set; }

    public virtual string? LocationCode { get; set; }

    public virtual string? Memo { get; set; }

    /// <summary>
    /// 班次开始时间
    /// </summary>
    public virtual DateTime? DrillShiftsStartTime { get; set; }
}
