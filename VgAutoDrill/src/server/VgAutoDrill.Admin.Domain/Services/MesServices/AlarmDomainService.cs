using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class AlarmDomainService : BaseDomainService<Alarm>, IAlarmDomainService
    {
        private readonly IAlarmRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AlarmDomainService(IUnitOfWork unitOfWork,
            IAlarmRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AlarmToExcelDto>> GetToExcelList(GetAlarmListReq req)
        {
            List<AlarmToExcelDto> list = await _repository.GetToExcelList(req);
            return list;
        }
    }
}
