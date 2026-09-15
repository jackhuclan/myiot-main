using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class MaterialStockStorageDomainService : BaseDomainService<MaterialStockStorage>, IMaterialStockStorageDomainService
    {
        private readonly IMaterialStockStorageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialStockStorageDomainService(IUnitOfWork unitOfWork,
            IMaterialStockStorageRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}