using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 分区配置
    /// </summary>
    public class PartitionSettingManager : IPartitionSettingManager
    {
        private readonly IPartitionSettingService _partitionSettingService;

        public PartitionSettingManager(IPartitionSettingService partitionSettingService)
        {
            _partitionSettingService = partitionSettingService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partitionCode"></param>
        /// <param name="key">(PartitionSettingConstants )</param>
        /// <param name="isReadCache"></param>
        /// <returns></returns>
        public async Task<int> GetIntValue(long partitionId, string key, bool isReadCache = true)
        {
            var setting = await _partitionSettingService.GetDataByCode(partitionId, key, isReadCache);
            if (setting.Code != 0 || setting.Data == null)
            {
                return 0;
            }
            return setting.Data.ToInt();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partitionCode">(PartitionSettingConstants)</param>
        /// <param name="key"></param>
        /// <param name="isReadCache"></param>
        /// <returns></returns>
        public async Task<string> GetStringValue(long partitionId, string key, bool isReadCache = true)
        {
            var setting = await _partitionSettingService.GetDataByCode(partitionId, key, isReadCache);
            if (setting.Code != 0 || setting.Data == null)
            {
                return string.Empty;
            }
            return setting.Data.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partitionCode"></param>
        /// <param name="key">(PartitionSettingConstants)</param>
        /// <param name="isReadCache"></param>
        /// <returns></returns>
        public async Task<bool> GetBoolValue(long partitionId, string key, bool isReadCache = true)
        {
            var setting = await _partitionSettingService.GetDataByCode(partitionId, key, isReadCache);
            if (setting.Code != 0 || setting.Data == null)
            {
                return false;
            }
            return setting.Data.ToBool();
        }

        public async Task InitializeSysConfigs(long partitionId = 0)
        {
            await _partitionSettingService.InitializeSysConfigs(partitionId);
        }

    }
}
