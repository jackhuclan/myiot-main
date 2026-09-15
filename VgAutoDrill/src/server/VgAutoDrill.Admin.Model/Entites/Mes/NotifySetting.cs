using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 通知设置
    ///</summary>
    [SugarTable("t_notify_setting")]
    public class NotifySetting : BaseEntityWithTree
    {
        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(ColumnName = "notify_desc")]
        public virtual string? NotifyDesc { get; set; }

        /// <summary>
        /// 通知方式
        /// </summary>
        [SugarColumn(ColumnName = "notify_ways")]
        public int? NotifyWays { get; set; }

        /// <summary>
        /// 通知参数
        /// </summary>
        [SugarColumn(ColumnName = "notify_params")]
        public virtual string? NotifyParams { get; set; }

        /// <summary>
        /// 是否重复发送 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_repeat_send")]
        public virtual byte IsRepeatSend { get; set; }

        /// <summary>
        /// 发送频率
        /// </summary>
        [SugarColumn(ColumnName = "send_frequency")]
        public virtual int? SendFrequency { get; set; }
    }
}
