using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 通知记录
    ///</summary>
    [SugarTable("t_notify")]
    public class Notify : BaseEntity
    {
        /// <summary>
        /// 通知时间
        /// </summary>
        [SugarColumn(ColumnName = "notify_time")]

        public virtual DateTime? NotifyTime { get; set; }

        /// <summary>
        /// 通知类型编号
        /// </summary>
        [SugarColumn(ColumnName = "notify_code")]

        public virtual string? NotifyCode { get; set; }

        /// <summary>
        /// 通知类型名称
        /// </summary>
        [SugarColumn(ColumnName = "notify_name")]

        public virtual string? NotifyName { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        [SugarColumn(ColumnName = "notify_msg")]

        public virtual string? NotifyMsg { get; set; }

        /// <summary>
        /// 通知方式
        /// </summary>
        [SugarColumn(ColumnName = "notify_ways")]

        public int? NotifyWays { get; set; }

        /// <summary>
        /// 通知方式名称
        /// </summary>
        [SugarColumn(ColumnName = "notify_ways_name")]
        public virtual string? NotifyWaysName { get; set; }

        /// <summary>
        /// 告警记录
        /// </summary>
        [SugarColumn(ColumnName = "alarm_record_id")]

        public int? AlarmRecordId { get; set; }
    }
}
