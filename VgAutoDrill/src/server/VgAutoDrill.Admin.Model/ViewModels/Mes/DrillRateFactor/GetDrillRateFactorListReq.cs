using VgAutoDrill.Admin.Model.CentralModels;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor
{
    public class GetDrillRateFactorListReq : Page
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
        /// 查询开始时间
        /// </summary>
        public DateTime? QueryStartTime { set; get; }

        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime? QueryEndTime { set; get; }
    }
}
