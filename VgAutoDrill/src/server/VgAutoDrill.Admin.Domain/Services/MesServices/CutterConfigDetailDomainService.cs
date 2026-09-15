using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class CutterConfigDetailDomainService : BaseDomainService<CutterConfigDetail>, ICutterConfigDetailDomainService
    {
        private readonly ICutterConfigDetailRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CutterConfigDetailDomainService(IUnitOfWork unitOfWork,
            ICutterConfigDetailRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
