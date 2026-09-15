using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 
    ///</summary>
    [SugarTable("t_schedule_location_panel")]
    public class ScheduleLocationPanel : BaseEntity
    {
        /// <summary>
        /// 外键：Schedule表id
        /// </summary>
        [SugarColumn(ColumnName = "schedule_id")]
        public long? ScheduleId { get; set; }

        /// <summary>
        /// 板料编号
        /// </summary>
        [SugarColumn(ColumnName = "panel_code")]
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 物料代码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [SugarColumn(ColumnName = "batch_code")]
        public virtual string? BatchCode { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        [SugarColumn(ColumnName = "panel_length")]
        public float PanelLength { get; set; }
        /// <summary>
        /// 板料位置
        /// </summary>
        [SugarColumn(ColumnName = "board_location")]
        public virtual string? BoardLocation { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        [SugarColumn(ColumnName = "product_status")]
        public virtual ProductStatus ProductStatus { get; set; }

        /// <summary>
        /// 单叠数量
        /// </summary>
        [SugarColumn(ColumnName = "pcs")]
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 工位ID
        /// </summary>
        [SugarColumn(ColumnName = "station_id")]
        public virtual long? StationId { get; set; }

        /// <summary>
        ///父类ID
        /// </summary>
        [SugarColumn(ColumnName = "parent_id")]
        public virtual long? ParentId { get; set; }
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

        /// <summary>
        /// 库位号
        /// </summary>
        [SugarColumn(ColumnName = "location_code")]
        public virtual string? LocationCode { get; set; }

        /// <summary>
        /// 料仓号
        /// </summary>
        [SugarColumn(ColumnName = "silo_code")]
        public virtual string? SiloCode { get; set; }
    }
}
