namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProductCategory
{
    /// <summary>
    /// 审批产品大类
    /// </summary>
    public class VettingProductCategoryReq
    {
        public List<VettingProductCategoryDto>? VettingProductCategorys { get; set; }
    }

    public class VettingProductCategoryDto
    {
        /// <summary>
        /// 产品大类ID
        /// </summary>
        public virtual long ProductCategoryId { get; set; }
    }
}
