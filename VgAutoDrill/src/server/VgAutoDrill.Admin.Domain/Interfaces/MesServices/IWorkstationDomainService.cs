using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IWorkstationDomainService : IBaseDomainService<WorkStation>
    {
        Task<PageDto<WorkStationLoadTaskDto>> GetWorkStationLoadTask(GetWorkStationLoadTaskReq req);
    }
}
