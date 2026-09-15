using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Manager;

public interface IPartitionManager
{
    Task Refresh();

    IReadOnlyList<RestPoint> RestPoints { get; }
    IReadOnlyList<Partition> Partitions { get; }
    IReadOnlyList<RestPoint> BookedRestPoints { get; }
    IReadOnlyList<PartitionRelation> PartitionRelations { get; }
    /// <summary>
    /// 公共空仓区域
    /// </summary>
    IReadOnlyList<Partition> PublicEmptySiloPartitions { get; }
    /// <summary>
    /// 公共生料区域
    /// </summary>
    IReadOnlyList<Partition> PublicRawPartitions { get; }

    /// <summary>
    /// 公共熟料区域
    /// </summary>
    IReadOnlyList<Partition> PublicClinkerPartitions { get; }

    /// <summary>
    /// 公共首件区域
    /// </summary>
    IReadOnlyList<Partition> PublicFirstPartitions { get; }

    bool TryFindBookedRest(string carDeviceId, out RestPoint? restPoint);

    bool TryGetRestPoint(string restCode, out RestPoint? restPoint);

    Task<string> SetCarPosition(string carDeviceId, string carCurrentPos);

    Task<BookPartResult> TryBookPartition(Partition partition, Agv agvDeviceProxy);
    Task<BookPartResult> TryBookPartition(Agv agv, Location location);

    bool TryGetPartition(string partitionCode, out Partition? partition);

    Task<string> TryUnbookPart(string partitionCode);

    Task<string> TryUnbookPartition(string agvDeviceId);

    Task<RestPoint?> TryBookRest(Agv agvDeviceProxy);
    bool TryGetPartition(IReadOnlyList<string> routeCodes, out Partition? partition);
}
