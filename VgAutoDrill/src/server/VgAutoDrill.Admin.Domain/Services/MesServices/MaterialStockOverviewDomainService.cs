using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class MaterialStockOverviewDomainService : BaseDomainService<MaterialStockOverview>, IMaterialStockOverviewDomainService
    {
        private readonly IMaterialStockOverviewRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialStockOverviewDomainService(IUnitOfWork unitOfWork,
            IMaterialStockOverviewRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
