namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockDetail
{
    public class MaterialStockDetailDto : BaseDto
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        public virtual long? StockId { get; set; }

        /// <summary>
        /// 板料编号
        /// </summary>
        public virtual string? BoardCode { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 单叠数量
        /// </summary>
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        public virtual long? StationId { get; set; }

        /// <summary>
        /// 料仓编号
        /// </summary>
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 当前层（从上往下）
        /// </summary>
        public virtual int? Layer { get; set; }
    }
}
