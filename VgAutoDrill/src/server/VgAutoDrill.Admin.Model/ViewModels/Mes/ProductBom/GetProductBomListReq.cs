namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProductBom
{
    public class GetProductBomListReq : Page
    {
        /// <summary>
        /// 产品结构名称
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// 产品结构编码
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 父对象编码
        /// </summary>
        public virtual string? ParentCode { get; set; }

        /// <summary>
        /// 父对象名称
        /// </summary>
        public virtual string? ParentName { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 排产数量
        /// </summary>
        public virtual decimal? Quantity { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual int? OrderNum { get; set; }
    }
}
