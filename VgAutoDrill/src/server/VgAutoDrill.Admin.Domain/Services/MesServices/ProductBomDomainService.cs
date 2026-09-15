using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductBom;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ProductBomDomainService : BaseDomainService<ProductBom>, IProductBomDomainService
    {
        private readonly IProductBomRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductBomDomainService(IUnitOfWork unitOfWork,
            IProductBomRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<ProductBomDto>> GetList(GetProductBomListReq req)
        {
            var pageDto = new PageDto<ProductBomDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<ProductBomDto>>();
            return pageDto;
        }

        public async Task<ProductBomDto> MultiQueryByID(long Id)
        {
            var data = await _repository.MultiQueryByID(Id);
            return data;
        }
    }
}
