using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 工艺参数、配方
    ///</summary>
    [SugarTable("t_process_recipe")]
    public class MesProcessRecipe : BaseEntity
    {
        /// <summary 
        /// 名称 
        ///</summary>
        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }

        [SugarColumn(ColumnName = "code")]
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string? Code { get; set; }

        [SugarColumn(ColumnName = "product_code")]
        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string? ProductCode { get; set; }

        [SugarColumn(ColumnName = "product_name")]
        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string? ProductName { get; set; }

        [SugarColumn(ColumnName = "process_code")]
        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }

        [SugarColumn(ColumnName = "process_name")]
        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        [SugarColumn(ColumnName = "parameters")]
        /// <summary>
        /// 参数
        /// json 字符串
        /// </summary>
        public virtual string? Parameters { get; set; }
    }
}
