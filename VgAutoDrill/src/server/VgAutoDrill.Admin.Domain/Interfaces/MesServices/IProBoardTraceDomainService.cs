using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IProBoardTraceDomainService : IBaseDomainService<TracePanel>
    {
        Task<PageDto<PanelDto>> GetPanelList(GetPanelListReq req);
    }
}
