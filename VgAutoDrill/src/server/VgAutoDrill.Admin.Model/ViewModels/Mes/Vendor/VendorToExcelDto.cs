using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Vendor
{
    public class VendorToExcelDto
    {
        [Column("供应商编码（必填）")]
        public string Code { get; set; }

        [Column("供应商名称（必填）")]
        public string Name { get; set; }

        [Column("备注")]
        public string Remark { get; set; }
    }
}
