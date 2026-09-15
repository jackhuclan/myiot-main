using VgAutoDrill.Admin.Model.ViewModels.Mes.Partition;

namespace VgAutoDrill.Central.Core.Mes.Interface;

public interface IPartitionAdapter
{
    Task<List<PartitionDto>> GetPartitions();

    Task RefreshCurrentAgv(string carDeviceId, string carCurrentPos);

    Task<bool> TryUnBookPartition(string partitionCode, string deviceId);

    Task HandleUnbookRest(string restCode);

    Task HandleBookRest(string restCode, string prebookAgv);

    Task UnbookPart(string partitionCode);
}
