namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceEvent
{
    public class GetEventDefineListReq : Page
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public virtual string? EventId { get; set; }
        /// <summary>
        /// 事件名称
        /// </summary>
        public virtual string? EventName { get; set; }

        public virtual int? EventLevel { get; set; }

        public int? DeviceTypeId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { set; get; } = -1;
    }
}
