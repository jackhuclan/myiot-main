using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ProduceTaskDomainService : BaseDomainService<ProduceTask>, IProduceTaskDomainService
    {
        private readonly IProduceTaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProduceTaskDomainService(IUnitOfWork unitOfWork,
            IProduceTaskRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
