using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceAndRouteDomainService : BaseDomainService<DeviceAndRoute>, IDeviceAndRouteDomainService
    {
        private readonly IDeviceAndRouteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceAndRouteDomainService(IUnitOfWork unitOfWork,
            IDeviceAndRouteRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<DeviceInfoAndRouteInfo>> GetList(GetDeviceAndRouteListReq req)
        {
            var pageDto = new PageDto<DeviceInfoAndRouteInfo>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<DeviceInfoAndRouteInfo>>();
            return pageDto;
        }

        public async Task<PageDto<DeviceFullDataAndRouteInfo>> GetDeviceAndRouteList(GetDeviceFullDataAndRouteInfoReq req)
        {
            var pageDto = new PageDto<DeviceFullDataAndRouteInfo>(req.PageNum, req.PageSize);
            var result = await _repository.GetDeviceAndRouteList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<DeviceFullDataAndRouteInfo>>();
            return pageDto;
        }
    }
}
