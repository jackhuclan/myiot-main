using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

public enum InteractionSequence
{
    [Description("")]
    None = 8,

    /// <summary>
    /// 只上
    /// </summary>
    [Description("只上")]
    LoadOnly = 0,

    /// <summary>
    /// 只下
    /// </summary>
    [Description("只下")]
    UnloadOnly = 1,

    /// <summary>
    /// 先上再下
    /// </summary>
    [Description("先上再下")]
    LoadThenUnload = 2,

    /// <summary>
    /// 先下再上
    /// </summary>
    [Description("先下再上")]
    UnloadThenLoad = 3,
}

