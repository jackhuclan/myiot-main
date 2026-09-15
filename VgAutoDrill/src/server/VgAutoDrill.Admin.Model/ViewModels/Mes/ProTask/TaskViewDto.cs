using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    public class TaskViewDto
    {
        /// <summary>
        /// 任务名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 任务编号
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// 生产工单名称
        /// </summary>

        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>

        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public virtual string? WorkStationName { get; set; }

        /// <summary>
        /// 工作站编号
        /// </summary>
        public virtual string? WorkStationCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>

        public virtual string? ItemName { get; set; }

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
        /// </summary>
        public virtual string? UnitOfMeasure { get; set; }

        /// <summary>
        /// 排产数量
        /// </summary>
        public virtual decimal? Quantity { get; set; }

        /// <summary>
        /// 已生产数量
        /// </summary>
        public virtual decimal? QuantityProduced { get; set; }

        /// <summary>
        /// 良品数量
        /// </summary>
        public virtual decimal? QuantityQuanlify { get; set; }

        /// <summary>
        /// 不良品数量
        /// </summary>
        public virtual decimal? QuantityUnquanlify { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string? ClientName { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public virtual string? ClientCode { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 生产时长
        /// </summary>
        public virtual int? Duration { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// 任务状态(DRAFT/COMMITED/BEGIN/FINISH)
        /// </summary>
        public virtual TaskStatusEnum? TaskStatus { get; set; }
        /// <summary>
        /// 是否关键工序(0/1)
        /// </summary>
        public virtual string? KeyFlag { get; set; }

        /// <summary>
        /// 本次排产叠数
        /// </summary>
        public virtual decimal? NowWadCount { get; set; }
        /// <summary>
        /// 叠板层数
        /// </summary>
        public decimal? PanelCount { get; set; } = 1;

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public DateTime? RealStartTime { get; set; }

        /// <summary>
        /// 实际耗时
        /// </summary>
        public int? RealDuration { get; set; }

        /// <summary>
        /// 实际结束日期
        /// </summary>
        public DateTime? RealEndTime { get; set; }

        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }

    }
}
