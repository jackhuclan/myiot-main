namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockDetail
{
    public class GetMaterialStockDetailListReq : Page
    {
        /// <summary>
        /// 板料编号
        /// </summary>
        public virtual string? BoardCode { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 料仓编号
        /// </summary>
        public virtual string? SiloCode { get; set; }
    }
}
