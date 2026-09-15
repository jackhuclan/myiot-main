using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class MesProcessDomainService : BaseDomainService<Process>, IProcessDomainService
    {
        private readonly IMesProcessRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MesProcessDomainService(IUnitOfWork unitOfWork,
            IMesProcessRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
