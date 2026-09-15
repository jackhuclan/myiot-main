using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 告警分类
/// </summary>
public enum AlarmKind
{
    [Description("")]
    Unknown,
    /// <summary>
    /// 钻机缺少生产任务排产或其他生产任务异常
    /// </summary>
    [Description("钻机缺少生产任务排产或其他生产任务异常")]
    WorkOrderTaskException = 1,
    /// <summary>
    /// 库位没有发起调度或其他库位调度异常
    /// </summary>
    [Description("库位没有发起调度或其他库位调度异常")]
    LoationScheduleException = 2,
    /// <summary>
    /// 代理异常
    /// </summary>
    [Description("代理异常")]
    AgentException = 3,
    /// <summary>
    /// mqtt异常
    /// </summary>
    [Description("mqtt异常")]
    MqttException = 4,
    /// <summary>
    /// http异常
    /// </summary>
    [Description("http异常")]
    HttpException = 5,
    /// <summary>
    /// agv底盘车故障
    /// </summary>
    [Description("agv底盘车故障")]
    AgvChassisException = 6,
    /// <summary>
    /// 钻机故障
    /// </summary>
    [Description("钻机故障")]
    DrillException = 7,
    /// <summary>
    /// 钻机buffer故障
    /// </summary>
    [Description("钻机buffer故障")]
    DrillBufferException = 8,
    /// <summary>
    /// 中控分配agv异常
    /// </summary>
    [Description("中控分配agv异常")]
    AgvAllocationException = 9,
}
