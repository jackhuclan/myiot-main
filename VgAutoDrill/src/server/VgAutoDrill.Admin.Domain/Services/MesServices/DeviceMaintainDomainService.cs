using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceMaintainDomainService : BaseDomainService<DeviceMaintain>, IDeviceMaintainDomainService
    {
        private readonly IDeviceMaintainRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceMaintainDomainService(IUnitOfWork unitOfWork,
            IDeviceMaintainRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<DeviceMaintainToExcelDto>> GetToExcelList()
        {
            List<DeviceMaintainToExcelDto> list = await _repository.GetToExcelList();
            return list;
        }
    }
}
