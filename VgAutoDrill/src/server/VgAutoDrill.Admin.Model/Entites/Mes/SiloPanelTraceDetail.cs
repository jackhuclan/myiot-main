using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 料仓板料追溯详细表
    /// </summary>
    [SugarTable("t_silo_panel_trace_detail")]
    public class SiloPanelTraceDetail : BaseEntity
    {
        /// <summary>
        /// 主表ID（关联t_silo_panel_trace表的id字段）
        /// </summary>
        [SugarColumn(ColumnName = "master_id", IsNullable = false)]
        public virtual long MasterId { get; set; }

        /// <summary>
        /// 位置编码
        /// </summary>
        [SugarColumn(ColumnName = "location_code", Length = 100, IsNullable = true)]
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 料仓编码
        /// </summary>
        [SugarColumn(ColumnName = "silo_code", Length = 100, IsNullable = true)]
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code", Length = 100, IsNullable = true)]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 板料编码
        /// </summary>
        [SugarColumn(ColumnName = "panel_code", Length = 100, IsNullable = true)]
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [SugarColumn(ColumnName = "floor_num", IsNullable = true)]
        public virtual int? FloorNum { get; set; }

        /// <summary>
        /// 板料类型 30100-生料 40100-熟料
        /// </summary>
        [SugarColumn(ColumnName = "product_status", IsNullable = false)]
        public virtual ProductStatus ProductStatus { get; set; } = ProductStatus.EmptySiloBox;

        /// <summary>
        /// 每叠块数
        /// </summary>
        [SugarColumn(ColumnName = "pcs", IsNullable = true)]
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 板宽
        /// </summary>
        [SugarColumn(ColumnName = "panel_width", IsNullable = true)]
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 板料长度
        /// </summary>
        [SugarColumn(ColumnName = "panel_length", IsNullable = true)]
        public virtual decimal? PanelLength { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        [SugarColumn(ColumnName = "pin_offset", IsNullable = true)]
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
