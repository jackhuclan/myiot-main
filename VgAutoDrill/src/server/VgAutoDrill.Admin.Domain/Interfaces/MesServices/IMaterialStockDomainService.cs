using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IMaterialStockDomainService : IBaseDomainService<MaterialStock>
    {
        Task<PageDto<MaterialStockFullPropertiesTreeDto>> GetTreeList(GetMaterialStockListReq req);

        Task<PageDto<MaterialStockDto>> GetList(GetMaterialStockListReq req);
    }
}
