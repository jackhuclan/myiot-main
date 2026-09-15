using VgAutoDrill.Admin.Model.ViewModels;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IPartitionSettingService
    {
        Task<ResponseDto<object>> GetDataByCode(long partitionId, string configCode, bool isReadCache = true);

        Task InitializeSysConfigs(long partitionId = 0);
    }
}
