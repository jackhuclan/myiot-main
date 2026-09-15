using VgAutoDrill.Admin.Model.CentralModels;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor
{
    public class DrillRateFactorDto : BaseDto
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public virtual string? DeviceId { get; set; }
        /// <summary> 
        /// 稼动率因素
        /// </summary>
        public virtual DrillRateFactorReason? Reason { get; set; }

        /// <summary> 
        /// 稼动率因素描述
        /// </summary>
        public virtual string? ReasonDesc { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime? EndTime { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        public virtual int? IntervalTime { get; set; }

        /// <summary>
        /// 班次
        /// </summary>
        public virtual string? DateSailings { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Memo { get; set; }
    }
}
