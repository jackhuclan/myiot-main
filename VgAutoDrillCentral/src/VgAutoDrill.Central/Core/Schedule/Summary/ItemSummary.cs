namespace VgAutoDrill.Central.Core.Schedule.Summary;

public class ItemSummary : IEquatable<ItemSummary>
{
    /// <summary>
    /// 料号
    /// </summary>
    public string ItemCode { get; set; }

    public int ItemCount { get; set; }

    public bool Equals(ItemSummary? other)
    {
        return other != null && string.Equals(ItemCode, other.ItemCode, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode() => this.ItemCode.ToLowerInvariant().GetHashCode();
}
