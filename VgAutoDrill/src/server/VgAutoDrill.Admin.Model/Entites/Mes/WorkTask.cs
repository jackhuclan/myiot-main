using SqlSugar;
using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 生产任务
    ///</summary>
    [SugarTable("t_task")]
    public class WorkTask : BaseEntityWithTree
    {
        /// <summary>
        /// 生产工单ID
        /// </summary>
        [SugarColumn(ColumnName = "work_order_id")]
        public int? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        [SugarColumn(ColumnName = "work_order_name")]

        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        [SugarColumn(ColumnName = "work_order_code")]

        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工作站Id
        /// </summary>
        [SugarColumn(ColumnName = "work_station_id")]
        public int? WorkStationId { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        [SugarColumn(ColumnName = "work_station_name")]

        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        [SugarColumn(ColumnName = "work_station_code")]

        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        [SugarColumn(ColumnName = "process_id")]
        public int? ProcessId { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        [SugarColumn(ColumnName = "process_name")]

        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        [SugarColumn(ColumnName = "process_code")]

        public virtual string? ProcessCode { get; set; }

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
        /// 叠板层数
        /// </summary>
        [SugarColumn(ColumnName = "panel_count")]
        public decimal? PanelCount { get; set; } = 1;
        /// <summary>
        /// 排产数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity")]
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 已生产数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_produced")]
        public decimal? QuantityProduced { get; set; }

        /// <summary>
        /// 良品数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_quanlify")]
        public decimal? QuantityQuanlify { get; set; }

        /// <summary>
        /// 不良品数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity_unquanlify")]
        public decimal? QuantityUnquanlify { get; set; }

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
        /// 开始日期
        /// </summary>
        [SugarColumn(ColumnName = "start_time")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 生产时长
        /// </summary>
        [SugarColumn(ColumnName = "duration")]
        public int? Duration { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        [SugarColumn(ColumnName = "end_time")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        [SugarColumn(ColumnName = "request_date")]
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// 完工状态(DRAFT/COMMITED/BEGIN/FINISH)
        /// </summary>
        [SugarColumn(ColumnName = "task_status")]
        public virtual TaskStatusEnum? TaskStatus { get; set; } = TaskStatusEnum.DRAFT;

        /// <summary>
        /// TASK的颜色
        /// </summary>
        [SugarColumn(ColumnName = "color")]
        public virtual string? Color { get; set; }

        /// <summary>
        /// 是否关键工序(0/1)
        /// </summary>
        [SugarColumn(ColumnName = "key_flag")]
        public virtual string? KeyFlag { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        [SugarColumn(ColumnName = "real_start_time")]
        public DateTime? RealStartTime { get; set; }
        /// <summary>
        /// 实际耗时
        /// </summary>
        [SugarColumn(ColumnName = "real_duration")]
        public int? RealDuration { get; set; }
        /// <summary>
        /// 实际结束日期
        /// </summary>
        [SugarColumn(ColumnName = "real_end_time")]
        public DateTime? RealEndTime { get; set; }
        /// <summary>
        /// 是否开启 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_started")]
        public byte? IsStarted { get; set; }
        /// <summary>
        /// 本次排产叠数
        /// </summary>
        [SugarColumn(ColumnName = "now_wad_count")]
        public decimal? NowWadCount { get; set; }

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
        /// 是否紧急插单
        /// </summary>
        [SugarColumn(ColumnName = "is_urgent")]
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        [SugarColumn(ColumnName = "incode_number")]
        public virtual string? IncodeNumber { get; set; }

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
        /// 板料二维码
        /// </summary>
        [SugarColumn(ColumnName = "bar_code")]
        public virtual string? BarCode { get; set; }


        /// <summary>
        /// 配刀组计划No
        /// </summary>
        [SugarColumn(ColumnName = "cutter_group_no")]
        public virtual string? CutterGroupNo { get; set; }

        /// <summary>
        /// 钻孔参数文件地址
        /// </summary>
        [SugarColumn(ColumnName = "dia_file_path")]
        public virtual string? DiaFilePath { get; set; }

        /// <summary>
        /// 钻孔程序文件路径
        /// </summary>
        [SugarColumn(ColumnName = "program_file_path")]
        public virtual string? ProgramFilePath { get; set; }

        /// <summary>
        /// 文件ftp的host地址
        /// </summary>
        [SugarColumn(ColumnName = "ftp_host")]
        public virtual string? FtpHost { get; set; } = string.Empty;

        /// <summary>
        /// 文件ftp的host地址端口
        /// </summary>
        [SugarColumn(ColumnName = "ftp_port")]
        public virtual string? FtpPort { get; set; } = string.Empty;

        /// <summary>
        /// 文件ftp地址的用户名
        /// </summary>
        [SugarColumn(ColumnName = "ftp_username")]
        public virtual string? FtpUsername { get; set; } = string.Empty;

        /// <summary>
        /// 文件ftp地址的密码
        /// </summary>
        [SugarColumn(ColumnName = "ftp_password")]
        public virtual string? FtpPassword { get; set; } = string.Empty;





    }
}
