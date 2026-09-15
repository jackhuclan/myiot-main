using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class CheckRecordsDomainService : BaseDomainService<CheckRecords>, ICheckRecordsDomainService
    {
        private readonly ICheckRecordsRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CheckRecordsDomainService(IUnitOfWork unitOfWork,
            ICheckRecordsRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
