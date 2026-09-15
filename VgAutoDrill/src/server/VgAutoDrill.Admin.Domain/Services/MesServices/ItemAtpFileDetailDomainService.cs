using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class ItemAtpFileDetailDomainService : BaseDomainService<ItemAtpFileDetail>, IItemAtpFileDetailDomainService
    {
        private readonly IItemAtpFileDetailRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ItemAtpFileDetailDomainService(IUnitOfWork unitOfWork,
            IItemAtpFileDetailRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
    }
}
