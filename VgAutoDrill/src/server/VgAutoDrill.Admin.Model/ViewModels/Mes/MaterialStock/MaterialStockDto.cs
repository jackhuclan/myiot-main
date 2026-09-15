namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock
{
    public class MaterialStockDto : BaseDto
    {
        /// <summary>
        /// 出入库单号
        /// </summary>
        public virtual string? Code { get; set; }

        /// <summary>
        /// 料仓编号
        /// </summary>
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 料仓最大层数
        /// </summary>
        public virtual int? MaxLayers { get; set; }

        /// <summary>
        /// 料仓实际层数
        /// </summary>
        public virtual int? CurrentLayers { get; set; }

        /// <summary>
        /// 入库或出库
        /// </summary>
        public virtual string? InOrOut { get; set; }

        /// <summary>
        /// 物料类型ID
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 产品物料ID
        /// </summary>
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 数量(叠数或片数)
        /// 钻孔/叠板：叠数 ；拆板：片数
        /// </summary>
        public decimal? QuantityTransaction { get; set; }

        /// <summary>
        /// 在库数量(叠数或片数)
        /// 钻孔/叠板：叠数 ；拆板：片数
        /// </summary>
        public virtual decimal? QuantityOnhand { get; set; }

        /// <summary>
        /// 入库批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }
        /// <summary>
        /// 工作站ID
        /// </summary>
        public virtual long? StationId { get; set; }

        /// <summary>
        /// 工作站编码
        /// </summary>
        public virtual string? StationCode { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public virtual long? WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public virtual string? WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public virtual string? WarehouseName { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        public virtual long? WorkorderId { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产任务ID
        /// </summary>
        public virtual long? TaskId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 出库匹配入库ID（默认0）
        /// </summary>
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// 出库匹配入库Code
        /// </summary>
        public virtual string? ParentCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }
    }
}
