using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 料仓状态定义
/// </summary>
public enum SiloStatus
{
    /// <summary>
    /// 初始状态
    /// </summary>
    [Description("初始状态")]
    None = -1,
    /// <summary>
    /// 手动
    /// </summary>
    [Description("手动")]
    Manual = 0,
    /// <summary>
    /// 就绪
    /// </summary>
    [Description("就绪")]
    Ready = 1,
    /// <summary>
    /// 自动
    /// </summary>
    [Description("自动")]
    Auto = 2,
}
