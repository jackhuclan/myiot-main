using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class DeviceDataToScreen
    {
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }
        /// <summary>
        /// 钻机状态，WORK  STOP  ALAM  WAIT
        /// </summary>
        public virtual string? DrillState { get; set; }
        /// <summary>
        /// 稼动率
        /// </summary>
        public virtual int? Duty { get; set; }

        /// <summary>
        /// 稼动率达成率
        /// </summary>
        public virtual int? DutyRate { get; set; }

        /// <summary>
        /// 设备生产进度，百分比，0~100
        /// </summary>
        public virtual int Percentage { get; set; }

        /// <summary>
        /// 当前料号
        /// </summary>
        public virtual string? NowItemCode { get; set; }

        /// <summary>
        /// 下一个料号
        /// </summary>
        public virtual string? NextItemCode { get; set; }
    }
}
