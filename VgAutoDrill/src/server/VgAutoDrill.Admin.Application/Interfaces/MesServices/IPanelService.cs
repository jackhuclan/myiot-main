using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Panel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPanelService
    {
        /// <summary>
        /// 获取树形数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<PanelFullPropertiesTreeDto>>> GetFullTreeList(GetPanelListReq req);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<PanelDto>>> GetList(GetPanelListReq req);

        /// <summary>
        /// 获取板料追溯List
        /// </summary>
        /// <returns></returns>
        Task<List<PanelToExcelDto>> GetToExcelList(GetPanelListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<PanelDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddData(AddOrUpdatePanelReq req);
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> BatchAddData(List<AddOrUpdatePanelReq> panels);
        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> UpdateData(AddOrUpdatePanelReq req);

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
        /// <summary>
        /// 批量初始化板料信息
        /// </summary>
        /// <param name="batchInsertPanelReq"></param>
        /// <returns></returns>
        Task<List<PanelDto>> GeneratePanels(BatchInsertPanelReq batchInsertPanelReq);
    }
}
