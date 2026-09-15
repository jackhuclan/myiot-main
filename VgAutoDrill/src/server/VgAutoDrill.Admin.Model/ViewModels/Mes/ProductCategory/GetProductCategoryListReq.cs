namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProductCategory
{
    public class GetProductCategoryListReq : Page
    {
        public virtual string? Name { get; set; }

        public virtual string? Code { get; set; }

        public virtual string? Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;

        /// <summary>
        /// 审批状态 
        /// 默认值: 0
        ///</summary>
        public virtual byte? VettingStatus { get; set; }
    }
}
