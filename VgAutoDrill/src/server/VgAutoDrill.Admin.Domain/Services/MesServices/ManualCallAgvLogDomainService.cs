using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ManualCallAgvLogDomainService : BaseDomainService<ManualCallAgvLog>, IManualCallAgvLogDomainService
    {
        private readonly IManualCallAgvLogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public ManualCallAgvLogDomainService(
            IUnitOfWork unitOfWork,
          IManualCallAgvLogRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _baseRepository = repository;
        }

    }
}
