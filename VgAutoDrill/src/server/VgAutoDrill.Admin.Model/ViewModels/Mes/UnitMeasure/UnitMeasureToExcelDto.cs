using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.UnitMeasure
{
    public class UnitMeasureToExcelDto
    {
        [Column("单位编码（必填）")]
        public string Code { get; set; }

        [Column("单位名称（必填）")]
        public string Name { get; set; }

        [Column("是否是主单位Y/N（必填）")]
        public string PrimaryFlag { get; set; }

        [Column("主单位ID")]
        public int PrimaryId { get; set; }

        [Column("与主单位换算比例")]
        public decimal ChangeRate { get; set; }

        [Column("备注")]
        public string Remark { get; set; }
    }
}
