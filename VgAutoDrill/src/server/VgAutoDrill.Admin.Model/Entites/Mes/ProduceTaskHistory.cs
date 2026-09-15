using SqlSugar;

namespace VgAutoDrill.Admin.Model.Entites.Mes
{
    /// <summary>
    /// 生产任务历史表
    ///</summary>
    [SugarTable("t_produce_task_history")]
    public class ProduceTaskHistory : BaseEntity
    {
        /// <summary>
        /// 任务单编号
        /// </summary>
        [SugarColumn(ColumnName = "task_code")]
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        [SugarColumn(ColumnName = "work_order_code")]
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        [SugarColumn(ColumnName = "work_order_name")]
        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        [SugarColumn(ColumnName = "item_code")]
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        [SugarColumn(ColumnName = "item_name")]
        public virtual string? ItemName { get; set; }

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
        /// 任务完工状态(DRAFT/COMMITED/BEGIN/FINISH)
        /// </summary>
        [SugarColumn(ColumnName = "task_status")]
        public virtual string? TaskStatus { get; set; }

        /// <summary>
        /// 本次排产叠数
        /// </summary>
        [SugarColumn(ColumnName = "now_wad_count")]
        public decimal? NowWadCount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(ColumnName = "start_time")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(ColumnName = "end_time")]
        public DateTime? EndTime { get; set; }
    }
}
