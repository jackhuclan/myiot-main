using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;

namespace VgAutoDrill.Admin.Repository.Interfaces.Equipment
{

    public interface IProBoardTraceRepository : IBaseRepository<TracePanel>
    {
        Task<IPageList<TracePanel>> GetPanelList(GetPanelListReq req);
    }
}
