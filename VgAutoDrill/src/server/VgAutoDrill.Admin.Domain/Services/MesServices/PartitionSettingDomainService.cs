using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class PartitionSettingDomainService : BaseDomainService<PartitionSetting>, IPartitionSettingDomainService
    {
        private readonly IPartitionSettingRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PartitionSettingDomainService(IUnitOfWork unitOfWork,
            IPartitionSettingRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

    }
}
