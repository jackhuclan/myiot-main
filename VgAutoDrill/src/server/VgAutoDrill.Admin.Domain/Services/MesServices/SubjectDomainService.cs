using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class SubjectDomainService : BaseDomainService<Subject>, ISubjectDomainService
    {
        private readonly ISubjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectDomainService(IUnitOfWork unitOfWork,
            ISubjectRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
