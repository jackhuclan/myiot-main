using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("t_device_panel")]
    public class DevicePanel : BaseEntity
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [SugarColumn(ColumnName = "device_code")]
        public virtual string? DeviceCode { get; set; }

        /// <summary>
        /// 板料二维码
        /// </summary>
        [SugarColumn(ColumnName = "panel_code")]
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 料仓二维码
        /// </summary>
        [SugarColumn(ColumnName = "silo_code")]
        public virtual string? SiloCode { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// Lot二维码
        /// </summary>
        [SugarColumn(ColumnName = "lot_id")]
        public virtual string? LotId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [SugarColumn(ColumnName = "batch_code")]
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 第几层
        /// </summary>
        [SugarColumn(ColumnName = "layer")]
        public virtual int? Layer { get; set; }

        /// <summary>
        /// 位置ID
        /// </summary>
        [SugarColumn(ColumnName = "position")]
        public virtual int? Position { get; set; }

        /// <summary>
        /// 成品状态
        /// </summary>
        [SugarColumn(ColumnName = "product_status")]
        public virtual ProductStatus? ProductStatus { get; set; }
        /// <summary>
        /// Panel的钻孔状态
        /// </summary>
        [SugarColumn(ColumnName = "drill_state")]
        public virtual int? DrillState { get; set; }
        /// <summary>
        /// 板宽
        /// </summary>
        [SugarColumn(ColumnName = "panel_width")]
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        [SugarColumn(ColumnName = "pin_offset")]
        public virtual decimal? PinOffset { get; set; }

        [SugarColumn(ColumnName = "location_code")]
        public virtual string? LocationCode { get; set; }
    }
}
