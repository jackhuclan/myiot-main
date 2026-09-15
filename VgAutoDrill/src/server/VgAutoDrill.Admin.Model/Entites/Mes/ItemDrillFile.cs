using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 钻带参数
    /// </summary>
    [SugarTable("t_item_drill_file")]
    public class ItemDrillFile : BaseEntity
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        [SugarColumn(ColumnName = "item_id")]
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        [SugarColumn(ColumnName = "item_type_id")]
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 钻带参数文件名
        /// </summary>
        [SugarColumn(ColumnName = "drill_file_name")]
        public virtual string? DrillFileName { get; set; }

        /// <summary>
        /// 钻带参数路径
        /// </summary>
        [SugarColumn(ColumnName = "drill_file_path")]
        public virtual string? DrillFilePath { get; set; }
    }
}
