using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 设备记录表
    /// </summary>
    [SugarTable("t_device_records")]
    public class DeviceRecords : BaseEntity
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 工作时间
        /// </summary>
        [SugarColumn(ColumnName = "work_time")]
        public virtual string? WorkTime { get; set; }

        /// <summary>
        /// 等待时间
        /// </summary>
        [SugarColumn(ColumnName = "wait_time")]
        public virtual string? WaitTime { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        [SugarColumn(ColumnName = "error_time")]
        public virtual string? ErrorTime { get; set; }

        /// <summary>
        /// 开机时间
        /// </summary>
        [SugarColumn(ColumnName = "open_time")]
        public virtual string? OpenTime { get; set; }

        /// <summary>
        /// 稼动率
        /// </summary>
        [SugarColumn(ColumnName = "duty")]
        public virtual string? Duty { get; set; }

        /// <summary>
        /// 结束到开始总时间
        /// </summary>
        [SugarColumn(ColumnName = "end_to_start_time")]
        public virtual string? EndToStartTime { get; set; }

        /// <summary>
        /// 清洗夹头总时间
        /// </summary>
        [SugarColumn(ColumnName = "collect_clear_time")]
        public virtual string? CollectClearTime { get; set; }

        /// <summary>
        /// 数据日期
        /// </summary>
        [SugarColumn(ColumnName = "date_string")]
        public virtual string? DateString { get; set; }
    }
}
