using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class TransportationTaskDomainService : BaseDomainService<TransferJob>, ITransportationTaskDomainService
    {
        private readonly ITransportationTaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public TransportationTaskDomainService(IUnitOfWork unitOfWork,
            ITransportationTaskRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
