using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute
{
    public class DeviceFullDataAndRouteInfo : DeviceDto
    {
        /// <summary>
        /// 工艺路线名称组合
        /// </summary>
        public virtual string? RouteNameDescription { get; set; }
    }
}
