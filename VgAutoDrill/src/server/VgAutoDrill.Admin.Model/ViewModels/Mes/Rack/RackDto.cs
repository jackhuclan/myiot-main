using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class RackDto : BaseDto
    {
        /// <summary>
        /// 所属仓库编号
        /// </summary>
        public virtual string? WareHouseCode { get; set; }

        /// <summary>
        /// 所属仓库id
        /// </summary>
        public virtual int? WareHouseId { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 是否关联料仓
        /// </summary>
        public virtual bool? IsHaveSilo { get; set; } = false;

        /// <summary>
        /// 料仓编号
        /// </summary>
        public virtual string? SiloCode { get; set; }
        ///// <summary>
        ///// 内点
        ///// </summary>
        //public virtual string? InnerPoint { get; set; }
        ///// <summary>
        ///// 外点
        ///// </summary>
        //public virtual string? OutPoint { get; set; }
        ///// <summary>
        ///// 小车内点
        ///// </summary>
        //public virtual string? TransInnerPoint { get; set; }
        ///// <summary>
        ///// 小车外点
        ///// </summary>
        //public virtual string? TransOutPoint { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual DeviceKind? DeviceKind { get; set; }
        /// <summary>
        /// 关联设备code
        /// </summary>
        public virtual string? RelateDeviceCode { get; set; }
        /// <summary>
        /// 位置码
        /// </summary>
        public virtual string? PositionCode { get; set; }


        /// <summary>
        /// 大车内点
        /// </summary>
        public virtual string? FeedAGVInnerPoint { get; set; }
        /// <summary>
        /// 大车外点
        /// </summary>
        public virtual string? FeedAGVOutputPoint { get; set; }

        /// <summary>
        /// 大车休息点
        /// </summary>
        public virtual string? FeedAGVRestPoint { get; set; }

        /// <summary>
        /// 小车内点
        /// </summary>
        public virtual string? TransAGVInnerPoint { get; set; }

        /// <summary>
        /// 小车外点
        /// </summary>
        public virtual string? TransAGVOutputPoint { get; set; }

        /// <summary>
        /// 小车休息点
        /// </summary>
        public virtual string? TransAGVRestPoint { get; set; }
    }
}
