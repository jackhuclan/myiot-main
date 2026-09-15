using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DrillWrokOrderDomainService : BaseDomainService<DrillWorkOrder>, IDrillTaskDomainService
    {
        private readonly IDrillTaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DrillWrokOrderDomainService(IUnitOfWork unitOfWork,
            IDrillTaskRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}

