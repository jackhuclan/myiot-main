using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart
{
    public class AddOrUpdateRestAndPartReq : BaseAddOrUpdateDto
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
        /// 工艺路线
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// AGV 类型  6-提升AGV  10--顶升AGV
        /// </summary>
        public virtual DeviceKind? AgvDeviceKind { get; set; }

        /// <summary>
        /// 该分区可分派的休息点的优先级  1>>2>>3
        /// </summary>
        public virtual int? Priority { get; set; }
    }
}
