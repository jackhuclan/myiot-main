using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Domain.Domain
{
    public class GanttData
    {
        public string? Id { get; set; }

        /// <summary>
        /// TASK 类型：workOrder；task
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// 工作站名称
        /// </summary>
        public string? WorkStation { get; set; }

        /// <summary>
        /// 产品或者MPS项目
        /// </summary>
        public string? Product { get; set; }

        /// <summary>
        /// 排产数量
        /// </summary>
        public decimal? Quantity { get; set; }

        /// <summary>
        /// 已生产数量
        /// </summary>
        public decimal? QuantityProduced { get; set; }

        /// <summary>
        /// 生产进度
        /// </summary>
        public decimal? Progress { get; set; }

        /// <summary>
        /// TASK的颜色
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string? Process { get; set; }

        /// <summary>
        /// 父TASK ID
        /// </summary>
        public string? Parent { get; set; }

        /// <summary>
        /// 开始生产时间
        /// </summary>
        public string? start_date { get; set; }

        /// <summary>
        /// 生产时长
        /// </summary>
        public long? Duration { get; set; }

        /** 完成生产时间 */
        public string? end_date { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public virtual TaskStatusEnum? TaskStatus { get; set; }
    }
}
