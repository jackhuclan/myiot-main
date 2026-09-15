namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory
{
    public class GetRouteAndProductCategoryListReq : Page
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 产品大类ID
        /// </summary>
        public virtual long? ProductCategoryId { get; set; }
    }
}
