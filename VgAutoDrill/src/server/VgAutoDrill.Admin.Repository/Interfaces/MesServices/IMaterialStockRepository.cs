using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IMaterialStockRepository : IBaseRepository<MaterialStock>
    {
        Task<IPageList<MaterialStock>> GetList(GetMaterialStockListReq req, bool isTree = false);
    }
}
