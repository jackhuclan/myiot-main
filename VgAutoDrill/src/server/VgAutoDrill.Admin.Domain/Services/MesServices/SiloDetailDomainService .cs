using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class SiloDetailDomainService : BaseDomainService<SiloDetail>, ISiloDetailDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SiloDetailDomainService(IUnitOfWork unitOfWork,
            ISiloDetailRepository repository)
        {
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
