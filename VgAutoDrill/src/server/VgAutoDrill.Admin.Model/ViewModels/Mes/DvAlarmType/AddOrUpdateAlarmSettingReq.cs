namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmType
{
    public class AddOrUpdateAlarmSettingReq : BaseAddOrUpdateWithTreeDto, IDtoWithTree
    {
        /// <summary>
        /// 描述
        /// </summary>

        public virtual string? AlarmDesc { get; set; }

        /// <summary>
        /// 告警级别（1普通、2严重、3紧急）
        /// </summary>

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
    }
}