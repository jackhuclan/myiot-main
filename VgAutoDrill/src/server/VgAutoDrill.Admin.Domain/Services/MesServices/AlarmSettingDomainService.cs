using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class AlarmSettingDomainService : BaseDomainService<AlarmSetting>, IAlarmSettingDomainService
    {
        private readonly IAlarmSettingRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AlarmSettingDomainService(IUnitOfWork unitOfWork,
            IAlarmSettingRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
