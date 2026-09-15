namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProductCategory
{
    public class AddOrUpdateProductCategoryReq : BaseAddOrUpdateWithTreeDto, IDtoWithTree
    {
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
        /// <summary>
        /// 建议机台数
        /// </summary>
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 审批状态 
        /// 默认值: 0
        ///</summary>
        public virtual byte? VettingStatus { get; set; }
    }
}
