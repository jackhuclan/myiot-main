using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTransOrder;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class ProTransOrderDomainService : BaseDomainService<TransOrder>, IProTransOrderDomainService
    {
        private readonly IProTransOrderRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProTransOrderDomainService(IUnitOfWork unitOfWork,
            IProTransOrderRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<TransOrderDto>> GetList(GetTransOrderListReq req)
        {
            var pageDto = new PageDto<TransOrderDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<TransOrderDto>>();
            return pageDto;
        }
    }
}
