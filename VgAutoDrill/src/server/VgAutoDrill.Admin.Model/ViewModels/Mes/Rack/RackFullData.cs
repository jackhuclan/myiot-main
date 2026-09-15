using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class RackFullData : RackDto
    {
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 库位是否就绪（从中控系统直接获取）
        /// </summary>
        public virtual bool? IsReady { get; set; }

        /// <summary>
        /// 是否已被预约
        /// </summary>
        public virtual bool? Appointed { get; set; }
        public virtual string? AppointedMessage { get; set; }

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

        /// <summary>
        /// 料仓载料信息汇总
        /// </summary>
        public virtual List<SiloItemSumInfo>? SiloInfos { get; set; } = new List<SiloItemSumInfo>();
    }
}
