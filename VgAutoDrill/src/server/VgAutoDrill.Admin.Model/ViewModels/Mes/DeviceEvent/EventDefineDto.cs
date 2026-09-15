namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceEvent
{
    public class EventDefineDto
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { set; get; }

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

        /// <summary>
        /// 创建人Id 
        ///</summary>
        public int? CreatorId { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间 
        ///</summary>
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 修改人Id 
        ///</summary>
        public int? ModifierId { get; set; }
    }
}
