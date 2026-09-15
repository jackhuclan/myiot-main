using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class PartitionDomainService : BaseDomainService<Partition>, IPartitionDomainService
    {
        private readonly IPartitionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PartitionDomainService(IUnitOfWork unitOfWork,
            IPartitionRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Partition> QueryWithSettingByID(object objId)
        {
            return await _repository.QueryWithSettingByID(objId);
        }

        public async Task<bool> AddOrUpdateWithSetting(Partition data)
        {
            return await _repository.AddOrUpdateWithSetting(data);
        }
    }
}
