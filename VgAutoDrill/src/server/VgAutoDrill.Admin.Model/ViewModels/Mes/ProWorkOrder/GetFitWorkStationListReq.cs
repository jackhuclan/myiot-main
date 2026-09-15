using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder
{
    /// <summary>
    /// 根据工单号，机台数，获取可用的机台数据
    /// </summary>
    public class GetFitWorkStationListReq : Page
    {

        /// <summary>
        /// 工单ID
        /// </summary>
        public virtual long? WorkOrderId { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 需要的机台数量
        /// </summary>
        public virtual int FitCount { get; set; }

        /// <summary>
        /// 工序编码，默认drill
        /// </summary>
        public virtual string ProcessCode { get; set; } = "drill";

        /// <summary>
        /// 工作站编码
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual List<DeviceStatus>? DeviceStatusList { set; get; }
    }
}
