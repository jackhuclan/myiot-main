using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class VendorDomainService : BaseDomainService<Vendor>, IVendorDomainService
    {
        private readonly IVendorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public VendorDomainService(IUnitOfWork unitOfWork,
            IVendorRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
