using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 告警设置
    ///</summary>
    [SugarTable("t_alarm_setting")]
    public class AlarmSetting : BaseEntityWithTree
    {
        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(ColumnName = "alarm_desc")]
        public virtual string? AlarmDesc { get; set; }

        /// <summary>
        /// 告警级别（1普通、2严重、3紧急）
        /// </summary>
        [SugarColumn(ColumnName = "alarm_level")]
        public int? AlarmLevel { get; set; }

        [SugarColumn(ColumnName = "event_id")]
        public int EventId { get; set; }

        [SugarColumn(ColumnName = "event_rules")]
        public virtual string? EventRules { get; set; }

        /// <summary>
        /// 通知方式
        /// ,分割
        /// </summary>
        [SugarColumn(ColumnName = "notify_way_ids")]
        public virtual string? NotifyWayIds { get; set; }

        /// <summary>
        /// 通知方式名称
        /// ,分割
        /// </summary>
        [SugarColumn(ColumnName = "notify_way_names")]
        public virtual string? NotifyWayNames { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        [SugarColumn(ColumnName = "event_name")]
        public virtual string? EventName { get; set; }

    }
}
