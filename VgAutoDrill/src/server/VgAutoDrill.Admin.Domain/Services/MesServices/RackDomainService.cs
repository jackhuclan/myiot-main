using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class RackDomainService : BaseDomainService<Rack>, IRackDomainService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RackDomainService(IUnitOfWork unitOfWork,
            IRackRepository repository)
        {
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
