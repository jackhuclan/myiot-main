using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IRouteProcessAndWorkStationDomainService : IBaseDomainService<RouteProcessAndWorkStation>
    {
        Task<PageDto<RouteProcessAndWorkStationDto>> GetList(GetRouteProcessAndWorkStationListReq req);

        Task<RouteProcessAndWorkStationDto> MultiQueryByID(long Id);

        Task<PageDto<RouteInfo>> GetRoutesByWorkStation(GetRouteProcessAndWorkStationListReq req);
        Task<List<WorkstationDto>> GetDrillRouteCodes();

        Task<PageDto<FitWorkStationDto>> GetFitWorkStationListByRoute(GetFitWorkStationListByRouteReq req);
    }
}
