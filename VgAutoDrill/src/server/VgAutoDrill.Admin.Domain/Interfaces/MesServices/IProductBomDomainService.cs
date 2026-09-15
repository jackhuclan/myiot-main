using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductBom;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IProductBomDomainService : IBaseDomainService<ProductBom>
    {
        Task<PageDto<ProductBomDto>> GetList(GetProductBomListReq req);

        Task<ProductBomDto> MultiQueryByID(long Id);
    }
}
