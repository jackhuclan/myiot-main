using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ItemDrillFileDetailDomainService : BaseDomainService<ItemDrillFileDetail>, IItemDrillFileDetailDomainService
    {
        private readonly IItemDrillFileDetailRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ItemDrillFileDetailDomainService(IUnitOfWork unitOfWork,
            IItemDrillFileDetailRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}