using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack
{
    public class FeedBackToExcelDto
    {
        [Column("报工类型")]
        public string? FeedBackType { get; set; }

        [Column("工作站ID")]
        public long? WorkStationId { get; set; }

        [Column("工作站编号")]
        public string? WorkStationCode { get; set; }

        [Column("工作站名称")]
        public string? WorkStationName { get; set; }

        [Column("生产工单ID")]
        public long? WorkOrderId { get; set; }

        [Column("生产工单编号")]
        public string? WorkOrderCode { get; set; }

        [Column("生产工单名称")]
        public string? WorkOrderName { get; set; }

        [Column("工序ID")]
        public long? ProcessId { get; set; }

        [Column("工序编码")]
        public string? ProcessCode { get; set; }

        [Column("工序名称")]
        public string? ProcessName { get; set; }

        [Column("生产任务ID")]
        public long? TaskId { get; set; }

        [Column("生产任务编号")]
        public string? TaskCode { get; set; }

        [Column("产品物料ID")]
        public long? ItemId { get; set; }

        [Column("产品物料编码")]
        public string? ItemCode { get; set; }

        [Column("产品物料名称")]
        public string? ItemName { get; set; }

        [Column("物料产品类型Id")]
        public long? ItemTypeId { get; set; }

        [Column("单位")]
        public string? UnitOfMeasure { get; set; }

        [Column("规格型号")]
        public string? Specification { get; set; }

        [Column("排产数量")]
        public decimal? Quantity { get; set; }

        [Column("本次报工数量")]
        public decimal? QuantityFeedBack { get; set; }

        [Column("良品数量")]
        public decimal? QuantityQualified { get; set; }

        [Column("不良品数量")]
        public decimal? QuantityUnQuanlified { get; set; }

        [Column("报工用户名")]
        public string? UserName { get; set; }

        [Column("昵称")]
        public string? NickName { get; set; }

        [Column("报工途径")]
        public string? FeedBackChannel { get; set; }

        [Column("报工时间")]
        public DateTime? FeedBackTime { get; set; }

        [Column("完工状态")]
        public string? FeedBackStatus { get; set; }

        [Column("是否关键工序")]
        public string? KeyFlag { get; set; }

        [Column("备注")]
        public string? Remark { get; set; }
    }
}
