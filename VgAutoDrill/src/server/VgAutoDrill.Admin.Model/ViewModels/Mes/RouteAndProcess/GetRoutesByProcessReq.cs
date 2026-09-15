namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess
{
    public class GetRoutesByProcessReq : Page
    {
        /// <summary>
        /// 工序编码
        /// </summary>
        public virtual string? ProcessCode { set; get; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }
    }
}
