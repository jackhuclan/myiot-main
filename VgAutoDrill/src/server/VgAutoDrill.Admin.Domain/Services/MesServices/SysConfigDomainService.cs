using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class SysConfigDomainService : BaseDomainService<SysConfig>, ISysConfigDomainService
    {
        private readonly ISysConfigRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SysConfigDomainService(IUnitOfWork unitOfWork,
            ISysConfigRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
