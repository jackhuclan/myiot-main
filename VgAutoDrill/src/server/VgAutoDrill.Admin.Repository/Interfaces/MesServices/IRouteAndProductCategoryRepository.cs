using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IRouteAndProductCategoryRepository : IBaseRepository<RouteAndProductCategory>
    {
        Task<PageList<RouteAndProductCategoryDto>> GetList(GetRouteAndProductCategoryListReq req);

        Task<PageList<RouteInfoByProductCategoryDto>> GetRouteInfoList(GetRouteAndProductCategoryListReq req);

        Task<RouteAndProductCategoryDto> MultiQueryByID(long Id);
    }
}
