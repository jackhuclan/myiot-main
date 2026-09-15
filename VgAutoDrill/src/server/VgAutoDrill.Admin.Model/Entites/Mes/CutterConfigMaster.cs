using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 钻孔刀具参数主表
    ///</summary>
    [SugarTable("t_cutter_config_master")]
    public class CutterConfigMaster : BaseEntity
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [SugarColumn(ColumnName = "config_name")]
        public virtual string? ConfigName { get; set; }
        /// <summary>
        /// 配置描述
        /// </summary>
        [SugarColumn(ColumnName = "config_desc")]
        public virtual string? ConfigDesc { get; set; }

        /// <summary>
        /// dia文件路径
        /// </summary>
        [SugarColumn(ColumnName = "dia_file_path")]
        public virtual string? DiaFilePath { get; set; }

        /// <summary>
        /// dia文件名
        /// </summary>
        [SugarColumn(ColumnName = "dia_file_name")]
        public virtual string? DiaFileName { get; set; }

        /// <summary>
        /// 产品大类ID
        /// </summary>
        [SugarColumn(ColumnName = "product_category_id")]
        public virtual long? ProductCategoryId { get; set; }

        /// <summary>
        /// 产品大类编码
        /// </summary>
        [SugarColumn(ColumnName = "product_category_code")]
        public virtual string? ProductCategoryCode { get; set; }

        /// <summary>
        /// 产品大类名称
        /// </summary>
        [SugarColumn(ColumnName = "product_category_name")]
        public virtual string? ProductCategoryName { get; set; }
    }
}
