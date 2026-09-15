using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcess
{
    public class ProcessToExcelDto
    {
        [Column("工序编码（必填）")]
        public string Code { set; get; }

        [Column("工序名称（必填）")]
        public string Name { set; get; }

        [Column("工艺要求")]
        public string Attention { get; set; }
    }
}
