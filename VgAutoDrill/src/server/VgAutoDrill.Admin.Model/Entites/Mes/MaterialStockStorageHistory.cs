using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 库存入库单历史表
    ///</summary>
    [SugarTable("t_material_stock_storage_history")]
    public class MaterialStockStorageHistory : BaseEntity
    {
        /// <summary>
        /// 出入库单号
        /// </summary>
        [SugarColumn(ColumnName = "material_stock_code")]
        public virtual string? MaterialStockCode { get; set; }

        /// <summary>
        /// 入库批次号
        /// </summary>
        [SugarColumn(ColumnName = "batch_code")]
        public virtual string? BatchCode { get; set; }

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
        /// 工序名称
        /// </summary>
        [SugarColumn(ColumnName = "process_name")]
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        [SugarColumn(ColumnName = "process_code")]
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 在库数量(叠数或片数)
        /// 钻孔/叠板：叠数 ；拆板：片数
        /// </summary>
        [SugarColumn(ColumnName = "quantity_onhand")]
        public virtual decimal? QuantityOnhand { get; set; }
    }
}
