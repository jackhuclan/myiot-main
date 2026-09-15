namespace VgAutoDrill.Central.Core.Mes.Model;

public class AllotsPanelDataRequest
{
    /// <summary>
    /// 设备编号
    /// </summary>
    public virtual string? DeviceCode { get; set; }

    /// <summary>
    /// 0钻机, 1生料仓,2熟料仓
    /// </summary>
    public virtual List<int>? Layers { get; set; }

    /// <summary>
    /// 料架编号
    /// </summary>
    public virtual string? RackCode { get; set; }
}
