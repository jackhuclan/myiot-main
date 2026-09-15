namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DvInformType
{
    public class AddOrUpdateNotifySettingReq : BaseAddOrUpdateWithTreeDto, IDtoWithTree
    {
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string? NotifyDesc { get; set; }

        /// <summary>
        /// 通知方式
        /// </summary>
        public int? NotifyWays { get; set; }

        /// <summary>
        /// 通知参数
        /// </summary>
        public virtual string? NotifyParams { get; set; }

        /// <summary>
        /// 是否重复发送 
        /// 默认值: 0
        ///</summary>
        public virtual byte IsRepeatSend { get; set; }

        /// <summary>
        /// 发送频率
        /// </summary>
        public virtual int? SendFrequency { get; set; }
    }
}