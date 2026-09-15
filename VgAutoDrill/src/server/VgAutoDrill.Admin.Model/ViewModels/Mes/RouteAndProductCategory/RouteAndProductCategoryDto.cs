namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory
{
    public class RouteAndProductCategoryDto : BaseDto
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
        /// 产品大类名称
        /// </summary>
        public virtual string? ProductCategoryName { get; set; }

        /// <summary>
        /// 产品大类编码
        /// </summary>
        public virtual string? ProductCategoryCode { get; set; }
    }
}
