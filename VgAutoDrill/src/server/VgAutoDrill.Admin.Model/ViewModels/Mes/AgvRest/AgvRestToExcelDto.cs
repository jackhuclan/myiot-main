using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest
{

    public class AgvRestToExcelDto
    {
        [Column("休息点编码（必填）")]
        public virtual string? Code { get; set; }

        [Column("休息点名称（必填）")]
        public virtual string? Name { get; set; }

        [Column("物理点位")]
        public virtual string? Point { get; set; }

    }
}
