using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 出入库明细表
    ///</summary>
    [SugarTable("t_material_stock_detail")]
    public class MaterialStockDetail : BaseEntity
    {
        /// <summary>
        /// 主表ID
        /// </summary>
        [SugarColumn(ColumnName = "stockId")]
        public virtual long? StockId { get; set; }

        /// <summary>
        /// 板料编号
        /// </summary>
        [SugarColumn(ColumnName = "board_code")]
        public virtual string? BoardCode { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 单叠数量
        /// </summary>
        [SugarColumn(ColumnName = "pcs")]
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        [SugarColumn(ColumnName = "station_id")]
        public virtual long? StationId { get; set; }

        /// <summary>
        /// 料仓编号
        /// </summary>
        [SugarColumn(ColumnName = "silo_code")]
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 当前层（从上往下）
        /// </summary>
        [SugarColumn(ColumnName = "layer")]
        public virtual int? Layer { get; set; }
    }
}
