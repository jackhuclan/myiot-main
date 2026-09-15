using System.ComponentModel;

namespace VgAutoDrill.Admin.Model.CentralModels;

/// <summary>
/// 稼动率因素
/// </summary>
public enum DrillRateFactorReason
{
    /// <summary>
    /// 未知原因
    /// </summary>
    [Description("未知原因")]
    Unknown = 0,

    /// <summary>
    /// 无生产任务
    /// </summary>
    [Description("无生产任务")]
    WithoutPendingWorkOrders = 1,

    /// <summary>
    /// 清洗夹头
    /// </summary>
    [Description("清洗夹头")]
    DrillCollectClear = 2,

    /// <summary>
    /// 刀具寿命
    /// </summary>
    [Description("刀具寿命")]
    DrillToolLifeExpored = 3,

    /// <summary>
    /// 无板料生产
    /// </summary>
    [Description("无板料生产")]
    WithoutPendingPanel = 4,

    /// <summary>
    /// 没有加载钻带参数
    /// </summary>
    [Description("没有加载钻带参数")]
    WithoutDrillFile = 5,

    /// <summary>
    /// 钻机设备异常
    /// </summary>
    [Description("钻机设备异常")]
    DrillAlarm = 6,

    /// <summary>
    /// 程序打板运行
    /// </summary>
    [Description("程序打板运行")]
    DrillRun = 7,

    /// <summary>
    /// Buffer无生料
    /// </summary>
    [Description("Buffer无生料")]
    BufferRawChangeNoBoard = 8,

    /// <summary>
    /// Buffer有板到上板完成
    /// </summary>
    [Description("Buffer有板到上板完成")]
    BufferRawComplete = 9,

    /// <summary>
    /// 钻机有板到开始打板
    /// </summary>
    [Description("钻机有板到开始打板")]
    DrillRawExistToRun = 10,

    /// <summary>
    /// Buffer有熟料
    /// </summary>
    [Description("Buffer有熟料")]
    BufferClinkerChangeExist = 11,

    /// <summary>
    /// 设备禁用
    /// </summary>
    [Description("设备禁用")]
    DeviceDisable = 12,

    /// <summary>
    /// Buffer自动
    /// </summary>
    [Description("Buffer自动")]
    BufferAutomatic = 13,

    /// <summary>
    /// 板料agv设备异常
    /// </summary>
    [Description("板料agv设备异常")]
    PanelAgvException = 14,

    /// <summary>
    /// 板方向检测
    /// </summary>
    [Description("板方向检测")]
    DrillBoardDirection,

    /// <summary>
    /// pin检测
    /// </summary>
    [Description("pin检测")]
    DrillTestPin,

    /// <summary>
    /// 刀具检测
    /// </summary>
    [Description("刀具检测")]
    DrillToolEvaluation,

    /// <summary>
    /// Buffer手动
    /// </summary>
    [Description("Buffer手动")]
    BufferManual,

    /// <summary>
    /// 钻机调度等待分配AGV
    /// </summary>
    [Description("钻机调度等待分配AGV")]
    DrillScheduleWaitingForAGV,

    /// <summary>
    /// AGV执行上料时长
    /// </summary>
    [Description("AGV执行上料时长")]
    DrillScheduleRunningToComplete,

    /// <summary>
    /// 结束到开始时长
    /// </summary>
    [Description("结束到开始时长")]
    DrillEndToStart,

    /// <summary>
    /// 吸尘报警
    /// </summary>
    [Description("吸尘报警")]
    DrillNoVacuum,

    /// <summary>
    /// 刀具寿命报警更换
    /// </summary>
    [Description("刀具寿命报警更换")]
    ToolLifeExporedChange,

    /// <summary>
    /// 不切换料号时的换刀
    /// </summary>
    [Description("不切换料号时的换刀")]
    NoSwitchMaterialToolChange,

    /// <summary>
    /// 切换料号时的换刀
    /// </summary>
    [Description("切换料号时的换刀")]
    SwitchMaterialToolChange,

    /// <summary>
    /// PIN校正 
    /// </summary>
    [Description("PIN校正")]
    PINRevise,

    /// <summary>
    /// 检测摆幅扭力 
    /// </summary>
    [Description("检测摆幅扭力")]
    DetectSwingTorque,

    /// <summary>
    /// 压力脚更换 
    /// </summary>
    [Description("压力脚更换")]
    PressureFootChange,

    /// <summary>
    /// 多层板最小设定层数 
    /// </summary>
    [Description("多层板最小设定层数")]
    MinMultilayerBoardsValue,

    /// <summary>
    /// 两层板等待首件 
    /// </summary>
    [Description("两层板等待首件结果")]
    TwoBoardsWaitFirstResult,

    /// <summary>
    /// 多层板等待首件 
    /// </summary>
    [Description("多层板等待首件结果")]
    MultilayerBoardsWaitFirstResult,

}
