namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder
{
    public class AddOrUpdateDrillWorkOrderReq : BaseAddOrUpdateDto
    {
        /// <summary>
        /// 生产工单ID
        /// </summary>
        public virtual long? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 产品物料ID
        /// </summary>
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 叠板层数
        /// </summary>
        public virtual decimal? PanelCount { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal? Quantity { get; set; }

        /// <summary>
        /// 计划叠数
        /// </summary>
        public virtual decimal? WadCount { get; set; }

        /// <summary>
        /// 轴数
        /// </summary>
        public virtual decimal? ShaftCount { get; set; }

        /// <summary>
        /// 总趟数
        /// </summary>
        public virtual decimal? AllPassesCount { get; set; }

        /// <summary>
        /// 本次要排趟数
        /// </summary>
        public virtual decimal? RemainderPassesCount { get; set; }

        /// <summary>
        /// 孔数
        /// </summary>
        public virtual decimal? DrillCount { get; set; }

        /// <summary>
        /// 已排趟数
        /// </summary>
        public virtual decimal? ScheduledCount { get; set; }

        /// <summary>
        /// 本次可用叠数
        /// </summary>
        public virtual decimal? UsableCount { get; set; }

        /// <summary>
        /// 单趟预计耗时
        /// </summary>
        public virtual decimal? SingleTripTime { get; set; }

        /// <summary>
        /// 钻孔总耗时
        /// </summary>
        public virtual decimal? DrillAllTime { get; set; }

        /// <summary>
        /// 单机趟数
        /// </summary>
        public virtual decimal? SingleTrips { get; set; }

        /// <summary>
        /// 分配机台数
        /// </summary>
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 是否已提交 
        /// 默认值: 0
        ///</summary>
        public virtual byte IsSubmited { get; set; }

        /// <summary>
        /// 提交人
        /// </summary>
        public virtual int? SubmitUser { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public virtual DateTime? SubmitTime { get; set; }

        /// <summary>
        /// 是否已添加任务 
        /// 默认值: 0
        ///</summary>
        public virtual byte IsAddTask { get; set; } = 0;

        /// <summary>
        /// 添加任务人
        /// </summary>
        public virtual int? AddTaskUser { get; set; }

        /// <summary>
        /// 添加任务时间
        /// </summary>
        public virtual DateTime? AddTaskTime { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        public virtual int? IsUrgent { get; set; }

        /// <summary>
        /// 需求日期
        /// </summary>
        public virtual DateTime? RequestDate { get; set; }

        /// <summary>
        /// 机台编码列表
        /// </summary>
        public virtual List<string>? DeviceCodes { get; set; }
    }
}
