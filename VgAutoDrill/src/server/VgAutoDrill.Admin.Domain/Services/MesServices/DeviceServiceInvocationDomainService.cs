using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceServiceInvocationDomainService : BaseDomainService<DeviceServiceInvocation>, IDeviceServiceInvocationDomainService
    {
        private readonly IDeviceServiceInvocationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeviceServiceInvocationDomainService(IUnitOfWork unitOfWork,
            IDeviceServiceInvocationRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
