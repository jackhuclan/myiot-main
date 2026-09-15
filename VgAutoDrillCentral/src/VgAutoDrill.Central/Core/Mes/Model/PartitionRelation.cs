using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Mes.Model;

/// <summary>
/// 分区关系表
/// </summary>
public class PartitionRelation
{
    public Partition Partition { get; set; }
    public RestPoint RestPoint { get; set; }

    /// <summary>
    /// AGV 类型  6-提升AGV  10--顶升AGV
    /// </summary>
    public DeviceKind? AgvDeviceKind { get; set; }

    /// <summary>
    /// 工艺路线
    /// </summary>
    public string? RouteCode { get; set; }

    /// <summary>
    /// 该分区可分派的休息点的优先级  1>>2>>3>>...
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// 当前关联关系是否可用
    /// </summary>
    public int Status { get; set; }

}
