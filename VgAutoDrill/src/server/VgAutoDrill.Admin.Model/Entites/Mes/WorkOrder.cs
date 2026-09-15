using SqlSugar;
using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 生产工单
    ///</summary>
    [SugarTable("t_work_order")]
    public class WorkOrder : BaseEntityWithTree
    {
        /// <summary>
        /// 来源类型
        /// 订单或者库存
        /// </summary>
        [SugarColumn(ColumnName = "order_source")]

        public virtual string? OrderSource { get; set; }

        /// <summary>
        /// 来源单据
        /// </summary>
        [SugarColumn(ColumnName = "source_code")]

        public virtual string? SourceCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [SugarColumn(ColumnName = "item_id")]
        public int? ItemId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]

        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]

        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        [SugarColumn(ColumnName = "item_type_id")]
        public virtual long? ItemTypeId { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [SugarColumn(ColumnName = "batch_code")]

        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        [SugarColumn(ColumnName = "specification")]

        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [SugarColumn(ColumnName = "unit_of_measure")]

        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity")]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 调整数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_changed")]
        public decimal? QuantityChanged { get; set; }

        /// <summary>
        /// 已生产数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_produced")]
        public decimal? QuantityProduced { get; set; }

        /// <summary>
        /// 已排产数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_scheduled")]
        public decimal? QuantityScheduled { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        [SugarColumn(ColumnName = "client_id")]
        public int? ClientId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [SugarColumn(ColumnName = "client_name")]
        public virtual string? ClientName { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        [SugarColumn(ColumnName = "client_code")]
        public virtual string? ClientCode { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        [SugarColumn(ColumnName = "request_date")]
        public DateTime? RequestDate { get; set; }
        /// <summary>
        /// 是否增加工单任务
        /// </summary>
        [SugarColumn(ColumnName = "is_add_work_order")]
        public byte? IsAddWorkOrder { get; set; } = 0;
        /// <summary>
        /// 增加工单时间
        /// </summary>
        [SugarColumn(ColumnName = "add_work_order_time")]
        public DateTime? AddWorkOrderTime { get; set; }

        /// <summary>
        /// 增加工单任务人id
        /// </summary>
        [SugarColumn(ColumnName = "add_work_order_id")]
        public int? AddWorkOrderId { get; set; }

        /// <summary>
        /// 生产工单的状态(DRAFT/COMMITED/SCHEDULED/BEGIN/FINISH)
        /// </summary>
        [SugarColumn(ColumnName = "manu_order_status")]
        public virtual ManuOrderStatusEnum? ManuOrderStatus { get; set; } = ManuOrderStatusEnum.DRAFT;
        /// <summary>
        /// 叠板层数
        /// </summary>
        [SugarColumn(ColumnName = "panel_count")]
        public virtual int? PanelCount { get; set; }

        /// <summary>
        /// 待排产叠数
        /// </summary>
        [SugarColumn(ColumnName = "wad_count")]
        public virtual decimal? WadCount { get; set; }

        /// <summary>
        /// 孔数
        /// </summary>
        [SugarColumn(ColumnName = "drill_count")]
        public virtual decimal? DrillCount { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        [SugarColumn(ColumnName = "route_id")]
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        [SugarColumn(ColumnName = "route_code")]
        public virtual string? RouteCode { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [SugarColumn(ColumnName = "route_name")]
        public virtual string? RouteName { get; set; }

        /// <summary>
        /// 建议机台数
        /// </summary>
        [SugarColumn(ColumnName = "dispense_machines")]
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        [SugarColumn(ColumnName = "is_Urgent")]
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        [SugarColumn(ColumnName = "incode_number")]
        public virtual string? IncodeNumber { get; set; }

        /// <summary>
        /// 标记颜色
        /// </summary>
        [SugarColumn(ColumnName = "remark_color")]
        public virtual string? RemarkColor { get; set; }

        [SugarColumn(ColumnName = "move_in_time")]
        public virtual DateTime? MoveInTime { get; set; }

        [SugarColumn(ColumnName = "move_out_time")]
        public virtual DateTime? MoveOutTime { get; set; }

        [SugarColumn(ColumnName = "track_in_time")]
        public virtual DateTime? TrackInTime { get; set; }

        [SugarColumn(ColumnName = "track_out_time")]
        public virtual DateTime? TrackOutTime { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        [SugarColumn(ColumnName = "spec_group")]
        public virtual string? SpecGroup { get; set; }

        /// <summary>
        /// 转换前文件路径
        /// </summary>
        [SugarColumn(ColumnName = "before_drill_file_path")]
        public virtual string? BeforeDrillFilePath { get; set; }

        /// <summary>
        /// 转换后文件路径
        /// </summary>
        [SugarColumn(ColumnName = "after_drill_file_path")]
        public virtual string? AfterDrillFilePath { get; set; }

        /// <summary>
        /// 是否需要重新刷数据0/1
        /// </summary>
        [SugarColumn(ColumnName = "is_rebrush")]
        public virtual int? IsRebrush { get; set; } = 0;

        /// <summary>
        /// 层数
        /// </summary>
        [SugarColumn(ColumnName = "layer_num")]
        public virtual int? LayerNum { get; set; }

        /// <summary>
        /// 是否外部工单导入0/1
        /// </summary>
        [SugarColumn(ColumnName = "is_external")]
        public virtual int? IsExternal { get; set; } = 0;

        /// <summary>
        /// 板料二维码
        /// </summary>
        [SugarColumn(ColumnName = "bar_code")]
        public virtual string? BarCode { get; set; }

        /// <summary>
        /// 工序编码(博敏需求)
        /// </summary>
        [SugarColumn(ColumnName = "process_code")]
        public string? ProcessCode { get; set; }

        /// <summary>
        /// 配刀相关内容
        /// </summary>
        [SugarColumn(ColumnName = "cutter_info")]
        public string? CutterInfo { get; set; }
    }
}
