namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockOverviewHistory
{
    public class GetMaterialStockOverviewHistoryListReq : Page
    {
        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }
    }
}
