using Npoi.Mapper.Attributes;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem
{
    public class ItemToExcelDto
    {
        [Column("板料长度")]
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelLength { get; set; }
        [Column("条码")]
        /// <summary>
        /// 条码
        /// </summary>
        public string IncodeNumber { get; set; }
        [Column("物料编码（必填）")]
        public string Code { get; set; }

        [Column("物料名称（必填）")]
        public string Name { get; set; }

        [Column("父对象Id(0表示是根对象)（必填）")]
        public long ParentID { get; set; }

        [Column("物料1产品2")]
        public int ItemOrProduct { get; set; }

        [Column("规格型号")]
        public string Specification { get; set; }

        [Column("单位")]
        public string UnitOfMeasure { get; set; }

        [Column("物料产品类型Id（必填）")]
        public int ItemTypeId { get; set; }

        [Column("产品大类ID")]
        public int ProductCategoryId { get; set; }

        [Column("产品大类编码")]
        public string ProductCategoryCode { get; set; }

        [Column("产品大类名称")]
        public string ProductCategoryName { get; set; }

        [Column("库房ID")]
        public int WarehouseId { get; set; }

        [Column("库房编码")]
        public string WarehouseCode { get; set; }

        [Column("库房名称")]
        public string WarehouseName { get; set; }
        [Column("板料宽度")]
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelWidth { get; set; }
    }
}
