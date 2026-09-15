using System.Diagnostics;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot;

public class PanelListSnapshot : List<PanelListSnapshotEntry>, IEquatable<PanelListSnapshot>
{
    public long Timestamp { get; private set; }
    public PanelListSnapshot()
    {
        Timestamp = DateTime.Now.Ticks;
    }

    public PanelListSnapshot(List<PanelListSnapshotEntry> entries)
    {
        this.AddRange(entries);
        Timestamp = DateTime.Now.Ticks;
    }

    public bool Equals(PanelListSnapshot? other)
    {
        if (other == null) return false;
        if (this.Count() != other.Count()) return false;
        this.Sort();
        other.Sort();

        int len = this.Count();
        for (int i = 0; i < len; i++)
        {
            if (!this[i].Equals(other[i]))
                return false;
        }

        return true;
    }

    public override string ToString() => string.Join(Environment.NewLine, this);

    public override bool Equals(object? obj)
    {
        return obj != null && Equals(obj as PanelListSnapshot);
    }

    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }
}

[DebuggerDisplay("{SiloCode}|{ItemStatus}|{ItemCode}|{ItemCount}")]
public class PanelListSnapshotEntry : IEquatable<PanelListSnapshotEntry>, IComparable<PanelListSnapshotEntry>
{
    private string _itemCode = string.Empty;
    private string _siloCode = string.Empty;

    public ProductStatus ItemStatus { get; set; } = ProductStatus.Noop;
    public string ItemCode { get => _itemCode; set => _itemCode = value ?? string.Empty; }
    public string SiloCode { get => _siloCode; set => _siloCode = value; }
    public int ItemCount { get; set; } = 0;
    public override string ToString() => $"{SiloCode.ToUpper()}|{ItemStatus}|{ItemCode.ToUpper()}|{ItemCount}";

    public override int GetHashCode() => this.ToString().GetHashCode();

    public int CompareTo(PanelListSnapshotEntry? other)
    {
        if (other == null)
            return 1;

        if (string.Compare(this.SiloCode, other.SiloCode, true) != 0)
            return string.Compare(this.SiloCode, other.SiloCode, true);

        if (this.ItemStatus != other.ItemStatus)
            return this.ItemStatus.CompareTo(other.ItemStatus);

        if (string.Compare(this.ItemCode, other.ItemCode, true) != 0)
            return string.Compare(this.ItemCode, other.ItemCode, true);

        if (this.ItemCount != other.ItemCount)
            return this.ItemCount.CompareTo(other.ItemCount);

        return 0;
    }

    public override bool Equals(object? obj)
    {
        return obj != null && Equals(obj as PanelListSnapshotEntry);
    }

    public bool Equals(PanelListSnapshotEntry? other)
    {
        return other != null
            && this.ItemStatus == other.ItemStatus
            && string.Compare(this.ItemCode, other.ItemCode, true) == 0
            && string.Compare(this.SiloCode, other.SiloCode, true) == 0
            && this.ItemCount == other.ItemCount;
    }
}
