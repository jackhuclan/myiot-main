using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class WorkOrderDomainService : BaseDomainService<WorkOrder>, IWorkOrderDomainService
    {
        private readonly IProWorkOrderRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkOrderDomainService(IUnitOfWork unitOfWork,
            IProWorkOrderRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<WorkOrderDto>> GetList(GetWorkOrderListReq req)
        {
            var pageDto = new PageDto<WorkOrderDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<WorkOrderDto>>();
            return pageDto;
        }

    }
}
