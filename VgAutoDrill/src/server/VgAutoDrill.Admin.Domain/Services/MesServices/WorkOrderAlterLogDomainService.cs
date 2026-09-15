using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class WorkOrderAlterLogDomainService : BaseDomainService<WorkOrderAlterLog>, IWorkOrderAlterLogDomainService
    {
        private readonly IWorkOrderAlterLogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkOrderAlterLogDomainService(IUnitOfWork unitOfWork,
            IWorkOrderAlterLogRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
