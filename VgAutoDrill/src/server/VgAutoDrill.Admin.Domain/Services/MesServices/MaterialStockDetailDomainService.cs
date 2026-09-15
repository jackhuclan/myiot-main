using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class MaterialStockDetailDomainService : BaseDomainService<MaterialStockDetail>, IMaterialStockDetailDomainService
    {
        private readonly IMaterialStockDetailRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialStockDetailDomainService(IUnitOfWork unitOfWork,
            IMaterialStockDetailRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
