using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Repository.Interfaces.Equipment
{

    public interface ITaskRepository : IBaseRepository<Model.Entites.Mes.WorkTask>
    {
        Task<IPageList<TaskDto>> GetList(GetTaskListReq req);

        Task<List<WorkTask>> GetDrillTaskList(GetDrillTaskReq req);

        Task<RouteInfoAndProcessInfo> GetRouteAndProcessList(GetRouteAndProcessByItemReq req);

        Task<IPageList<Model.Entites.Mes.WorkTask>> GetEquipmentList(GetTaskListReq req);

        Task<IPageList<FitWorkStationDto>> GetFitWorkStationList(GetFitWorkStationListReq req);

        Task<IPageList<Model.Entites.Mes.WorkTask>> GetTaskByDevice(GetDrillOrAgvDeviceInfoReq req);
    }
}
