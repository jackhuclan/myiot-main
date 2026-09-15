using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Equipment;

namespace VgAutoDrill.Admin.Domain.Services.Equipment
{
    public class ProBoardTraceDomainService : BaseDomainService<TracePanel>, IProBoardTraceDomainService
    {
        private readonly IProBoardTraceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ProBoardTraceDomainService(IUnitOfWork unitOfWork,
            IProBoardTraceRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<PanelDto>> GetPanelList(GetPanelListReq req)
        {
            var pageDto = new PageDto<PanelDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetPanelList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<PanelDto>>();
            return pageDto;
        }
    }
}
