using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class NotiyDomainService : BaseDomainService<Notify>, INotifyDomainService
    {
        private readonly INotifyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public NotiyDomainService(IUnitOfWork unitOfWork,
            INotifyRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
