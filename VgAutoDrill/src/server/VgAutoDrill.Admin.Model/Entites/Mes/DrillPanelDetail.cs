using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 钻机载料明细
    ///</summary>
    [SugarTable("t_drill_panel_detail")]
    public class DrillPanelDetail : BaseEntity
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }
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
        /// 该层板料类型:（38000-等待钻孔, 39000-正在钻孔,40000-钻机完成加工）
        /// </summary>
        [SugarColumn(ColumnName = "product_status")]
        public virtual ProductStatus? ProductStatus { get; set; }
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
        /// 0生料仓, 1钻机,2熟料仓
        /// </summary>
        [SugarColumn(ColumnName = "layer")]
        public virtual int Layer { get; set; } = 1;

        /// <summary>
        /// 轴
        /// </summary>
        [SugarColumn(ColumnName = "splindle_index")]
        public virtual int? SplindleIndex { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [SugarColumn(ColumnName = "batch_code")]
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        [SugarColumn(ColumnName = "pin_offset")]
        public virtual decimal? PinOffset { get; set; }
    }
}
