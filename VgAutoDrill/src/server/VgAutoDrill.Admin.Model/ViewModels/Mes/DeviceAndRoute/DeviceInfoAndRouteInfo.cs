namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute
{
    public class DeviceInfoAndRouteInfo : DeviceAndRouteDto
    {
        /// <summary>
        /// 工艺路线说明
        /// </summary>
        public virtual string? RouteDesc { get; set; }

        /// <summary>
        /// 工艺路线备注
        /// </summary>
        public virtual string? RouteRemark { get; set; }

        /// <summary>
        /// 工艺路线审批状态 
        ///</summary>
        public virtual byte RouteVettingStatus { get; set; }
    }
}
