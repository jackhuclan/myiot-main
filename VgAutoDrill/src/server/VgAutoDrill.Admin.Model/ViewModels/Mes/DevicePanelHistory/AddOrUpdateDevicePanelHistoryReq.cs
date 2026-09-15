using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory
{
    public class AddOrUpdateDevicePanelHistoryReq : AddOrUpdateDevicePanelReq
    {
        /// <summary>
        /// 设备负载板料时间
        /// </summary>
        public virtual DateTime? DevicePanelTime { get; set; }
    }
}
