using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IDevicePanelHistoryRepository : IBaseRepository<DevicePanelHistory>
    {
        Task<IPageList<DevicePanelHistory>> GetList(GetDevicePanelHistoryListReq req);

        Task<List<DevicePanelHistory>> GetListByPanel(List<string?> panelCodeList, GetPanelListReq req);
    }
}
