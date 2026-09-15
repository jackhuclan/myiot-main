using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// ATP文件
    /// </summary>
    [SugarTable("t_item_atp_file")]
    public class ItemAtpFile : BaseEntity
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
        /// ATP文件名
        /// </summary>
        [SugarColumn(ColumnName = "atp_file_name")]
        public virtual string? AtpFileName { get; set; }

        /// <summary>
        /// ATP文件路径
        /// </summary>
        [SugarColumn(ColumnName = "atp_file_path")]
        public virtual string? ATPFilePath { get; set; }

        /// <summary>
        /// ATP参数
        /// </summary>
        [SugarColumn(ColumnName = "atp_parameters")]
        public virtual string? ATPParameters { get; set; }

        /// <summary>
        /// 钻带参数ID
        /// </summary>
        [SugarColumn(ColumnName = "item_drill_file_id")]
        public virtual long? ItemDrillFileId { get; set; }

        /// <summary>
        /// 钻带参数文件名
        /// </summary>
        [SugarColumn(ColumnName = "item_drill_file_name")]
        public virtual string? ItemDrillFileName { get; set; }

        /// <summary>
        /// 钻带参数路径
        /// </summary>
        [SugarColumn(ColumnName = "item_drill_file_path")]
        public virtual string? ItemDrillFilePath { get; set; }

        /// <summary>
        /// 钻孔刀具参数主表ID
        /// </summary>
        [SugarColumn(ColumnName = "cutter_config_master_id")]
        public virtual long? CutterConfigMasterId { get; set; }

        /// <summary>
        /// 刀盘数量
        /// </summary>
        [SugarColumn(ColumnName = "disk_count")]
        public virtual int? DiskCount { get; set; }

        /// <summary>
        /// 单盘刀列数
        /// </summary>
        [SugarColumn(ColumnName = "columns_limit")]
        public virtual int? ColumnsLimit { get; set; }

        /// <summary>
        /// 单盘刀行数
        /// </summary>
        [SugarColumn(ColumnName = "rows_limit")]
        public virtual int? RowsLimit { get; set; }

        /// <summary>
        /// 是否自动生成
        /// </summary>
        [SugarColumn(ColumnName = "is_generated")]
        public virtual int? IsGenerated { get; set; }

        /// <summary>
        /// 自动生成时间
        /// </summary>
        [SugarColumn(ColumnName = "generate_time")]
        public virtual DateTime? GenerateTime { get; set; }

        /// <summary>
        /// dia文件名
        /// </summary>
        [SugarColumn(ColumnName = "dia_file_name")]
        public virtual string? DiaFileName { get; set; }

        /// <summary>
        /// dia文件路径
        /// </summary>
        [SugarColumn(ColumnName = "dia_file_path")]
        public virtual string? DiaFilePath { get; set; }
    }
}
