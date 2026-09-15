using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute
{
    public class GetDeviceFullDataAndRouteInfoReq : Page
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public virtual string? DeviceName { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual List<DeviceStatus>? DeviceStatusList { set; get; }
        public virtual List<DeviceKind>? RequestDeviceKindList { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public virtual int? DeviceTypeId { get; set; }

        /// <summary>
        /// 设备类型Code
        /// </summary>
        public virtual string? DeviceTypeCode { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }

    }
}
