using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// AGV休息点和分区关联关系
    ///</summary>
    [SugarTable("t_agv_rest_and_part")]
    public class AgvRestAndPart : BaseEntity
    {
        /// <summary>
        /// 分区编码
        /// </summary>
        [SugarColumn(ColumnName = "part_code")]
        public virtual string? PartCode { get; set; }

        /// <summary>
        /// 分区名称
        /// </summary>
        [SugarColumn(ColumnName = "part_name")]
        public virtual string? PartName { get; set; }

        /// <summary>
        /// 休息点id
        /// </summary>
        [SugarColumn(ColumnName = "rest_id")]
        public virtual long? RestId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        [SugarColumn(ColumnName = "route_code")]
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// AGV 类型  6-提升AGV  10--顶升AGV
        /// </summary>
        [SugarColumn(ColumnName = "agv_device_kind")]
        public virtual DeviceKind? AgvDeviceKind { get; set; }

        /// <summary>
        /// 该分区可分派的休息点的优先级  1>>2>>3
        /// </summary>
        [SugarColumn(ColumnName = "priority")]
        public virtual int? Priority { get; set; }

    }
}
