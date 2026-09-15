using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class CutterGroupDomainService : BaseDomainService<Model.Entites.Mes.CutterGroup>, ICutterGroupDomainService
    {
        private readonly ICutterGroupRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CutterGroupDomainService(IUnitOfWork unitOfWork,
            ICutterGroupRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }



    }
}
