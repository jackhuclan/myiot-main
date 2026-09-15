using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 库存总览表
    ///</summary>
    [SugarTable("t_material_stock_overview")]
    public class MaterialStockOverview : BaseEntity
    {
        /// <summary>
        /// 产品物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 在库总数量(叠数或片数)
        /// 钻孔/叠板：叠数 ；拆板：片数
        /// </summary>
        [SugarColumn(ColumnName = "sum_onhand")]
        public virtual decimal? SumOnhand { get; set; }

        /// <summary>
        /// 计划占用量
        /// </summary>
        [SugarColumn(ColumnName = "sum_plan")]
        public virtual decimal? SumPlan { get; set; }

        /// <summary>
        /// 预计可用数量
        /// </summary>
        [SugarColumn(ColumnName = "usable_count")]
        public virtual decimal? UsableCount { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [SugarColumn(ColumnName = "process_name")]
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        [SugarColumn(ColumnName = "process_code")]
        public virtual string? ProcessCode { get; set; }
    }
}
