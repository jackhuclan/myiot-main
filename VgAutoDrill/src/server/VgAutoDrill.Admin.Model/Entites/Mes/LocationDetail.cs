using SqlSugar;
using System.ComponentModel.DataAnnotations;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 库位明细表
    /// </summary>
    [SugarTable("t_location_detail")]
    public class LocationDetail : BaseEntity
    {
        /// <summary>
        /// 库位编号
        /// </summary>
        [SugarColumn(ColumnName = "code", Length = 100, IsNullable = false)]
        [Required]
        public virtual string Code { get; set; } = string.Empty;

        /// <summary>
        /// 板料
        /// </summary>
        [SugarColumn(ColumnName = "panel", Length = 200, IsNullable = false)]
        public virtual string Panel { get; set; } = string.Empty;

        /// <summary>
        /// 物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code", Length = 100, IsNullable = false)]
        public virtual string ItemCode { get; set; } = string.Empty;

        /// <summary>
        /// 批次编码
        /// </summary>
        [SugarColumn(ColumnName = "batch_code", Length = 100, IsNullable = true)]
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [SugarColumn(ColumnName = "floor_num", IsNullable = true)]
        public virtual int? FloorNum { get; set; }

        /// <summary>
        /// 板料编码
        /// </summary>
        [SugarColumn(ColumnName = "panel_code", Length = 50, IsNullable = true)]
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 该层板料类型:（0-空仓, 30100-生料,40100-熟料）
        /// </summary>
        [SugarColumn(ColumnName = "product_status", IsNullable = true)]
        public virtual ProductStatus? ProductStatus { get; set; }

        /// <summary>
        /// 每叠片数
        /// </summary>
        [SugarColumn(ColumnName = "pcs", IsNullable = true)]
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 板料宽度
        /// </summary>
        [SugarColumn(ColumnName = "panel_width", DecimalDigits = 2, IsNullable = true)]
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 板料长度
        /// </summary>
        [SugarColumn(ColumnName = "panel_length", DecimalDigits = 2, IsNullable = true)]
        public virtual decimal? PanelLength { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        [SugarColumn(ColumnName = "pin_offset", DecimalDigits = 2, IsNullable = true)]
        public virtual decimal? PinOffset { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [SugarColumn(ColumnName = "creator_name", Length = 100, IsNullable = true)]
        public virtual string? CreatorName { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        [SugarColumn(ColumnName = "modifier_name", Length = 100, IsNullable = true)]
        public virtual string? ModifierName { get; set; }
    }
}
