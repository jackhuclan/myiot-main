using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Mysql;

/// <summary>
/// 分区自动料仓转运配置
/// </summary>
public class PartitionAutoSiloTransferOptions : ICloneable
{
    public static PartitionAutoSiloTransferOptions None = new();

    /// <summary>
    /// 料仓中转位和车间内线边仓模式
    /// </summary>
    public AutoSiloTransferMode TransferMode { get; set; } = AutoSiloTransferMode.None;

    /// <summary>
    /// 分区类型
    /// </summary>
    public PartitionKind PartitionKind { get; set; } = PartitionKind.Unknown;

    /// <summary>
    /// 分区代码
    /// </summary>
    public string PartCode { get; set; } = string.Empty;

    /// <summary>
    /// 允许熟料转出
    /// </summary>
    public bool EnableDrilledTrackOut { get; set; } = false;

    /// <summary>
    /// 允许熟料转入
    /// </summary>
    public bool EnableDrilledTrackIn { get; set; } = false;

    /// <summary>
    /// 允许生料转出
    /// </summary>
    public bool EnableUndrilledTrackOut { get; set; } = false;

    /// <summary>
    /// 允许生料转入
    /// </summary>
    public bool EnableUndrilledTrackIn { get; set; } = false;

    /// <summary>
    /// 允许空料盒/空仓转出
    /// </summary>
    public bool EnableEmptyBoxTrackOut { get; set; } = false;

    /// <summary>
    /// 允许空料盒/空仓转入
    /// </summary>
    public bool EnableEmptyBoxTrackIn { get; set; } = false;

    /// <summary>
    /// 允许首件转出
    /// </summary>
    public bool EnableFirstTrackOut { get; set; } = false;

    /// <summary>
    /// 允许首件转入
    /// </summary>
    public bool EnableFirstTrackIn { get; set; } = false;

    public object Clone() => this.MemberwiseClone();
}
