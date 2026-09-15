namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockStorageHistory
{
    public class GetMaterialStockStorageHistoryListReq : Page
    {
        /// <summary>
        /// 出入库单号
        /// </summary>
        public virtual string? MaterialStockCode { get; set; }

        /// <summary>
        /// 入库批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

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

