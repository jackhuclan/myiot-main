using Mapster;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Domain.Services.MesServices
{
    public class DevicePanelHistoryDomainService : BaseDomainService<DevicePanelHistory>, IDevicePanelHistoryDomainService
    {
        private readonly IDevicePanelHistoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DevicePanelHistoryDomainService(IUnitOfWork unitOfWork,
            IDevicePanelHistoryRepository repository)
        {
            _repository = repository;
            _baseRepository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PageDto<DevicePanelHistoryDto>> PageList(GetDevicePanelHistoryListReq req)
        {
            var pageDto = new PageDto<DevicePanelHistoryDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<DevicePanelHistoryDto>>();
            return pageDto;
        }

        public async Task<List<DevicePanelHistoryDto>> GetListByPanel(List<string?> panelCodeList, GetPanelListReq req)
        {
            var result = await _repository.GetListByPanel(panelCodeList, req);
            return result.ToList().Adapt<List<DevicePanelHistoryDto>>();
        }
    }
}
