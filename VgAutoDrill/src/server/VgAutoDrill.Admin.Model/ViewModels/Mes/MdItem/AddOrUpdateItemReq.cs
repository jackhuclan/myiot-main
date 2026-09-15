namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem
{
    public class AddOrUpdateItemReq : BaseAddOrUpdateWithTreeDto, IDtoWithTree
    {
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelLength { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public virtual string? IncodeNumber { get; set; }
        /// <summary>
        /// 物料1产品2
        /// Item Or Product
        /// </summary>
        public virtual int? ItemOrProduct { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>

        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual int? ItemTypeId { get; set; }
        /// <summary>
        /// 产品大类ID
        /// </summary>
        public virtual int? ProductCategoryId { get; set; }
        /// <summary>
        /// 产品大类编码
        /// </summary>
        public virtual string? ProductCategoryCode { get; set; }
        /// <summary>
        /// 产品大类名称
        /// </summary>
        public virtual string? ProductCategoryName { get; set; }
        /// <summary>
        /// 库房ID
        /// </summary>
        public virtual int? WarehouseId { get; set; }
        /// <summary>
        /// 库房编码
        /// </summary>
        public virtual string? WarehouseCode { get; set; }
        /// <summary>
        /// 库房名称
        /// </summary>
        public virtual string? WarehouseName { get; set; }

        /// <summary>
        /// 建议机台数
        /// </summary>
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 钻带文件路径
        /// </summary>
        public virtual string? DrillFilePath { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelWidth { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public virtual int? LayerNum { get; set; }
        /// <summary>
        /// 叠板层数，
        /// </summary>
        public virtual decimal? PanelCount { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        public virtual string? SpecGroup { get; set; }

        /// <summary>
        /// 转换前文件路径
        /// </summary>
        public virtual string? BeforeDrillFilePath { get; set; }

        /// <summary>
        /// 转换后文件路径
        /// </summary>
        public virtual string? AfterDrillFilePath { get; set; }
    }
}