namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask
{
    /// <summary>
    /// 生产量统计
    /// </summary>
    public class TaskStatsByTime
    {
        /// <summary>
        /// 生产目标
        /// </summary>
        public virtual decimal ProductionTargets { get; set; }

        /// <summary>
        /// 实际产量
        /// </summary>
        public virtual decimal ProductionActual { get; set; }

        /// <summary>
        /// 日进度
        /// </summary>
        public virtual decimal ProgressDaily { get; set; }
    }
}
