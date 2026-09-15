namespace VgAutoDrill.Central.Core.Schedule.Summary;

/// <summary>
/// 负载的熟料仓
/// </summary>
public class PayloadDrilledSiloSummary : ItemSummary
{
    /// <summary>
    /// 料仓号
    /// </summary>
    public IReadOnlyList<string> SiloCodes { get; set; }

    /// <summary>
    /// 料仓数量
    /// </summary>
    public int SiloCount { get; set; }

    /// <summary>
    /// 空层数
    /// </summary>
    public IReadOnlyList<int> EmptyLayerCounts { get; set; }

    /// <summary>
    /// 库位号
    /// </summary>
    public IReadOnlyList<string> LocatioCodes { get; set; }
}
