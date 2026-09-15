using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class DeviceFullDataDto : DeviceInfoDto
    {
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public virtual string? DeviceTypeName { get; set; }

        /// <summary>
        /// 工艺路线编码集合
        /// </summary>
        public virtual List<string?> RouteCode { get; set; } = new List<string?>();
    }
}
