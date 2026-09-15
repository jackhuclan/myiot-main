using VgAutoDrill.Admin.Model.Enum;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface ISysConfigManager
    {
        Task<int> GetIntValue(string key, SysConfigCategoryEnum category = SysConfigCategoryEnum.None, bool isReadCache = true);

        Task<string> GetStringValue(string key, SysConfigCategoryEnum category = SysConfigCategoryEnum.None, bool isReadCache = true);

        Task<bool> GetBoolValue(string key, SysConfigCategoryEnum category = SysConfigCategoryEnum.None, bool isReadCache = true);

        Task InitializeSysConfigs(bool isForceAllRefresh = true);
    }
}
