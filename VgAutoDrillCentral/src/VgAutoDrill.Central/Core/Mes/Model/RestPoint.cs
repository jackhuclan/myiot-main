namespace VgAutoDrill.Central.Core.Mes.Model;

public class RestPoint
{
    /// <summary>
    /// 休息点Code
    /// </summary>
    public string RestCode { get; set; }

    /// <summary>
    /// 休息点名称
    /// </summary>
    public string? RestName { get; set; }
    /// <summary>
    /// 物理点位
    /// </summary>
    public string Point { get; set; }

    /// <summary>
    /// 工艺路线
    /// </summary>
    public string? RouteCode { get; set; }

    /// <summary>
    /// 该点预定分配的AGV
    /// </summary>
    public string PreBookAGV { get; set; }

    /// <summary>
    /// 当前点正占用的AGV
    /// </summary>
    public string CurrentAgv { get; set; }

    public int? Priority { get; set; }
    /// <summary>
    /// 当前关联关系是否可用
    /// </summary>
    public int Status { get; set; }
}
