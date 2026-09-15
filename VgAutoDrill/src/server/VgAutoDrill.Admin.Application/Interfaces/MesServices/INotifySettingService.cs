using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvInformType;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotifySettingService
    {
        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<NotifySettingTreeDto>>> GetTreeList();

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<NotifySettingDto>>> GetList(GetNotifySettingListReq req);


        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<NotifySettingDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateNotifySettingReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateNotifySettingReq req);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteList(object[] idList);
    }
}
