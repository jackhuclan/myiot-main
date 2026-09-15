using SqlSugar;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    [SugarTable("t_workorder_and_panel")]
    public class WorkOrderAndPanel : BaseEntity
    {
        /// <summary>
        /// 工单
        /// </summary>
        [SugarColumn(ColumnName = "work_order_code")]
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// task
        /// </summary>
        [SugarColumn(ColumnName = "task_code")]
        public virtual string? TaskCode { get; set; }
        /// <summary>
        /// 料号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 板材编码
        /// </summary>
        [SugarColumn(ColumnName = "panel_code")]
        public virtual string? PanelCode { get; set; }

        /// <summary>
        /// 片数
        /// </summary>
        [SugarColumn(ColumnName = "pcs")]
        public virtual int? Pcs { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        [SugarColumn(ColumnName = "batch_code")]
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 产品状态
        /// </summary>
        [SugarColumn(ColumnName = "product_status")]
        public virtual ProductStatus ProductStatus { get; set; }

        /// <summary>
        /// 板料位置
        /// </summary>
        [SugarColumn(ColumnName = "board_location")]
        public virtual string? BoardLocation { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        [SugarColumn(ColumnName = "station_id")]
        public virtual int? StationId { get; set; }

        /// <summary>
        /// 板料宽度
        /// </summary>
        [SugarColumn(ColumnName = "panel_width")]
        public virtual decimal? PanelWidth { get; set; }

        /// <summary>
        /// 梢钉偏移量
        /// </summary>
        [SugarColumn(ColumnName = "pin_offset")]
        public virtual decimal? PinOffset { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        [SugarColumn(ColumnName = "panel_length")]
        public float PanelLength { get; set; }

        /// <summary>
        /// 外部工单
        /// </summary>

        [SugarColumn(ColumnName = "external_worker_order")]
        public virtual string? ExternalWorkerOrder { get; set; }
    }
}
