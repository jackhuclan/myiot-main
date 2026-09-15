using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 库存记录表
    ///</summary>
    [SugarTable("t_material_stock")]
    public class MaterialStock : BaseEntity
    {
        /// <summary>
        /// 出入库单号
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        /// <summary>
        /// 料仓编号
        /// </summary>
        [SugarColumn(ColumnName = "silo_code")]
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 料仓最大层数
        /// </summary>
        [SugarColumn(ColumnName = "max_layers")]
        public virtual int? MaxLayers { get; set; }

        /// <summary>
        /// 料仓实际层数
        /// </summary>
        [SugarColumn(ColumnName = "current_layers")]
        public virtual int? CurrentLayers { get; set; }

        /// <summary>
        /// 入库或出库（in/out）
        /// </summary>
        [SugarColumn(ColumnName = "in_or_out")]
        public virtual string? InOrOut { get; set; }

        /// <summary>
        /// 物料类型ID
        /// </summary>
        [SugarColumn(ColumnName = "item_type_id")]
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 产品物料ID
        /// </summary>
        [SugarColumn(ColumnName = "item_id")]
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]
        public virtual string? ItemName { get; set; }

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
        /// 数量(叠数或片数)
        /// 钻孔/叠板：叠数 ；拆板：片数
        /// </summary>
        [SugarColumn(ColumnName = "quantity_transaction")]
        public virtual decimal? QuantityTransaction { get; set; }

        /// <summary>
        /// 在库数量(叠数或片数)
        /// 钻孔/叠板：叠数 ；拆板：片数
        /// </summary>
        [SugarColumn(ColumnName = "quantity_onhand")]
        public virtual decimal? QuantityOnhand { get; set; }

        /// <summary>
        /// 入库批次号
        /// </summary>
        [SugarColumn(ColumnName = "batch_code")]
        public virtual string? BatchCode { get; set; }
        /// <summary>
        /// 工作站ID
        /// </summary>
        [SugarColumn(ColumnName = "station_id")]
        public virtual long? StationId { get; set; }

        /// <summary>
        /// 工作站编码
        /// </summary>
        [SugarColumn(ColumnName = "station_code")]
        public virtual string? StationCode { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_id")]
        public virtual long? WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_code")]
        public virtual string? WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        [SugarColumn(ColumnName = "warehouse_name")]
        public virtual string? WarehouseName { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        [SugarColumn(ColumnName = "workorder_id")]
        public virtual long? WorkorderId { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        [SugarColumn(ColumnName = "workorder_code")]
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产任务ID
        /// </summary>
        [SugarColumn(ColumnName = "task_id")]
        public virtual long? TaskId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 出库匹配入库ID（默认0）
        /// </summary>
        [SugarColumn(ColumnName = "parent_id")]
        public virtual long? ParentId { get; set; }

        /// <summary>
        /// 出库匹配入库Code
        /// </summary>
        [SugarColumn(ColumnName = "parent_code")]
        public virtual string? ParentCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [SugarColumn(ColumnName = "process_name")]
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        [SugarColumn(ColumnName = "process_code")]
        public virtual string? ProcessCode { get; set; }
    }
}
