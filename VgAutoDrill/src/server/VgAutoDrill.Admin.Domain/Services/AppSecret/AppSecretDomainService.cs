using VgAutoDrill.Admin.Domain.Interfaces.AppSecret;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.AppSecret;

namespace VgAutoDrill.Admin.Domain.Services.AppSecret
{
    public class AppSecretDomainService : BaseDomainService<SysAppSecret>, IAppSecretDomainService
    {
        private readonly IAppSecretRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AppSecretDomainService(IUnitOfWork unitOfWork,
            IAppSecretRepository repository)
        {
            this._repository = repository;
            base._baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

    }
}
