namespace VgAutoDrill.Admin.Model.ViewModels.Mes.NotificationRecord
{
    public class GetNotifyListReq : Page
    {
        /// <summary>
        /// 通知类型编号
        /// </summary>
        public virtual string? NotifyCode { get; set; }

        /// <summary>
        /// 通知类型名称
        /// </summary>
        public virtual string? NotifyName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
    }
}
