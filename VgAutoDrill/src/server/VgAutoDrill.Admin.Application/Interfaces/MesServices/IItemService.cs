using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem.VegaRawMaterial;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IItemService
    {

        Task<ItemDto> FindSingleAsync(string itemCode, string incodeNumber);

        Task<ItemDto> GetItemAsync(string itemCode);

        Task<ResponseDto<ItemDto>> SetItemDrillFilePath(string itemCode, string drillFilePath);

        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<ItemTreeDto>>> GetTreeList();

        /// <summary>
        /// 获取树形数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<ItemFullPropertiesTreeDto>>> GetFullTreeList(GetItemListReq req);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<ItemDto>>> GetList(GetItemListReq req);
        /// <summary>
        /// 获取产品代码列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>        
        Task<List<ItemDto>> GetProductCodes(QueryItemCodeRequest req);
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<ItemDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateItemReq req);

        /// <summary>
        /// 批量添加信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> BulkInsert(List<ItemToExcelDto> list);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateItemReq req);

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
        /// 获取维嘉生料
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<GetVegaRawMaterialDto>>> GetVegaRawMaterial(GetVegaRawMaterialReq req);
    }
}
