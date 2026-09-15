
using VgAutoDrill.Admin.Domain.Interfaces.Position;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Position;

namespace VgAutoDrill.Admin.Domain.Services.Position
{
    public class PositionDomainService : BaseDomainService<SysPosition>, IPositionDomainService
    {
        private readonly IPositionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PositionDomainService(IUnitOfWork unitOfWork,
            IPositionRepository repository)
        {
            this._repository = repository;
            base._baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

    }
}
