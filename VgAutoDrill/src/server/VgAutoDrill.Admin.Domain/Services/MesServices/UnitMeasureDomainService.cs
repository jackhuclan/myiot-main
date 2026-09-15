using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class UnitMeasureDomainService : BaseDomainService<UnitMeasure>, IUnitMeasureDomainService
    {
        private readonly IUnitMeasureRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UnitMeasureDomainService(IUnitOfWork unitOfWork,
            IUnitMeasureRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
