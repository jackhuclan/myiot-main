using VgAutoDrill.Admin.Domain.Services;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public class DeviceRecordsSummaryExtendDomainService : BaseDomainService<DeviceRecordsSummaryExtend>, IDeviceRecordsSummaryExtendDomainService
    {
        private readonly IDeviceRecordsSummaryExtendRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceRecordsSummaryExtendDomainService(IUnitOfWork unitOfWork,
            IDeviceRecordsSummaryExtendRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }


        public async Task<List<DeviceRecordsSummaryExtend>> GetListByDeviceCode(string deviceCode)
        {
            return await _repository.GetListByDeviceCode(deviceCode);
        }


    }
}
