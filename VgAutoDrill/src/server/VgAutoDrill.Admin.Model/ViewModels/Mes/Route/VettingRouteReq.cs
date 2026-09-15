namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Route
{
    /// <summary>
    /// 审批工艺路线
    /// </summary>
    public class VettingRouteReq
    {
        public List<VettingRouteDto>? VettingRoutes { get; set; }
    }

    public class VettingRouteDto
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long RouteId { get; set; }
    }
}
