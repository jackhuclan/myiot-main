namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTransOrder
{
    public class TransOrderDto : BaseDtoWithTreeDto
    {
        /// <summary>
        /// 任务单ID
        /// </summary>
        public int? TaskId { get; set; }

        /// <summary>
        /// 任务单编号
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 生产工单ID
        /// </summary>
        public int? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>

        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>

        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工作站Id
        /// </summary>
        public int? WorkStationId { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public int? ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>

        public int? ItemId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>

        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 流转数量
        /// </summary>
        public decimal? QuantityTransfered { get; set; }

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProduceDate { get; set; }
    }
}
