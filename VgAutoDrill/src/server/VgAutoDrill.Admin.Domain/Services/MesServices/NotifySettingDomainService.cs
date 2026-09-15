using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class NotifySettingDomainService : BaseDomainService<NotifySetting>, INotifySettingDomainService
    {
        private readonly INotifySettingRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public NotifySettingDomainService(IUnitOfWork unitOfWork,
            INotifySettingRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
