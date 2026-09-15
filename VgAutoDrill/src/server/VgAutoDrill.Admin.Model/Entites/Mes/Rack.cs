using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 料架
    /// </summary>
    [SugarTable("t_rack")]
    public class Rack : BaseEntity
    {
        /// <summary>
        /// 所属仓库编号
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_code")]
        public virtual string? WareHouseCode { get; set; }

        /// <summary>
        /// 所属仓库id
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_id")]
        public virtual int? WareHouseId { get; set; }

        /// <summary>
        /// 料架编号
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        /// <summary>
        /// 是否关联料仓
        /// </summary>
        [SugarColumn(ColumnName = "is_have_silo")]
        public virtual bool? IsHaveSilo { get; set; } = false;

        /// <summary>
        /// 料仓编号
        /// </summary>
        [SugarColumn(ColumnName = "silo_code")]
        public virtual string? SiloCode { get; set; }

        ///// <summary>
        ///// 内点
        ///// </summary>
        //[SugarColumn(ColumnName = "inner_point")]
        //public virtual string? InnerPoint { get; set; }

        ///// <summary>
        ///// 外点
        ///// </summary>
        //[SugarColumn(ColumnName = "out_point")]
        //public virtual string? OutPoint { get; set; }
        ///// <summary>
        ///// 小车内点
        ///// </summary>
        //[SugarColumn(ColumnName = "trans_inner_point")]
        //public virtual string? TransInnerPoint { get; set; }

        ///// <summary>
        ///// 小车外点
        ///// </summary>
        //[SugarColumn(ColumnName = "trans_out_point")]
        //public virtual string? TransOutPoint { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        [SugarColumn(ColumnName = "device_kind")]
        public virtual DeviceKind? DeviceKind { get; set; }
        /// <summary>
        /// 关联设备code
        /// </summary>
        [SugarColumn(ColumnName = "relate_device_code")]
        public virtual string? RelateDeviceCode { get; set; }
        /// <summary>
        /// 位置码
        /// </summary>
        [SugarColumn(ColumnName = "position_code")]
        public virtual string? PositionCode { get; set; }


        /// <summary>
        /// 大车内点
        /// </summary>
        [SugarColumn(ColumnName = "feed_agv_inner_point")]
        public virtual string? FeedAGVInnerPoint { get; set; }

        /// <summary>
        /// 大车外点
        /// </summary>
        [SugarColumn(ColumnName = "feed_agv_out_point")]
        public virtual string? FeedAGVOutputPoint { get; set; }

        /// <summary>
        /// 大车休息点
        /// </summary>
        [SugarColumn(ColumnName = "feed_agv_rest_point")]
        public virtual string? FeedAGVRestPoint { get; set; }

        /// <summary>
        /// 小车内点
        /// </summary>
        [SugarColumn(ColumnName = "trans_agv_inner_point")]
        public virtual string? TransAGVInnerPoint { get; set; }

        /// <summary>
        /// 小车外点
        /// </summary>
        [SugarColumn(ColumnName = "trans_agv_out_point")]
        public virtual string? TransAGVOutputPoint { get; set; }

        /// <summary>
        /// 小车休息点
        /// </summary>
        [SugarColumn(ColumnName = "trans_agv_rest_point")]
        public virtual string? TransAGVRestPoint { get; set; }

    }
}
