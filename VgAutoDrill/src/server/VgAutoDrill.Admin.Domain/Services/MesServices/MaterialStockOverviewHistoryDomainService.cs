using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class MaterialStockOverviewHistoryDomainService : BaseDomainService<MaterialStockOverviewHistory>, IMaterialStockOverviewHistoryDomainService
    {
        private readonly IMaterialStockOverviewHistoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialStockOverviewHistoryDomainService(IUnitOfWork unitOfWork,
            IMaterialStockOverviewHistoryRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}

