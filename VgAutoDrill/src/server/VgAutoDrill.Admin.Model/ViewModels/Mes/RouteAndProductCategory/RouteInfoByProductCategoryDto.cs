namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory
{
    public class RouteInfoByProductCategoryDto : BaseDto
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

        /// <summary>
        /// 工艺路线说明
        /// </summary>
        public virtual string? RouteDesc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? RouteRemark { get; set; }

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
