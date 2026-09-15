using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProductCategory
{
    public class ProductCategoryToExcelDto
    {
        [Column("产品大类编码（必填）")]
        public string Code { get; set; }

        [Column("产品大类名称（必填）")]
        public string Name { get; set; }

        [Column("父对象Id(0表示是根对象)（必填）")]
        public long ParentID { get; set; }

        [Column("备注")]
        public string Remark { get; set; }
        [Column("建议机台数")]
        public virtual decimal? DispenseMachines { get; set; }
    }
}
