using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IWorkstationRepository : IBaseRepository<WorkStation>
    {
        Task<IPageList<WorkStationLoadTaskDto>> GetWorkStationLoadTask(GetWorkStationLoadTaskReq req);
    }
}