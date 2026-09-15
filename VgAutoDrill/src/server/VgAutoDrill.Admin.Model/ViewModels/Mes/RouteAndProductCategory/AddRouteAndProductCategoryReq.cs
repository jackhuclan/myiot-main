namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory
{
    public class AddRouteAndProductCategoryReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 是否从工艺路线功能发起的更新
        /// </summary>
        public virtual bool FromRoute { get; set; } = false;

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual List<long> RouteIds { get; set; } = new List<long>();

        /// <summary>
        /// 产品大类ID集合
        /// </summary>
        public virtual List<long> ProductCategoryIds { get; set; } = new List<long>();

        /// <summary>
        /// 优先级集合
        /// </summary>
        public virtual List<long> OrderNums { get; set; } = new List<long>();
    }
}
