namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class CentralRackDto
    {
        public virtual string? Code { get; set; }

        public virtual string? SiloCode { get; set; }

        public virtual string? DeviceId { get; set; }
        
        /// <summary>
        /// 是否已被预约
        /// </summary>
        public virtual bool? Appointed { get; set; }
        public virtual string? AppointedMessage { get; set; }

        /// <summary>
        /// 料架是否就绪（从中控系统直接获取）
        /// </summary>
        public virtual bool? IsReady { get; set; }

        /// <summary>
        /// 是否存在告警
        /// </summary>
        public virtual bool? HasAlarm { get; set; }

        /// <summary>
        /// 告警信息
        /// </summary>
        public virtual string? AlarmMessage { get; set; }

        /// <summary>
        /// 钻机是否能够上下料
        /// </summary>
        public virtual bool? IsAvailableForAgv { get; set; }

        public virtual List<VgAutoDrill.Fundation.Iot.Models.Panel>? Panels { get; set; }
    }
}
