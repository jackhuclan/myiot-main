using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class WorkstationDomainService : BaseDomainService<WorkStation>, IWorkstationDomainService
    {
        private readonly IWorkstationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkstationDomainService(IUnitOfWork unitOfWork,
            IWorkstationRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<WorkStationLoadTaskDto>> GetWorkStationLoadTask(GetWorkStationLoadTaskReq req)
        {
            var pageDto = new PageDto<WorkStationLoadTaskDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetWorkStationLoadTask(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<WorkStationLoadTaskDto>>();
            return pageDto;
        }
    }
}
