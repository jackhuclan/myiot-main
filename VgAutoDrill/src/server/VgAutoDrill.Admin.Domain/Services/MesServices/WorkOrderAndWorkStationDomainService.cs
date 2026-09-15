using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class WorkOrderAndWorkStationDomainService : BaseDomainService<WorkOrderAndWorkStation>, IWorkOrderAndWorkStationDomainService
    {
        private readonly IWorkOrderAndWorkStationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkOrderAndWorkStationDomainService(IUnitOfWork unitOfWork,
            IWorkOrderAndWorkStationRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
