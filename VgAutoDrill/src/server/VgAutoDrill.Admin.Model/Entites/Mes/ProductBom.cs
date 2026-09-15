using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 产品结构表
    ///</summary>
    [SugarTable("t_product_bom")]
    public class ProductBom : BaseEntityWithTree
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "item_id")]
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 排产数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity")]
        public virtual decimal? Quantity { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "order_num")]
        public virtual int? OrderNum { get; set; }
    }
}
