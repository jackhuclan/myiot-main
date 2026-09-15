namespace VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv
{
    /// <summary>
    /// 任务执行结果
    /// </summary>
    public class JobResult
    {
        /// <summary>
        /// 任务状态
        /// </summary>
        public JobState State { get; set; }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string? ErrorMessage { get; set; }
        /// <summary>
        /// 任务流程信息
        /// </summary>
        public Dictionary<string, object>? JobRecord { get; set; }
    }
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum JobState
    {
        /// <summary>
        /// 创建成功，等待运行
        /// </summary>
        Created,
        /// <summary>
        /// 执行中
        /// </summary>
        Running,
        /// <summary>
        /// 任务执行完毕
        /// </summary>
        Complete,
        /// <summary>
        /// 异常
        /// </summary>
        Error
    }

}
