using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class RouteAndProductCategoryDomainService : BaseDomainService<RouteAndProductCategory>, IRouteAndProductCategoryDomainService
    {
        private readonly IRouteAndProductCategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public RouteAndProductCategoryDomainService(IUnitOfWork unitOfWork,
            IRouteAndProductCategoryRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<PageDto<RouteAndProductCategoryDto>> GetList(GetRouteAndProductCategoryListReq req)
        {
            var pageDto = new PageDto<RouteAndProductCategoryDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<RouteAndProductCategoryDto>>();
            return pageDto;
        }

        public async Task<PageDto<RouteInfoByProductCategoryDto>> GetRouteInfoList(GetRouteAndProductCategoryListReq req)
        {
            var pageDto = new PageDto<RouteInfoByProductCategoryDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetRouteInfoList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<RouteInfoByProductCategoryDto>>();
            return pageDto;
        }

        public async Task<RouteAndProductCategoryDto> MultiQueryByID(long Id)
        {
            var data = await _repository.MultiQueryByID(Id);
            return data;
        }
    }
}
