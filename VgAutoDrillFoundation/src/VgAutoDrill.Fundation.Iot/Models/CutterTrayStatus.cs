using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 刀盘状态
/// </summary>
public enum CutterTrayStatus
{
    /// <summary>
    /// 空操作
    /// </summary>
    [Description("空操作")]
    Noop = -1,
    /// <summary>
    /// 空负载
    /// </summary>
    [Description("空负载")]
    NoTray = 0,
    /// <summary>
    /// 空料盒
    /// </summary>
    [Description("空料盒")]
    EmptyTray = 1,
    /// <summary>
    /// 新刀
    /// </summary>
    [Description("新刀")]
    New = 100,
    /// <summary>
    /// 旧刀
    /// </summary>
    [Description("旧刀")]
    Old = 200
}
