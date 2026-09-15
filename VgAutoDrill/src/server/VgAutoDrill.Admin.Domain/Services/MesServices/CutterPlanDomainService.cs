using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class CutterPlanDomainService : BaseDomainService<CutterPlan>, ICutterPlanDomainService
    {
        private readonly ICutterPlanRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CutterPlanDomainService(IUnitOfWork unitOfWork,
            ICutterPlanRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
