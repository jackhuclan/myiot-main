using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class ClientDomainService : BaseDomainService<Client>, IClientDomainService
    {
        private readonly IClientRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ClientDomainService(IUnitOfWork unitOfWork,
            IClientRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
