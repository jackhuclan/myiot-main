using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 工艺路线与产品大类关系表
    ///</summary>
    [SugarTable("t_route_and_product_category")]
    public class RouteAndProductCategory : BaseEntity
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        [SugarColumn(ColumnName = "route_id")]
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 产品大类ID
        /// </summary>
        [SugarColumn(ColumnName = "product_category_id")]
        public virtual long? ProductCategoryId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [SugarColumn(ColumnName = "order_num")]
        public virtual long? OrderNum { get; set; }
    }
}
