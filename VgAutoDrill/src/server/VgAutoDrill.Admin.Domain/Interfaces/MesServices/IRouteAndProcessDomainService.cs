using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IRouteAndProcessDomainService : IBaseDomainService<RouteAndProcess>
    {
        Task<PageDto<RouteAndProcessDto>> GetList(GetRouteAndProcessListReq req);

        Task<RouteAndProcessDto> MultiQueryByID(long Id);

        Task<PageDto<RouteInfo>> GetRoutesByProcess(GetRoutesByProcessReq req);
    }
}
