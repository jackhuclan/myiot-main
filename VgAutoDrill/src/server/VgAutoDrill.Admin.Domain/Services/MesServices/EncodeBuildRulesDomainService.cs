using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class EncodeBuildRulesDomainService : BaseDomainService<EncodeBuildRules>, IEncodeBuildRulesDomainService
    {
        private readonly IEncodeBuildRulesRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public EncodeBuildRulesDomainService(IUnitOfWork unitOfWork,
            IEncodeBuildRulesRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
