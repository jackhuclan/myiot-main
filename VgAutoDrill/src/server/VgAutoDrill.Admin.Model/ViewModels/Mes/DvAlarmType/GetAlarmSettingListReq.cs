namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmType
{
    public class GetAlarmSettingListReq : Page
    {
        public string Name { get; set; }
        public virtual string? Code { get; set; }

        public int? AlarmLevel { get; set; }

        public int EventId { get; set; }

        public virtual string? EventRules { get; set; }

        /// <summary>
        /// 通知方式
        /// ,分割
        /// </summary>
        public virtual string? NotifyWayIds { get; set; }

        /// <summary>
        /// 通知方式名称
        /// ,分割
        /// </summary>
        public virtual string? NotifyWayNames { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public virtual string? EventName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { set; get; } = -1;
    }
}
