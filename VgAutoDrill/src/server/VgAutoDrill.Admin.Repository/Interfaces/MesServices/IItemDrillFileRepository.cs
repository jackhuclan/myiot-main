using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFile;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IItemDrillFileRepository : IBaseRepository<ItemDrillFile>
    {
        Task<IPageList<ItemDrillFile>> GetList(GetItemDrillFileListReq req);
    }
}
