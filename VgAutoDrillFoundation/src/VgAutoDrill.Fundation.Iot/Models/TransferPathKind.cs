using System.ComponentModel;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 转移路线种类
/// </summary>
public enum TransferPathKind
{
    /// <summary>
    /// 未知
    /// </summary>
    [Description("未知")]
    Unknown = 0,

    /// <summary>
    /// 中转位到pin
    /// </summary>
    [Description("中转位到pin")]
    ForkToPin = 1,

    /// <summary>
    /// pin到中转位
    /// </summary>
    [Description("pin到中转位")]
    PinToFork = 2,

    /// <summary>
    /// 中转位到unpin
    /// </summary>
    [Description("中转位到unpin")]
    ForkToUnPin = 3,

    /// <summary>
    /// unpin到中转位
    /// </summary>
    [Description("unpin到中转位")]
    UnPinToFork = 4,

    /// <summary>
    /// 中转位到线边仓
    /// </summary>
    [Description("中转位到线边仓")]
    ForkToWip = 5,

    /// <summary>
    /// 线边仓到中转位
    /// </summary>
    [Description("线边仓到中转位")]
    WipToFork = 6,

    /// <summary>
    /// 线边仓到外部退pin线
    /// </summary>
    [Description("线边仓到外部退pin线")]
    WipToOutsideUnPin = 7,

    /// <summary>
    /// 线边仓到内部退pin线
    /// </summary>
    [Description("线边仓到内部退pin线")]
    WipToInnerUnPin = 8,

    /// <summary>
    /// 外部到wip
    /// </summary>
    [Description("外部到wip")]
    OutsideToWip = 9,

    /// <summary>
    /// wip到外部工厂
    /// </summary>
    [Description("wip到外部工厂")]
    WipToOutside = 10,

    /// <summary>
    /// 外部到中转位
    /// </summary>
    [Description("外部到中转位")]
    OutsideToFork = 11,

    /// <summary>
    /// 中转位到外部退pin线
    /// </summary>
    [Description("中转位到外部退pin线")]
    ForkToOutsideUnPin = 12,

    /// <summary>
    /// 中转位到外部
    /// </summary>
    [Description("中转位到外部")]
    ForkToOutside = 13,

    /// <summary>
    /// 线边仓到上pin
    /// </summary>
    [Description("线边仓到上pin")]
    WipToPin = 14,

    /// <summary>
    /// 上pin到线边仓
    /// </summary>
    [Description("上pin到线边仓")]
    PinToWip = 15,

    /// <summary>
    /// 退pin线到线边仓
    /// </summary>
    [Description("退pin线到线边仓")]
    UnPinToWip = 16,

    /// <summary>
    /// 线边仓到退pin线
    /// </summary>
    [Description("线边仓到退pin线")]
    WipToUnPin = 17
}
