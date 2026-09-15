using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

public enum DeviceKind
{
    /// <summary>
    /// 未知设备
    /// </summary>
    [Description("未知设备")]
    Unknown = 0,
    /// <summary>
    /// 模拟设备
    /// </summary>
    [Description("模拟设备")]
    Mock = 1,
    /// <summary>
    /// CNC84钻机
    /// </summary>
    [Description("CNC84钻机")]
    CNC84Drill = 2,
    /// <summary>
    /// CNC95钻机
    /// </summary>
    [Description("CNC95钻机")]
    CNC95Drill = 3,
    /// <summary>
    /// 叠板机
    /// </summary>
    [Description("叠板机")]
    Pin = 4,
    /// <summary>
    /// 拆板机
    /// </summary>
    [Description("拆板机")]
    UnPin = 5,
    /// <summary>
    /// BackPanelAgv
    /// </summary>
    [Description("BackPanelAgv")]
    BackPanelAgv = 6,
    /// <summary>
    /// 前上料AGV
    /// </summary>
    [Description("前上料AGV")]
    FrontPanelAgv = 7,
    /// <summary>
    /// 换刀AGV
    /// </summary>
    [Description("换刀AGV")]
    FrontToolAgv = 8,
    /// <summary>
    /// RollerSiloAgv
    /// </summary>
    [Description("RollerSiloAgv")]
    RollerSiloAgv = 9,
    /// <summary>
    /// 运输AGV
    /// </summary>
    [Description("运输AGV")]
    ShelfSiloAgv = 10,
    /// <summary>
    /// 生料仓暂存台
    /// </summary>
    [Description("生料仓暂存台")]
    RawTagingDesk = 11,
    /// <summary>
    /// 熟料仓暂存台
    /// </summary>
    [Description("熟料仓暂存台")]
    ProcessedTagingDesk = 12,
    /// <summary>
    /// 公共料仓缓存区域设备类型
    /// </summary>
    [Description("公共料仓缓存区域设备类型")]
    PublicPanelSiloWIP = 13,
    /// <summary>
    /// 刀具料架
    /// </summary>
    [Description("刀具料架")]
    CutterSiloShelf = 14,
    /// <summary>
    /// 板料叉齿
    /// </summary>
    [Description("板料叉齿")]
    PanelSiloFork = 15
}
