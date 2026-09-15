using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 物料产品定义表
    ///</summary>
    [SugarTable("t_item")]
    public class Item : BaseEntityWithTree
    {
        /// <summary>
        /// 板料长度
        /// </summary>
        [SugarColumn(ColumnName = "panel_length")]
        public float PanelLength { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [SugarColumn(ColumnName = "incode_number")]
        public virtual string? IncodeNumber { get; set; }
        /// <summary>
        /// 物料1产品2
        /// Item Or Product
        /// </summary>
        [SugarColumn(ColumnName = "item_or_product")]

        public virtual int? ItemOrProduct { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [SugarColumn(ColumnName = "specification")]

        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [SugarColumn(ColumnName = "unit_of_measure")]

        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        [SugarColumn(ColumnName = "item_type_id")]
        public virtual long? ItemTypeId { get; set; }
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
        /// <summary>
        /// 库房ID
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_id")]
        public virtual int? WarehouseId { get; set; }
        /// <summary>
        /// 库房编码
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_code")]
        public virtual string? WarehouseCode { get; set; }
        /// <summary>
        /// 库房名称
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_name")]
        public virtual string? WarehouseName { get; set; }

        /// <summary>
        /// 建议机台数
        /// </summary>
        [SugarColumn(ColumnName = "dispense_machines")]
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 钻带文件路径
        /// </summary>
        [SugarColumn(ColumnName = "drill_file_path")]
        public virtual string? DrillFilePath { get; set; }

        /// <summary>
        /// 板料长度
        /// </summary>
        [SugarColumn(ColumnName = "panel_width")]
        public float PanelWidth { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [SugarColumn(ColumnName = "layer_num")]
        public virtual int? LayerNum { get; set; }
        /// <summary>
        /// 叠数
        /// </summary>
        [SugarColumn(ColumnName = "panel_count")]
        public virtual int? PanelCount { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        [SugarColumn(ColumnName = "spec_group")]
        public virtual string? SpecGroup { get; set; }

        /// <summary>
        /// 转换前文件路径
        /// </summary>
        [SugarColumn(ColumnName = "before_drill_file_path")]
        public virtual string? BeforeDrillFilePath { get; set; }

        /// <summary>
        /// 转换后文件路径
        /// </summary>
        [SugarColumn(ColumnName = "after_drill_file_path")]
        public virtual string? AfterDrillFilePath { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public virtual string? CreatorName { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public virtual string? ModifierName { get; set; }
    }
}
