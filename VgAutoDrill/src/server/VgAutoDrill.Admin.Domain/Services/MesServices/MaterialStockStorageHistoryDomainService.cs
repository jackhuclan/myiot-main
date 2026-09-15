using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class MaterialStockStorageHistoryDomainService : BaseDomainService<MaterialStockStorageHistory>, IMaterialStockStorageHistoryDomainService
    {
        private readonly IMaterialStockStorageHistoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialStockStorageHistoryDomainService(IUnitOfWork unitOfWork,
            IMaterialStockStorageHistoryRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
