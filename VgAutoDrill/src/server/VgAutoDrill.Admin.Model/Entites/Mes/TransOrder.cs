using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 生产流转单
    ///</summary>
    [SugarTable("t_trans_order")]
    public class TransOrder : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(ColumnName = "name")]
        public virtual string? Name { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [SugarColumn(ColumnName = "code")]
        public virtual string? Code { get; set; }

        /// <summary>
        /// 任务单ID
        /// </summary>
        [SugarColumn(ColumnName = "task_id")]
        public int? TaskId { get; set; }

        /// <summary>
        /// 任务单编号
        /// </summary>
        [SugarColumn(ColumnName = "task_code")]
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        [SugarColumn(ColumnName = "work_order_id")]
        public int? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        [SugarColumn(ColumnName = "work_order_name")]

        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        [SugarColumn(ColumnName = "work_order_code")]

        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [SugarColumn(ColumnName = "batch_code")]

        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 工作站Id
        /// </summary>
        [SugarColumn(ColumnName = "work_station_id")]
        public int? WorkStationId { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        [SugarColumn(ColumnName = "work_station_name")]

        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        [SugarColumn(ColumnName = "work_station_code")]

        public virtual string? WorkStationCode { get; set; }


        /// <summary>
        /// 工序Id
        /// </summary>
        [SugarColumn(ColumnName = "process_id")]
        public int? ProcessId { get; set; }

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

        /// <summary>
        /// 产品ID
        /// </summary>
        [SugarColumn(ColumnName = "item_id")]
        public int? ItemId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]

        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]

        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        [SugarColumn(ColumnName = "item_type_id")]
        public virtual long? ItemTypeId { get; set; }

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
        /// 流转数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_transfered")]
        public decimal? QuantityTransfered { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        [SugarColumn(ColumnName = "produce_date")]
        public DateTime? ProduceDate { get; set; }
    }
}
