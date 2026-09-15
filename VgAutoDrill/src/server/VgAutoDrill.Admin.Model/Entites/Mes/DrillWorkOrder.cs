using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 钻孔任务
    ///</summary>
    [SugarTable("t_drill_work_order")]
    public class DrillWorkOrder : BaseEntity
    {
        /// <summary>
        /// 生产工单ID
        /// </summary>
        [SugarColumn(ColumnName = "workorder_id")]
        public virtual long? WorkOrderId { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        [SugarColumn(ColumnName = "workorder_code")]
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 产品物料ID
        /// </summary>
        [SugarColumn(ColumnName = "item_id")]
        public virtual long? ItemId { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 叠板层数
        /// </summary>
        [SugarColumn(ColumnName = "panel_count")]
        public virtual decimal? PanelCount { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnName = "quantity")]
        public virtual decimal? Quantity { get; set; }

        /// <summary>
        /// 计划叠数
        /// </summary>
        [SugarColumn(ColumnName = "wad_count")]
        public virtual decimal? WadCount { get; set; }

        /// <summary>
        /// 轴数
        /// </summary>
        [SugarColumn(ColumnName = "shaft_count")]
        public virtual decimal? ShaftCount { get; set; }

        /// <summary>
        /// 总趟数
        /// </summary>
        [SugarColumn(ColumnName = "all_passes_count")]
        public virtual decimal? AllPassesCount { get; set; }

        /// <summary>
        /// 本次要排趟数
        /// </summary>
        [SugarColumn(ColumnName = "remainder_passes_count")]
        public virtual decimal? RemainderPassesCount { get; set; }

        /// <summary>
        /// 孔数
        /// </summary>
        [SugarColumn(ColumnName = "drill_count")]
        public virtual decimal? DrillCount { get; set; }

        /// <summary>
        /// 已排趟数
        /// </summary>
        [SugarColumn(ColumnName = "scheduled_count")]
        public virtual decimal? ScheduledCount { get; set; }

        /// <summary>
        /// 本次可用叠数
        /// </summary>
        [SugarColumn(ColumnName = "usable_count")]
        public virtual decimal? UsableCount { get; set; }

        /// <summary>
        /// 单趟预计耗时
        /// </summary>
        [SugarColumn(ColumnName = "single_trip_time")]
        public virtual decimal? SingleTripTime { get; set; }

        /// <summary>
        /// 钻孔总耗时
        /// </summary>
        [SugarColumn(ColumnName = "drill_all_time")]
        public virtual decimal? DrillAllTime { get; set; }

        /// <summary>
        /// 单机趟数
        /// </summary>
        [SugarColumn(ColumnName = "single_trips")]
        public virtual decimal? SingleTrips { get; set; }

        /// <summary>
        /// 分配机台数
        /// </summary>
        [SugarColumn(ColumnName = "dispense_machines")]
        public virtual decimal? DispenseMachines { get; set; }

        /// <summary>
        /// 是否已提交 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_submited")]
        public virtual byte IsSubmited { get; set; } = 0;

        /// <summary>
        /// 提交人
        /// </summary>
        [SugarColumn(ColumnName = "submit_user")]
        public virtual int? SubmitUser { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        [SugarColumn(ColumnName = "submit_time")]
        public virtual DateTime? SubmitTime { get; set; }

        /// <summary>
        /// 是否已添加任务 
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "is_add_task")]
        public virtual byte IsAddTask { get; set; } = 0;

        /// <summary>
        /// 添加任务人
        /// </summary>
        [SugarColumn(ColumnName = "add_task_user")]
        public virtual int? AddTaskUser { get; set; }

        /// <summary>
        /// 添加任务时间
        /// </summary>
        [SugarColumn(ColumnName = "add_task_time")]
        public virtual DateTime? AddTaskTime { get; set; }

        /// <summary>
        /// 是否紧急插单
        /// </summary>
        [SugarColumn(ColumnName = "is_urgent")]
        public virtual int? IsUrgent { get; set; }
    }
}
