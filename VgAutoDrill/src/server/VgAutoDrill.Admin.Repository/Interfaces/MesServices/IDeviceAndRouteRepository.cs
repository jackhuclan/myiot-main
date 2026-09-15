using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDeviceAndRouteRepository : IBaseRepository<DeviceAndRoute>
    {
        Task<PageList<DeviceInfoAndRouteInfo>> GetList(GetDeviceAndRouteListReq req);

        Task<PageList<DeviceFullDataAndRouteInfo>> GetDeviceAndRouteList(GetDeviceFullDataAndRouteInfoReq req);
    }
}

