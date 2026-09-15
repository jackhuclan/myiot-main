using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IDevicePanelHistoryDomainService : IBaseDomainService<DevicePanelHistory>
    {
        Task<PageDto<DevicePanelHistoryDto>> PageList(GetDevicePanelHistoryListReq req);

        Task<List<DevicePanelHistoryDto>> GetListByPanel(List<string?> panelCodeList, GetPanelListReq req);
    }
}
