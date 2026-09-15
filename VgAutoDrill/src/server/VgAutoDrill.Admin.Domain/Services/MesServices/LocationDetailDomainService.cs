using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class LocationDetailDomainService : BaseDomainService<LocationDetail>, ILocationDetailDomainService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocationDetailRepository _repository;

        public LocationDetailDomainService(IUnitOfWork unitOfWork,
            ILocationDetailRepository repository) : base()
        {
            _baseRepository = repository;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<LocationDetail>> GetByCodeAsync(string code)
        {
            return await _repository.GetByCodeAsync(code);
        }

        public async Task<List<(LocationDetail LocationDetail, string SiloCode)>> GetByCodeWithSiloCodeAsync(string code)
        {
            return await _repository.GetByCodeWithSiloCodeAsync(code);
        }
    }
}
