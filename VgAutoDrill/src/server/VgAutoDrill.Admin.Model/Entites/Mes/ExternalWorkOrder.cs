using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{

    /// <summary>
    /// 生产工单
    ///</summary>
    [SugarTable("t_external_work_order")]
    public class ExternalWorkOrder : BaseEntity
    {
        /// <summary>
        /// 工单名
        /// </summary>
        [SugarColumn(ColumnName = "name")]

        public virtual string? Name { get; set; }
        /// <summary>
        /// 工单编号
        /// </summary>
        [SugarColumn(ColumnName = "code")]

        public virtual string? Code { get; set; }
        /// <summary>
        /// 内部工单id
        /// </summary>
        [SugarColumn(ColumnName = "inner_order_id")]

        public virtual long? InnerOrderId { get; set; }
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
        /// 孔数
        /// </summary>
        [SugarColumn(ColumnName = "drill_count")]
        public virtual int? DrillCount { get; set; }

        /// <summary>
        /// 物料大类编码
        /// </summary>
        [SugarColumn(ColumnName = "product_category_code")]
        public virtual string? ProductCategoryCode { get; set; }

        /// <summary>
        /// 待排产叠数
        /// </summary>
        [SugarColumn(ColumnName = "wad_count")]
        public virtual decimal? WadCount { get; set; }
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
        /// 条码
        /// </summary>
        [SugarColumn(ColumnName = "incode_number")]
        public virtual string? IncodeNumber { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        [SugarColumn(ColumnName = "panel_length")]
        public float PanelLength { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [SugarColumn(ColumnName = "specification")]
        public virtual string? Specification { get; set; }

        /// <summary>
        /// 建议机台数
        /// </summary>
        [SugarColumn(ColumnName = "dispense_machines")]
        public virtual decimal? DispenseMachines { get; set; }

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
        /// 叠板层数
        /// </summary>
        [SugarColumn(ColumnName = "panel_count")]
        public virtual decimal? PanelCount { get; set; }
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
        /// 需求日期
        /// </summary>
        [SugarColumn(ColumnName = "request_date")]
        public DateTime? RequestDate { get; set; }
        /// <summary>
        /// 是否紧急插单
        /// </summary>
        [SugarColumn(ColumnName = "is_urgent")]
        public virtual int? IsUrgent { get; set; }
        /// <summary>
        /// 处理结果
        /// </summary>
        [SugarColumn(ColumnName = "remark")]
        public virtual string? Remark { get; set; }

        [SugarColumn(ColumnName = "device_codes")]
        public virtual string? DeviceCodes { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        [SugarColumn(ColumnName = "spec_group")]
        public virtual string? SpecGroup { get; set; }

        [SugarColumn(ColumnName = "move_in_time")]
        public virtual DateTime? MoveInTime { get; set; }

        [SugarColumn(ColumnName = "move_out_time")]
        public virtual DateTime? MoveOutTime { get; set; }

        [SugarColumn(ColumnName = "track_in_time")]
        public virtual DateTime? TrackInTime { get; set; }

        [SugarColumn(ColumnName = "track_out_time")]
        public virtual DateTime? TrackOutTime { get; set; }

        [SugarColumn(ColumnName = "sign_move_in_time")]
        public virtual DateTime? SignMoveInTime { get; set; }

        [SugarColumn(ColumnName = "sign_move_out_time")]
        public virtual DateTime? SignMoveOutTime { get; set; }

        [SugarColumn(ColumnName = "sign_track_in_time")]
        public virtual DateTime? SignTrackInTime { get; set; }

        [SugarColumn(ColumnName = "sign_track_out_time")]
        public virtual DateTime? SignTrackOutTime { get; set; }

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
        /// 层数
        /// </summary>
        [SugarColumn(ColumnName = "layer_num")]
        public virtual int? LayerNum { get; set; }

        /// <summary>
        /// 是否错误数据0/1
        /// </summary>
        [SugarColumn(ColumnName = "is_error_data")]
        public virtual int? IsErrorData { get; set; } = 0;

        /// <summary>
        /// 是否在钻孔计划仓
        /// </summary>
        [SugarColumn(ColumnName = "is_in_plan_warehouse")]
        public virtual string? IsInPlanWarehouse { get; set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        [SugarColumn(ColumnName = "is_hold")]
        public virtual string? IsHold { get; set; }

        /// <summary>
        /// 是否在钻孔计划仓备注
        /// </summary>
        [SugarColumn(ColumnName = "is_in_plan_warehouse_remark")]
        public virtual string? IsInPlanWarehouseRemark { get; set; }

        /// <summary>
        /// 板料二维码
        /// </summary>
        [SugarColumn(ColumnName = "bar_code")]
        public virtual string? BarCode { get; set; }

        /// <summary>
        /// 是否已经下发转仓命令0/1
        /// </summary>
        [SugarColumn(ColumnName = "is_send_move_info")]
        public virtual int? IsSendMoveInfo { get; set; } = 0;

        /// <summary>
        /// 物料库存数量
        /// </summary>
        [SugarColumn(ColumnName = "lot_stock_num")]
        public decimal? LotStockNum { get; set; }


        /// <summary>
        /// 工序编码(博敏需求)
        /// </summary>
        [SugarColumn(ColumnName = "process_code")]
        public string? ProcessCode { get; set; }
        /// <summary>
        /// 配刀相关内容（该工单lot 对应的总趟数）
        /// </summary>

        [SugarColumn(ColumnName = "cutter_info")]
        public string? CutterInfo { get; set; }
    }
}
