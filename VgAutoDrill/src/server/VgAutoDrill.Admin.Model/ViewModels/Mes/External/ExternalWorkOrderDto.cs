using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class ExternalWorkOrderDto : BaseDto
    {
        /// <summary>
        /// 工单名
        /// </summary>
        public virtual string? Name { get; set; }
        /// <summary>
        /// 工单编号
        /// </summary>
        public virtual string? Code { get; set; }
        public virtual long? InnerOrderId { get; set; }
        /// <summary>
        /// 工单来源
        /// </summary>
        public virtual string? OrderSource { get; set; }

        /// <summary>
        /// 来源单据
        /// </summary>
        public virtual string? SourceCode { get; set; }
        /// <summary>
        /// 产品名称
        /// 无值时，用ItemCode填写
        /// </summary>
        public virtual string? ItemName { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public virtual string? IncodeNumber { get; set; }
        /// <summary>
        /// 板料长度
        /// </summary>
        public float PanelLength { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string? ItemCode { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public virtual string? BatchCode { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string? Specification { get; set; }

        /// <summary>
        /// 单位
        /// 未给值时，使用预存的系统单位 code==Panel
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 生产数量,pcs 片数
        /// 使用此字段 填写内部工单表的QuantityChanged
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 待排产叠数，
        /// 
        /// </summary>
        public virtual decimal? WadCount { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string? ClientName { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public virtual string? ClientCode { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// 叠板层数，
        /// 板料的默认层数，实际个别Panel的层数可能小于这个值
        /// </summary>
        public virtual decimal? PanelCount { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public virtual int? LayerNum { get; set; }

        /// <summary>
        /// 孔数，
        /// 用于预估任务的耗时，无值时按10000计算
        /// </summary>
        public virtual decimal? DrillCount { get; set; }

        /// <summary>
        /// 产品大类code，
        /// P01,或者其他
        /// </summary>
        public virtual string? ProductCategoryCode { get; set; }

        /// <summary>
        /// 工艺路线编号，
        /// A0001,或者其他
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }

        /// <summary>
        /// 建议机台数
        /// </summary>
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 钻机的设备代号
        /// </summary>
        public virtual string? DeviceCodes { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        public virtual int? Status { get; set; }
        /// <summary>
        /// 处理结果描述
        /// </summary>
        public virtual string? Remark { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        public virtual string? SpecGroup { get; set; }

        /// <summary>
        /// 板料信息
        /// </summary>
        public List<ExternalPanelDto> Panels { get; set; } = new List<ExternalPanelDto>();

        public virtual DateTime? MoveInTime { get; set; }

        public virtual DateTime? MoveOutTime { get; set; }

        public virtual DateTime? TrackInTime { get; set; }

        public virtual DateTime? TrackOutTime { get; set; }

        public virtual DateTime? SignMoveInTime { get; set; }

        public virtual DateTime? SignMoveOutTime { get; set; }

        public virtual DateTime? SignTrackInTime { get; set; }

        public virtual DateTime? SignTrackOutTime { get; set; }

        /// <summary>
        /// 转换前文件路径
        /// </summary>
        public virtual string? BeforeDrillFilePath { get; set; }

        /// <summary>
        /// 转换后文件路径
        /// </summary>
        public virtual string? AfterDrillFilePath { get; set; }

        /// <summary>
        /// 是否在钻孔计划仓
        /// </summary>
        public virtual string? IsInPlanWarehouse { get; set; }

        /// <summary>
        /// 是否暂停
        /// </summary>
        public virtual string? IsHold { get; set; }
        /// <summary>
        /// 是否在钻孔计划仓备注
        /// </summary>
        public virtual string? IsInPlanWarehouseRemark { get; set; }

        /// <summary>
        /// 是否已经下发转仓命令0/1
        /// </summary>
        public virtual int? IsSendMoveInfo { get; set; }

        /// <summary>
        /// 板料二维码
        /// </summary>
        public virtual string? BarCode { get; set; }

        /// <summary>
        /// Hold是否可用
        /// </summary>
        public virtual bool KwHoldEnable { get; set; } = false;

        /// <summary>
        /// Mes转仓是否可用
        /// </summary>
        public virtual bool KwAGVStockInEnable { get; set; } = false;
        /// <summary>
        /// 物料库存数量
        /// </summary>
        public decimal? LotStockNum { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcessCode { get; set; }

        /// <summary>
        /// 配刀相关内容（该工单lot 对应的总趟数）
        /// </summary>
        public string? CutterInfo { get; set; }

        /// <summary>
        /// 内部工单状态
        /// </summary>
        public virtual ManuOrderStatusEnum? InnerOrderStatus { get; set; }

        public int? IsErrorData { get; set; }
        
    }
}
