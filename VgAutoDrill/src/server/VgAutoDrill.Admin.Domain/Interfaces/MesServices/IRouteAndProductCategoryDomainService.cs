using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IRouteAndProductCategoryDomainService : IBaseDomainService<RouteAndProductCategory>
    {
        Task<PageDto<RouteAndProductCategoryDto>> GetList(GetRouteAndProductCategoryListReq req);

        Task<PageDto<RouteInfoByProductCategoryDto>> GetRouteInfoList(GetRouteAndProductCategoryListReq req);

        Task<RouteAndProductCategoryDto> MultiQueryByID(long Id);
    }
}
