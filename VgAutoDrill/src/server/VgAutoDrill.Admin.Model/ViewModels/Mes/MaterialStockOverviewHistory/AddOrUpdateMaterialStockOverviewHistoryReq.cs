namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockOverviewHistory
{
    public class AddOrUpdateMaterialStockOverviewHistoryReq : BaseAddOrUpdateDto
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
        /// 在库总数量(叠数或片数)
        /// 钻孔/叠板：叠数 ；拆板：片数
        /// </summary>
        public virtual decimal? SumOnhand { get; set; }

        /// <summary>
        /// 计划占用量
        /// </summary>
        public virtual decimal? SumPlan { get; set; }

        /// <summary>
        /// 预计可用数量
        /// </summary>
        public virtual decimal? UsableCount { get; set; }

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
