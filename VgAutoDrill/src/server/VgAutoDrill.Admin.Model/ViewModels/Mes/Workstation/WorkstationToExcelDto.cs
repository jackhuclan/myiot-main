using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation
{
    public class WorkstationToExcelDto
    {
        [Column("工作站编码（必填）")]
        public string Code { get; set; }

        [Column("工作站名称（必填）")]
        public string Name { get; set; }

        [Column("所在车间ID")]
        public int WorkshopId { get; set; }

        [Column("所在车间编码")]
        public string WorkshopCode { get; set; }

        [Column("所在车间名称")]
        public string WorkshopName { get; set; }

        [Column("备注")]
        public string Remark { get; set; }
    }
}
