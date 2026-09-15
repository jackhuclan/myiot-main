using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDeviceAndRouteDomainService : IBaseDomainService<DeviceAndRoute>
    {
        Task<PageDto<DeviceInfoAndRouteInfo>> GetList(GetDeviceAndRouteListReq req);

        Task<PageDto<DeviceFullDataAndRouteInfo>> GetDeviceAndRouteList(GetDeviceFullDataAndRouteInfoReq req);
    }
}
