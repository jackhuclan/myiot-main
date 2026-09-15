namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder
{
    public class UpdateRouteReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

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