namespace VgAutoDrill.Central.Core.Mysql;

/// <summary>
/// 自动料仓转运模式
/// </summary>
public enum AutoSiloTransferMode
{
    /// <summary>
    /// 不转运
    /// </summary>
    None = 0,

    /// <summary>
    /// 在fork和outside之间转运
    /// </summary>
    ForkOutside = 1,

    /// <summary>
    /// 在wip和outside之间转运
    /// </summary>
    WipOutside = 2,

    /// <summary>
    /// 在fork, wip和outside之间转运
    /// </summary>
    ForkWipOutside = 3,

    /// <summary>
    /// 在fork和wip之间转运
    /// </summary>
    ForkWip = 4
}
