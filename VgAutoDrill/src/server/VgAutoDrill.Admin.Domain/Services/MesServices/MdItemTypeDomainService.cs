using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class MdItemTypeDomainService : BaseDomainService<ItemType>, IMdItemTypeDomainService
    {
        private readonly IItemTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MdItemTypeDomainService(IUnitOfWork unitOfWork,
            IItemTypeRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
