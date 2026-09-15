using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class RouteAndProcessDomainService : BaseDomainService<RouteAndProcess>, IRouteAndProcessDomainService
    {
        private readonly IRouteAndProcessRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RouteAndProcessDomainService(IUnitOfWork unitOfWork,
            IRouteAndProcessRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<RouteAndProcessDto>> GetList(GetRouteAndProcessListReq req)
        {
            var pageDto = new PageDto<RouteAndProcessDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<RouteAndProcessDto>>();
            return pageDto;
        }

        public async Task<RouteAndProcessDto> MultiQueryByID(long Id)
        {
            var data = await _repository.MultiQueryByID(Id);
            return data;
        }

        public async Task<PageDto<RouteInfo>> GetRoutesByProcess(GetRoutesByProcessReq req)
        {
            var pageDto = new PageDto<RouteInfo>(req.PageNum, req.PageSize);
            var result = await _repository.GetRoutesByProcess(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<RouteInfo>>();
            return pageDto;
        }
    }
}
