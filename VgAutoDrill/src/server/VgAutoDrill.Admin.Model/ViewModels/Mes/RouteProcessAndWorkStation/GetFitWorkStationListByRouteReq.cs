using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation
{
    /// <summary>
    /// 根据工艺路线、工序获取工作站（按空闲时间排序）
    /// </summary>
    public class GetFitWorkStationListByRouteReq : Page
    {
        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual List<DeviceStatus>? DeviceStatusList { set; get; }
    }
}
