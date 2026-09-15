namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProduceTask
{
    public class ProduceTaskDto : BaseDto
    {
        /// <summary>
        /// 任务单编号
        /// </summary>
        public virtual string? TaskCode { get; set; }

        /// <summary>
        /// 生产工单编号
        /// </summary>
        public virtual string? WorkOrderCode { get; set; }

        /// <summary>
        /// 生产工单名称
        /// </summary>
        public virtual string? WorkOrderName { get; set; }

        /// <summary>
        /// 产品物料编码
        /// </summary>
        public virtual string? ItemCode { get; set; }

        /// <summary>
        /// 产品物料名称
        /// </summary>
        public virtual string? ItemName { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public virtual string? ProcessName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public virtual string? ProcessCode { get; set; }

        /// <summary>
        /// 任务完工状态(DRAFT/COMMITED/BEGIN/FINISH)
        /// </summary>
        public virtual string? TaskStatus { get; set; }

        /// <summary>
        /// 本次排产叠数
        /// </summary>
        public decimal? NowWadCount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
