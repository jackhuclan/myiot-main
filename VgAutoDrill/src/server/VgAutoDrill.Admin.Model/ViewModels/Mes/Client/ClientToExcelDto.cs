using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Client
{
    public class ClientToExcelDto
    {
        [Column("客户编码（必填）")]
        public string Code { get; set; }

        [Column("客户名称（必填）")]
        public string Name { get; set; }

        [Column("备注")]
        public string Remark { get; set; }
    }
}
