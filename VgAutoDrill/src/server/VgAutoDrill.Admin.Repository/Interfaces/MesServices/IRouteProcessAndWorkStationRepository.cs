using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IRouteProcessAndWorkStationRepository : IBaseRepository<RouteProcessAndWorkStation>
    {
        Task<PageList<RouteProcessAndWorkStationDto>> GetList(GetRouteProcessAndWorkStationListReq req);

        Task<RouteProcessAndWorkStationDto> MultiQueryByID(long Id);

        Task<PageList<RouteInfo>> GetRoutesByWorkStation(GetRouteProcessAndWorkStationListReq req);
        Task<List<WorkstationDto>> GetDrillRouteCodes();

        Task<PageList<FitWorkStationDto>> GetFitWorkStationListByRoute(GetFitWorkStationListByRouteReq req);
    }
}
