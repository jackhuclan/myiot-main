using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductBom;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IProductBomRepository : IBaseRepository<ProductBom>
    {
        Task<PageList<ProductBomDto>> GetList(GetProductBomListReq req);

        Task<ProductBomDto> MultiQueryByID(long Id);
    }
}
