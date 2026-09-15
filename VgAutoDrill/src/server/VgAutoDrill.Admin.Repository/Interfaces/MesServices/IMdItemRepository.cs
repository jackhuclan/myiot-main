using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;

namespace VgAutoDrill.Admin.Repository.Interfaces.Equipment
{
    public interface IMdItemRepository : IBaseRepository<Item>
    {
        Task<IPageList<Item>> GetList(GetItemListReq req, bool isTree = false);

        /// <summary>
        /// 获取物料信息（包含创建人和修改人姓名）
        /// </summary>
        /// <param name="id">物料ID</param>
        /// <returns>物料信息</returns>
        Task<Item> GetItemWithUserInfo(long id);
    }
}
