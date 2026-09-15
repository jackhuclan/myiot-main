using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public interface ISysConfigService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<SysConfigDto>>> GetList(GetSysConfigListReq req);
        Task<ResponseDto<List<SysConfigTreeDto>>> GetTreeList(GetSysConfigListReq req);
        Task<ResponseDto<List<TreeDto>>> GetSysConfigCategoryEnums();

        /// <summary>
        /// 获取枚举配置项集合
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<SysConfigInfoByEnumType>> GetSysConfigEnumList(GetSysConfigByEnumType req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<SysConfigDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdateSysConfigReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdateSysConfigReq req);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteData(long id);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteDataList(List<long> idList);

        /// <summary>
        /// 根据编码获取配置项数值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<ResponseDto<object>> GetDataByCode(string code, SysConfigCategoryEnum category = SysConfigCategoryEnum.None, bool isReadCache = true);

        /// <summary>
        /// 获取告警信息等及时信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<SysTimelyInformationDto>> GetSysTimelyInformation();
        Task<ResponseDto<string>> SaveBasicSysData(List<AddOrUpdateSysConfigReq> reqs);
        Task InitializeSysConfigs(bool isForceAllRefresh = true);
    }
}
