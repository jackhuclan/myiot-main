using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceTypeDomainService : BaseDomainService<DeviceType>, IDeviceTypeDomainService
    {
        private readonly IDeviceTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeviceTypeDomainService(IUnitOfWork unitOfWork,
            IDeviceTypeRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
