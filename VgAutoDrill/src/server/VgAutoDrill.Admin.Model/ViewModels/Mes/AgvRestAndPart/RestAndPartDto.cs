using VgAutoDrill.Fundation.Iot.Models;
namespace VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart
{
    public class RestAndPartDto : BaseDto
    {
        /// <summary>
        /// 分区编码
        /// </summary>
        public virtual string? PartCode { get; set; }

        /// <summary>
        /// 分区名称
        /// </summary>
        public virtual string? PartName { get; set; }

        /// <summary>
        /// 休息点编码
        /// </summary>
        public virtual string? RestCode { get; set; }

        /// <summary>
        /// 休息点名称
        /// </summary>
        public virtual string? RestName { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// AGV 类型  6-提升AGV  10--顶升AGV
        /// </summary>
        public virtual DeviceKind? AgvDeviceKind { get; set; }

        /// <summary>
        /// 物理点位
        /// </summary>
        public virtual string? Point { get; set; }

        /// <summary>
        /// 该分区可分派的休息点的优先级  1>>2>>3
        /// </summary>
        public virtual int? Priority { get; set; }

        /// <summary>
        /// 当前点正占用的AGV
        /// </summary>
        public virtual string? CurrentAgv { get; set; }

        /// <summary>
        /// 该分区该点预定分配的AGV
        /// </summary>
        public virtual string? PreBookAgv { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public virtual DateTime? PreBookTime { get; set; }

        /// <summary>
        /// 休息点状态
        /// </summary>
        public virtual int? RestStatus { get; set; }
    }
}
