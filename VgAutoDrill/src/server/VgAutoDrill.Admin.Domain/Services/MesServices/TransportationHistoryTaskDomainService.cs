using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class TransportationHistoryTaskDomainService : BaseDomainService<TransportationHistoryTask>, ITransportationHistoryTaskDomainService
    {
        private readonly ITransportationHistoryTaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TransportationHistoryTaskDomainService(IUnitOfWork unitOfWork,
            ITransportationHistoryTaskRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
