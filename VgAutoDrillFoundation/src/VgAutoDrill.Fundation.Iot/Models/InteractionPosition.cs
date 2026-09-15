using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// interaction position
/// </summary>
public enum InteractionPosition
{
    /// <summary>
    /// 后上料
    /// </summary>
    [Description("后上料")]
    Rear = 0,
    /// <summary>
    /// 前上料
    /// </summary>
    [Description("前上料")]
    Front = 1
}
