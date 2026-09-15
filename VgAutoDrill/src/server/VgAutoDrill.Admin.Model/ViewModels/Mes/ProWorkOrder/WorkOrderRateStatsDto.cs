namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder
{
    /// <summary>
    /// 工单统计
    /// </summary>
    public class WorkOrderRateStatsDto
    {
        /// <summary>
        /// 生产增率
        /// </summary>
        public virtual string FeedBackAddRate { get; set; } = "0";

        /// <summary>
        /// 工单增率
        /// </summary>
        public virtual string WorkOrderAddRate { get; set; } = "0";
    }
}
