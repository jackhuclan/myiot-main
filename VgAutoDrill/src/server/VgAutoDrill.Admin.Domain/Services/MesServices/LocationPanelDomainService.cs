using AutoMapper;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class LocationPanelDomainService : BaseDomainService<ScheduleLocationPanel>, ILocationPanelDomainService
    {
        private readonly ILocationPanelRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationPanelDomainService(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILocationPanelRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

    }
}
