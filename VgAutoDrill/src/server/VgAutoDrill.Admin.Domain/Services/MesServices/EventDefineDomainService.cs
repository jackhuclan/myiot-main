using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class EventDefineDomainService : BaseDomainService<EventDefine>, IEventDefineDomainService
    {
        private readonly IEventDefineRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public EventDefineDomainService(IUnitOfWork unitOfWork,
            IEventDefineRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
