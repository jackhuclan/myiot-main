using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory
{
    public class DevicePanelHistoryDto : DevicePanelDto
    {
        /// <summary>
        /// 设备负载板料时间
        /// </summary>
        public virtual DateTime? DevicePanelTime { get; set; }

        /// <summary>
        /// 前端使用，不需存数据库
        /// </summary>
        public virtual bool IsChecked { get; set; } = false;

        public virtual string? ProductStatusDesc { get; set; }
    }
}
