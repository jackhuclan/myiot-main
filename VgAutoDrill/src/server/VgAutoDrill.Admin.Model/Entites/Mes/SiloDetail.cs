using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 料仓
    /// </summary>
    [SugarTable("t_silo_detail")]
    public class SiloDetail : BaseEntity
    {
        // <summary>
        /// 料仓编码
        /// </summary>
        [SugarColumn(ColumnName = "silo_code")]
        public virtual string? SiloCode { get; set; }
        /// <summary>
        /// 物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 板料编码
        /// </summary>
        [SugarColumn(ColumnName = "panel_code")]
        public virtual string? PanelCode { get; set; }
        /// <summary>
        /// 层数
        /// </summary>
        [SugarColumn(ColumnName = "floor_num")]
        public virtual int? FloorNum { get; set; }

        /// <summary>
        /// 板料类型 30100-生料 40100-熟料
        /// </summary>
        [SugarColumn(ColumnName = "product_status")]
        public virtual ProductStatus ProductStatus { get; set; } = ProductStatus.EmptySiloBox;
        /// <summary>
        /// 每叠块数
        /// </summary>
        [SugarColumn(ColumnName = "pcs")]
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 板宽
        /// </summary>
        [SugarColumn(ColumnName = "panel_width")]
        public virtual decimal? PanelWidth { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        [SugarColumn(ColumnName = "panel_length")]
        public virtual decimal? PanelLength { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        [SugarColumn(ColumnName = "pin_offset")]
        public virtual decimal? PinOffset { get; set; }
    }
}
