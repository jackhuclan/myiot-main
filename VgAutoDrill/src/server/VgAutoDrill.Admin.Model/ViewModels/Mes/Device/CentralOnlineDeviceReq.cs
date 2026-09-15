using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class CentralOnlineDeviceReq : Page
    {
        public virtual string? ProductId { get; set; }

        public virtual string? DeviceId { get; set; }

        public virtual string? TargetDevice { get; set; }

        public virtual string? Status { get; set; }

        public virtual string? RoutingKey { get; set; }

        public virtual bool? NotActive { get; set; }

        /// <summary>
        /// 自动机器还是手动机器
        /// </summary>
        public virtual bool? IsAuto { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        public virtual List<DeviceKind>? RequestDeviceKindList { get; set; }
    }
}
