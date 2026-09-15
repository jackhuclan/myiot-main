using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 物料产品类别
    ///</summary>
    [SugarTable("t_item_type")]
    public class ItemType : BaseEntityWithTree
    {
        /// <summary>
        /// 物料1产品2
        /// Item Or Product
        /// </summary>
        [SugarColumn(ColumnName = "item_or_product")]
        public virtual int? ItemOrProduct { get; set; }

        /// <summary>
        /// 是否系统自带
        /// </summary>
        [SugarColumn(ColumnName = "is_system")]
        public virtual string? IsSystem { get; set; }
    }
}
