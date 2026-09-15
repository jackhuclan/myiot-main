using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Schedule;

/// <summary>
/// 调度状态顺序：Created->WaitingForAgv->Allocated->Delivered->Running->PartCompleted->Completed
/// </summary>
public enum ScheduledTaskStatus
{
    /// <summary>
    /// 默认，非法状态
    /// </summary>
    [Description("默认，非法状态")]
    None = 0,
    /// <summary>
    /// 已上报
    /// </summary>
    [Description("已上报")]
    Created = 1,
    /// <summary>
    /// 等待分配agv
    /// </summary>
    [Description("等待分配agv")]
    WaitingForAgv = 8,
    /// <summary>
    /// 已分配AGV，等待下发
    /// </summary>
    [Description("已分配AGV，等待下发")]
    Allocated = 2,
    /// <summary>
    /// 下发成功
    /// </summary>
    [Description("下发成功")]
    Delivered = 6,
    /// <summary>
    /// 开始调度
    /// </summary>
    [Description("开始调度")]
    Running = 3,
    /// <summary>
    /// 调度完成
    /// </summary>
    [Description("调度完成")]
    Completed = 4,
    /// <summary>
    /// 部分完成
    /// 等待AGV下次上料
    /// </summary>
    [Description("部分完成,等待AGV下次上料")]
    PartCompleted = 5,
    /// <summary>
    /// 计划任务执行失败
    /// </summary>
    [Description("计划任务执行失败")]
    Failed = -1,
    /// <summary>
    /// 取消计划任务
    /// </summary>
    [Description("取消计划任务")]
    Canceled = -2,
}
