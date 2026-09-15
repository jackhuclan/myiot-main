namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceEvent
{
    public class AddOrUpdateEventDefineReq : BaseAddOrUpdateDto
    {

        /// <summary>
        /// 事件ID
        /// </summary>
        public virtual string? EventId { get; set; }

        /// <summary>
        /// 事件名称
        /// </summary>
        public virtual string? EventName { get; set; }

        /// <summary>
        /// 参数配置
        /// </summary>
        public virtual string? ParameterJson { get; set; }

        public virtual int? EventLevel { get; set; }

        public int? DeviceTypeId { get; set; }
    }
}