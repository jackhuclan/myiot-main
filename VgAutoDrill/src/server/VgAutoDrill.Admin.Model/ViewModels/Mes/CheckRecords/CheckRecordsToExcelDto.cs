using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CheckRecords
{
    public class CheckRecordsToExcelDto
    {
        [Column("生产任务名称")]
        public string? TaskName { get; set; }

        [Column("生产任务编号")]
        public string? TaskCode { get; set; }

        [Column("生产工单名称")]
        public string? WorkOrderName { get; set; }

        [Column("生产工单编号")]
        public string? WorkOrderCode { get; set; }

        [Column("检验人员姓名")]
        public string? UserName { get; set; }

        [Column("是否检验合格")]
        public string? IsCheckOk { get; set; }

        [Column("检验日期")]
        public DateTime? CheckTime { get; set; }

        [Column("备注")]
        public string? Remark { get; set; }
    }
}
