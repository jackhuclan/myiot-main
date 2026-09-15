using VgAutoDrill.Admin.Model.CentralModels;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor
{
    public class DrillRateFactorSummaryDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceId { get; set; }
        /// <summary> 
        /// 稼动率因素
        /// </summary>
        public virtual DrillRateFactorReason? Reason { get; set; }

        public virtual int ReasonTime { get; set; }

        public virtual int ReasonTimeTotalSeconds { get; set; }

        /// <summary>
        /// 异常报警次数
        /// </summary>
        public virtual int ReasonCount { get; set; }
    }
}
