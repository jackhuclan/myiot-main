using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItemType
{
    public class ItemTypeToExcelDto
    {
        [Column("物料类型编码（必填）")]
        public string Code { get; set; }

        [Column("物料类型名称（必填）")]
        public string Name { get; set; }

        [Column("父对象Id(0表示是根对象)（必填）")]
        public long ParentID { get; set; }

        [Column("物料1产品2")]
        public int ItemOrProduct { get; set; }
    }
}
