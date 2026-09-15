using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class TaskToExcelDto
    {
        [Column("生产任务编码")]
        public string? Code { set; get; }

        [Column("生产任务名称")]
        public string? Name { set; get; }

        [Column("父对象Id")]
        public long? ParentId { get; set; }

        [Column("生产工单ID")]
        public long? WorkOrderId { get; set; }

        [Column("生产工单名称")]
        public string? WorkOrderName { get; set; }

        [Column("生产工单编号")]
        public string? WorkOrderCode { get; set; }

        [Column("工作站Id")]
        public long? WorkStationId { get; set; }

        [Column("工作站名称")]
        public string? WorkStationName { get; set; }

        [Column("工作站编号")]
        public string? WorkStationCode { get; set; }

        [Column("工序Id")]
        public long? ProcessId { get; set; }

        [Column("工序名称")]
        public string? ProcessName { get; set; }

        [Column("工序编号")]
        public string? ProcessCode { get; set; }

        [Column("产品ID")]
        public long? ItemId { get; set; }

        [Column("产品名称")]
        public string? ItemName { get; set; }

        [Column("产品编号")]
        public string? ItemCode { get; set; }

        [Column("物料产品类型Id")]
        public long? ItemTypeId { get; set; }

        [Column("批次号")]
        public string? BatchCode { get; set; }

        [Column("规格型号")]
        public string? Specification { get; set; }

        [Column("单位")]
        public string? UnitOfMeasure { get; set; }

        [Column("排产数量")]
        public decimal? Quantity { get; set; }

        [Column("已生产数量")]
        public decimal? QuantityProduced { get; set; }

        [Column("良品数量")]
        public decimal? QuantityQuanlify { get; set; }

        [Column("不良品数量")]
        public decimal? QuantityUnquanlify { get; set; }

        [Column("本次排产叠数")]
        public virtual decimal? NowWadCount { get; set; }

        [Column("客户Id")]
        public long? ClientId { get; set; }

        [Column("客户名称")]
        public string? ClientName { get; set; }

        [Column("客户编号")]
        public string? ClientCode { get; set; }

        [Column("开始日期")]
        public DateTime? StartTime { get; set; }

        [Column("生产时长")]
        public int? Duration { get; set; }

        [Column("结束日期")]
        public DateTime? EndTime { get; set; }

        [Column("需求日期")]
        public DateTime? RequestDate { get; set; }

        [Column("完工状态")]
        public string? TaskStatus { get; set; }

        [Column("TASK的颜色")]
        public string? Color { get; set; }

        [Column("是否关键工序")]
        public string? KeyFlag { get; set; }
    }
}
