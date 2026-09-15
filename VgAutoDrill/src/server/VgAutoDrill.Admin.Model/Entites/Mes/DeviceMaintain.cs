using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备维护记录
    ///</summary>
    [SugarTable("t_device_maintain")]
    public class DeviceMaintain : BaseEntity
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        [SugarColumn(ColumnName = "device_id")]
        public int? DeviceId { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [SugarColumn(ColumnName = "device_name")]
        public virtual string? DeviceName { get; set; }

        /// <summary>
        /// 维护时间
        /// </summary>
        [SugarColumn(ColumnName = "maintain_time")]
        public virtual DateTime? MaintainTime { get; set; }

        /// <summary>
        /// 维护人ID
        /// </summary>
        [SugarColumn(ColumnName = "maintain_id")]
        public int? MaintainId { get; set; }

        /// <summary>
        /// 维护人
        /// </summary>
        [SugarColumn(ColumnName = "maintain_person")]
        public virtual string? MaintainPerson { get; set; }

        /// <summary>
        /// 维护状态(-1/0/1)
        /// </summary>
        [SugarColumn(ColumnName = "maintain_status")]
        public virtual string? MaintainStatus { get; set; } = "-1";

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }
    }
}
