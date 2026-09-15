using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder
{
    public class WorkOrderDto : BaseDtoWithTreeDto
    {
        /// <summary>
        /// 来源类型
        /// 订单或者库存
        /// </summary>

        public virtual string? OrderSource { get; set; }

        /// <summary>
        /// 来源单据
        /// </summary>

        public virtual string? SourceCode { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>

        public int? ItemId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 物料产品类型Id
        /// </summary>
        public virtual long? ItemTypeId { get; set; }

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
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 生产数量
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 调整数量
        /// </summary>
        public decimal? QuantityChanged { get; set; }

        /// <summary>
        /// 已生产数量
        /// </summary>
        public decimal? QuantityProduced { get; set; }

        /// <summary>
        /// 已排产数量
        /// </summary>
        public decimal? QuantityScheduled { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public int? ClientId { get; set; }

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
        /// 生产工单的状态(DRAFT/COMMITED/SCHEDULED/FINISH)
        /// </summary>
        public virtual string? WorkOrderStatus { get; set; }
        /// <summary>
        /// 生产工单的状态(DRAFT/COMMITED/SCHEDULED/BEGIN/FINISH)
        /// </summary>
        public virtual ManuOrderStatusEnum? ManuOrderStatus { get; set; }

        /// <summary>
        /// 叠板层数
        /// </summary>
        public virtual int? PanelCount { get; set; }

        /// <summary>
        /// 待排产叠数
        /// </summary>
        public virtual decimal? WadCount { get; set; }

        /// <summary>
        /// 孔数
        /// </summary>
        public virtual decimal? DrillCount { get; set; }

        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public virtual long? RouteId { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }

        public virtual string? Route
        {
            get
            {
                return $"{RouteCode} {RouteName}";
            }
        }

        /// <summary>
        /// 建议机台数
        /// </summary>
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 是否增加工单任务
        /// </summary>
        public byte? IsAddWorkOrder { get; set; }

        /// <summary>
        /// 钻孔任务默认轴数
        /// </summary>
        public virtual int DrillTaskShaftCount { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int IsUrgent { get; set; }

        /// <summary>
        /// 标记颜色
        /// </summary>
        public virtual string? RemarkColor { get; set; }

        public virtual DateTime? MoveInTime { get; set; }

        public virtual DateTime? MoveOutTime { get; set; }

        public virtual DateTime? TrackInTime { get; set; }

        public virtual DateTime? TrackOutTime { get; set; }

        /// <summary>
        /// 工序组
        /// </summary>
        public virtual string? SpecGroup { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public virtual string? IncodeNumber { get; set; }

        /// <summary>
        /// 转换前文件路径
        /// </summary>
        public virtual string? BeforeDrillFilePath { get; set; }

        /// <summary>
        /// 转换后文件路径
        /// </summary>
        public virtual string? AfterDrillFilePath { get; set; }

        /// <summary>
        /// 是否需要重新刷数据
        /// </summary>
        public virtual int? IsRebrush { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        public virtual int? LayerNum { get; set; }

        /// <summary>
        /// 是否外部工单导入0/1
        /// </summary>
        public virtual int? IsExternal { get; set; }

        /// <summary>
        /// 板料二维码
        /// </summary>
        public virtual string? BarCode { get; set; }

        /// <summary>
        /// 工序编码(博敏需求)
        /// </summary>
        public string? ProcessCode { get; set; }
    }
}
