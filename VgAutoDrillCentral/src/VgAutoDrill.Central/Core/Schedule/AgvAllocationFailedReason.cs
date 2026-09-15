namespace VgAutoDrill.Central.Core.Schedule;

public enum AgvAllocationFailedReason
{
    /// <summary>
    /// 无效状态
    /// </summary>
    Invalid = -200,
    /// <summary>
    /// 主叫设备离线
    /// </summary>
    CallerOffline = -100,
    /// <summary>
    /// 中控系统维护
    /// </summary>
    CentralSystemMaintain = -50,
    /// <summary>
    /// 转换调度申请的RequestJson时失败
    /// </summary>
    FailedToConvertRequestJson = -40,
    /// <summary>
    /// 调度任务已取消
    /// </summary>
    Cancelled = -1,
    /// <summary>
    /// 等待更高优先级的调度任务
    /// </summary>
    WaitingHigherSchedule = 0,
    /// <summary>
    /// 无可用AGV
    /// </summary>
    NoAvailableAGV = 1,
    /// <summary>
    /// 可调度状态
    /// </summary>
    Schedulable = 2,
    /// <summary>
    /// 已调度状态
    /// </summary>
    Completed = 3,
    /// <summary>
    /// 调度失败
    /// </summary>
    Failed = 4
}
