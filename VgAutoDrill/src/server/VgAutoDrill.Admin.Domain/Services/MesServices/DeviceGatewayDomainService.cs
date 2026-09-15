using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceGatewayDomainService : BaseDomainService<DeviceGateway>, IDeviceGatewayDomainService
    {
        private readonly IDeviceGatewayRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeviceGatewayDomainService(
            IUnitOfWork unitOfWork,
          IDeviceGatewayRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _baseRepository = repository;
        }
    }
}
