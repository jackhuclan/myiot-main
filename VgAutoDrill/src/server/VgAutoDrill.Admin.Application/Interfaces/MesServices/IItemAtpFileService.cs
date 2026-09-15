using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    /// <summary>
    /// ATP文件
    /// </summary>
    public interface IItemAtpFileService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<ItemAtpFileDto>>> GetList(GetItemAtpFileListReq req);

        /// <summary>
        /// 根据ItemTypeId获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<ItemAtpFileDto>>> GetEquipmentList(GetItemAtpFileListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<ItemAtpFileDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddCheck(AddOrUpdateItemAtpFileReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateCheck(AddOrUpdateItemAtpFileReq req);

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
        /// 生成ATP文件
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> GenerateATP(string filePath, long Id);
    }
}

