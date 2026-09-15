using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

public enum PanelKind
{
    /// <summary>
    /// 未指定
    /// </summary>
    [Description("未指定")]
    Unspecified = 0,

    /// <summary>
    /// 生料
    /// </summary>
    [Description("生料")]
    Undrilled = 1,

    /// <summary>
    /// 熟料
    /// </summary>
    [Description("熟料")]
    Drilled = 2,

    /// <summary>
    /// 首件
    /// </summary>
    [Description("首件")]
    FirstDrilled = 3,
}
