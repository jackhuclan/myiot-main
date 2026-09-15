using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 物料种类
/// </summary>
public enum MaterialKind
{
    /// <summary>
    /// 未指定
    /// </summary>
    [Description("未指定")]
    Unspecified = 0,

    /// <summary>
    /// 板料
    /// </summary>
    [Description("板料")]
    Panel = 1,

    /// <summary>
    /// 刀具
    /// </summary>
    [Description("刀具")]
    Cutter = 2,

    /// <summary>
    /// 板料料仓
    /// </summary>
    [Description("板料料仓")]
    PanelSilo = 3,

    /// <summary>
    /// 刀具料仓
    /// </summary>
    [Description("刀具料仓")]
    CutterSilo = 4,

    /// <summary>
    /// 空板料料仓
    /// </summary>
    [Description("空板料料仓")]
    EmptyPanelSilo = 5,

    /// <summary>
    /// 空刀具料仓
    /// </summary>
    [Description("空刀具料仓")]
    EmptyCutterSilo = 6,
}
