using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot;

public enum DeviceStatus
{
    /// <summary>
    /// 未知状态
    /// </summary>
    [Description("未知状态")]
    Unknown = -1,

    /// <summary>
    /// 在线
    /// </summary>
    [Description("在线")]
    Online = 0,

    /// <summary>
    /// 已离线
    /// </summary>
    [Description("已离线")]
    Offline = 1,

    /// <summary>
    /// 待机
    /// </summary>
    [Description("待机")]
    Ready = 2,

    /// <summary>
    /// 工作中
    /// </summary>
    [Description("工作中")]
    Working = 3,

    /// <summary>
    /// 故障
    /// </summary>
    [Description("故障")]
    Exception = 4,

    /// <summary>
    /// 低电量
    /// </summary>
    [Description("低电量")]
    LowBattery = 5,

    /// <summary>
    /// 充电中
    /// </summary>
    [Description("充电中")]
    Charging = 6,

    /// <summary>
    /// 设备维护
    /// 设置此状态后，设备将停机维护，不再配发新任务
    /// </summary>
    [Description("设备维护,设置此状态后，设备将停机维护，不再配发新任务")]
    Maintenance = 7,

    /// <summary>
    /// 停机
    /// </summary>
    [Description("停机")]
    Shutdown = 8,
}
