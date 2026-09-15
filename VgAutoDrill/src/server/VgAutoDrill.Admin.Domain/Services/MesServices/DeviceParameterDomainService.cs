using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class DeviceParameterDomainService : BaseDomainService<DeviceParameter>, IDeviceParameterDomainService
    {
        private readonly IDeviceParameterRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceParameterDomainService(IUnitOfWork unitOfWork,
            IDeviceParameterRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
