using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class AgvRestCodeDomainService : BaseDomainService<AgvRest>, IAgvRestDomainService
    {
        private readonly IAgvRestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AgvRestCodeDomainService(IUnitOfWork unitOfWork,
            IAgvRestRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

    }
}
