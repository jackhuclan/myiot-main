using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class SiloDomainService : BaseDomainService<Silo>, ISiloDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SiloDomainService(IUnitOfWork unitOfWork,
            ISiloRepository repository)
        {
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
