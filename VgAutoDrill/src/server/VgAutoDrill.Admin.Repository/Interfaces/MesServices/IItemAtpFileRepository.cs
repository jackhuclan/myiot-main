using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IItemAtpFileRepository : IBaseRepository<ItemAtpFile>
    {
        Task<IPageList<ItemAtpFile>> GetList(GetItemAtpFileListReq req);
    }
}
