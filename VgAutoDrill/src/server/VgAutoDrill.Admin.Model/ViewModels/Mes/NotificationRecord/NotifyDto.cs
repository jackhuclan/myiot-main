namespace VgAutoDrill.Admin.Model.ViewModels.Mes.NotificationRecord
{
    public class NotifyDto : BaseDto
    {
        /// <summary>
        /// 通知时间
        /// </summary>
        public virtual DateTime? NotifyTime { get; set; }

        /// <summary>
        /// 通知类型编号
        /// </summary>
        public virtual string? NotifyCode { get; set; }

        /// <summary>
        /// 通知类型名称
        /// </summary>
        public virtual string? NotifyName { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public virtual string? NotifyMsg { get; set; }

        /// <summary>
        /// 通知方式
        /// </summary>
        public int? NotifyWays { get; set; }

        /// <summary>
        /// 通知方式名称
        /// </summary>
        public virtual string? NotifyWaysName { get; set; }

        /// <summary>
        /// 告警记录
        /// </summary>
        public int? AlarmRecordId { get; set; }
    }
}
