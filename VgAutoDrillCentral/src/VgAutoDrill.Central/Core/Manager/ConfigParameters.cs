namespace VgAutoDrill.Central.Core.Manager;

/// <summary>
/// sysConfigManager获取的系统参数
/// </summary>
public record ConfigParameters
{
    /// <summary>
    /// 最少首件转出数量
    /// </summary>
    public int MinFirstDrilledTrackOutNum { get; set; }
    /// <summary>
    /// 最少熟料转出数量
    /// </summary>
    public int MinDrilledTrackOutNum { get; set; }
    /// <summary>
    /// 是否自动转出熟料
    /// </summary>
    public bool AutoDrilledTrackOutSilo { get; set; }
    /// <summary>
    /// 生料占用库位超时时间秒
    /// </summary>
    public int UndrilledItemOccupyLocationIdleTimeout { get; set; } = 30;
    /// <summary>
    /// 熟料占用库位超时时间 分钟
    /// </summary>
    public int DrilledItemOccupyLocationIdleTimeout { get; set; } = 20;

    /// <summary>
    /// 允许超时的熟料转出
    /// </summary>
    public bool EnableIdleTimeoutDrilledTrackout { get; set; } = false;

    public int CountEmptyPayloadForkNow { get; set; } = 1;
}
