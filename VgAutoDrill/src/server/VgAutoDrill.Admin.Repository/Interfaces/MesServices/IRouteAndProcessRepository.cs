using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IRouteAndProcessRepository : IBaseRepository<RouteAndProcess>
    {
        Task<PageList<RouteAndProcessDto>> GetList(GetRouteAndProcessListReq req);

        Task<RouteAndProcessDto> MultiQueryByID(long Id);

        Task<PageList<RouteInfo>> GetRoutesByProcess(GetRoutesByProcessReq req);
    }
}
