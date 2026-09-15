namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory
{
    public class UpdateRouteAndProductCategoryReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 产品大类ID
        /// </summary>
        public virtual long? ProductCategoryId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual long? OrderNum { get; set; }
    }
}
