using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class RouteProcessAndWorkStationDomainService : BaseDomainService<RouteProcessAndWorkStation>, IRouteProcessAndWorkStationDomainService
    {
        private readonly IRouteProcessAndWorkStationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RouteProcessAndWorkStationDomainService(IUnitOfWork unitOfWork,
            IRouteProcessAndWorkStationRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<RouteProcessAndWorkStationDto>> GetList(GetRouteProcessAndWorkStationListReq req)
        {
            var pageDto = new PageDto<RouteProcessAndWorkStationDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<RouteProcessAndWorkStationDto>>();
            return pageDto;
        }

        public async Task<RouteProcessAndWorkStationDto> MultiQueryByID(long Id)
        {
            var data = await _repository.MultiQueryByID(Id);
            return data;
        }

        public async Task<PageDto<RouteInfo>> GetRoutesByWorkStation(GetRouteProcessAndWorkStationListReq req)
        {
            var pageDto = new PageDto<RouteInfo>(req.PageNum, req.PageSize);
            var result = await _repository.GetRoutesByWorkStation(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<RouteInfo>>();
            return pageDto;
        }
        public async Task<List<WorkstationDto>> GetDrillRouteCodes()
        {
            return await _repository.GetDrillRouteCodes();
        }

        public async Task<PageDto<FitWorkStationDto>> GetFitWorkStationListByRoute(GetFitWorkStationListByRouteReq req)
        {
            var pageDto = new PageDto<FitWorkStationDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetFitWorkStationListByRoute(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<FitWorkStationDto>>();
            return pageDto;
        }
    }
}
