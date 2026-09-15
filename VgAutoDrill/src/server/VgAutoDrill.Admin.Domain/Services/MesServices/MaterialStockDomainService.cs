using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class MaterialStockDomainService : BaseDomainService<MaterialStock>, IMaterialStockDomainService
    {
        private readonly IMaterialStockRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialStockDomainService(IUnitOfWork unitOfWork,
            IMaterialStockRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<MaterialStockFullPropertiesTreeDto>> GetTreeList(GetMaterialStockListReq req)
        {
            var pageDto = new PageDto<MaterialStockFullPropertiesTreeDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req, true);

            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<MaterialStockFullPropertiesTreeDto>>();

            return pageDto;
        }

        public async Task<PageDto<MaterialStockDto>> GetList(GetMaterialStockListReq req)
        {
            var pageDto = new PageDto<MaterialStockDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);

            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<MaterialStockDto>>();

            return pageDto;
        }
    }
}
