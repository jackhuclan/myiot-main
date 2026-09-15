using VgAutoDrill.Admin.Domain.Interfaces.Menu;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Menu;

namespace VgAutoDrill.Admin.Domain.Services.Menu
{
    public class MenuDomainService : BaseDomainService<SysMenu>, IMenuDomainService
    {
        private readonly IMenuRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MenuDomainService(IUnitOfWork unitOfWork,
            IMenuRepository repository)
        {
            this._repository = repository;
            base._baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

    }
}
