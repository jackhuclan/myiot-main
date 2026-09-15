using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 系统配置所有Code
    /// </summary>
    public class SysConfigManager : ISysConfigManager
    {
        private readonly ISysConfigService sysConfigService;

        public SysConfigManager(ISysConfigService sysConfigService)
        {
            this.sysConfigService = sysConfigService;
        }

        public async Task<int> GetIntValue(string key, SysConfigCategoryEnum category = SysConfigCategoryEnum.None, bool isReadCache = true)
        {
            var setting = await sysConfigService.GetDataByCode(key, category, isReadCache);
            if (setting.Code != 0 || setting.Data == null)
            {
                return 0;
            }
            return setting.Data.ToInt();
        }

        public async Task<string> GetStringValue(string key, SysConfigCategoryEnum category = SysConfigCategoryEnum.None, bool isReadCache = true)
        {
            var setting = await sysConfigService.GetDataByCode(key, category, isReadCache);
            if (setting.Code != 0 || setting.Data == null)
            {
                return string.Empty;
            }
            return setting.Data.ToString();
        }

        public async Task<bool> GetBoolValue(string key, SysConfigCategoryEnum category = SysConfigCategoryEnum.None, bool isReadCache = true)
        {
            var setting = await sysConfigService.GetDataByCode(key, category, isReadCache);
            if (setting.Code != 0 || setting.Data == null)
            {
                return false;
            }
            return setting.Data.ToBool();
        }

        public async Task InitializeSysConfigs(bool isForceAllRefresh = true)
        {
            await sysConfigService.InitializeSysConfigs(isForceAllRefresh);
        }
    }
}
