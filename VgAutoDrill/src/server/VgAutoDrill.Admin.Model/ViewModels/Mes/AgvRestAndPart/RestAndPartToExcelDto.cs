using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart
{
    public class RestAndPartToExcelDto
    {
        /// <summary>
        /// 
        /// </summary>
        [Column("分区编码（必填）")]
        public virtual string? PartCode { get; set; }

        [Column("休息点编码（必填）")]
        public virtual string? RestCode { get; set; }

        [Column("AGV 类型  6-提升AGV  10--顶升AGV（必填）")]
        public virtual int? AgvDeviceKind { get; set; }

        [Column("工艺路线编码")]
        public virtual string? RouteCode { get; set; }

        [Column("该分区可分派的休息点的优先级  1>>2>>3")]
        public virtual int? Priority { get; set; }
    }
}
