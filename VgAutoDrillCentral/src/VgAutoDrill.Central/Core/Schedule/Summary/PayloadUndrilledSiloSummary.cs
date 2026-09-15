namespace VgAutoDrill.Central.Core.Schedule.Summary;

/// <summary>
/// 负载的生料仓
/// </summary>
public class PayloadUndrilledSiloSummary : ItemSummary
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
    /// 生料数量
    /// </summary>
    public IReadOnlyList<int> UndrilledItemCounts { get; set; }

    /// <summary>
    /// 库位号
    /// </summary>
    public IReadOnlyList<string> LocatioCodes { get; set; }
}
