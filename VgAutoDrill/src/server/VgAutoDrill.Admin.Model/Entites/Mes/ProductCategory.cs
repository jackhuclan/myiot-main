using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 产品大类
    ///</summary>
    [SugarTable("t_product_category")]
    public class ProductCategory : BaseEntityWithTree
    {
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]

        public virtual string? Remark { get; set; }

        [SugarColumn(ColumnName = "dispense_machines")]
        /// <summary>
        /// 建议机台数
        /// </summary>
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 审批状态 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "vetting_status")]
        public virtual byte VettingStatus { get; set; } = 0;
    }
}
