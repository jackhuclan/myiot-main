using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class DrillDeviceTaskInfo : TaskDto
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public virtual string? DeviceId { get; set; }
    }
}
