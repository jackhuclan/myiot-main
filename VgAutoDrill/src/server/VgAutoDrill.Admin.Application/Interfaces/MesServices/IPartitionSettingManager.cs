namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IPartitionSettingManager
    {
        Task<int> GetIntValue(long partitionId, string key, bool isReadCache = true);

        Task<string> GetStringValue(long partitionId, string key, bool isReadCache = true);

        Task<bool> GetBoolValue(long partitionId, string key, bool isReadCache = true);

        Task InitializeSysConfigs(long partitionId = 0);
    }
}
