using SqlSugar;
using VgAutoDrill.Admin.Model.CentralModels;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 钻机稼动率因素表
    ///</summary>
    [SugarTable("t_drill_rate_factor")]
    public class DrillRateFactor : BaseEntity
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [SugarColumn(ColumnName = "device_id")]
        public virtual string? DeviceId { get; set; }
        /// <summary> 
        /// 因素
        /// </summary>
        [SugarColumn(ColumnName = "reason")]
        public virtual DrillRateFactorReason? Reason { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnName = "start_time")]
        public virtual DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnName = "end_time")]
        public virtual DateTime? EndTime { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        [SugarColumn(ColumnName = "location_code")]
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "memo")]
        public virtual string? Memo { get; set; }

    }
}
