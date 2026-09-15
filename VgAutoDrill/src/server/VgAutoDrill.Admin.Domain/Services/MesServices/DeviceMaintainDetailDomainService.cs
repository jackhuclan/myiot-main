using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DeviceMaintainDetailDomainService : BaseDomainService<DeviceMaintainDetail>, IDeviceMaintainDetailDomainService
    {
        private readonly IDeviceMaintainDetailRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceMaintainDetailDomainService(IUnitOfWork unitOfWork,
            IDeviceMaintainDetailRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}

