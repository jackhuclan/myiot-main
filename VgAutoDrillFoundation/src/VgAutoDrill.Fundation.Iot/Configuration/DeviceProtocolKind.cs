using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Configuration;

/// <summary>
/// 设备协议种类
/// </summary>
public enum DeviceProtocolKind
{
    /// <summary>
    /// 未知设备连接器
    /// </summary>
    [Description("未知设备连接器")]
    Unknown = 0,
    /// <summary>
    /// modubs
    /// </summary>
    [Description("modubs")]
    Modbus = 1,
    /// <summary>
    /// mc 协议
    /// </summary>
    [Description("mc 协议")]
    MC = 2,
}
