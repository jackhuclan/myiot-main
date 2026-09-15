using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IItemDomainService : IBaseDomainService<Item>
    {
        Task<PageDto<ItemFullPropertiesTreeDto>> PageFullTreeList(GetItemListReq req);

        Task<PageDto<ItemDto>> PageList(GetItemListReq req);
        /// <summary>
        /// 获取产品代码列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>        
        Task<List<ItemDto>> GetProductCodes(QueryItemCodeRequest req);

        /// <summary>
        /// 获取物料信息（包含创建人和修改人姓名）
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>物料信息</returns>
        Task<Item> GetItemWithUserInfo(long id);
    }
}
