using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface ITaskDomainService : IBaseDomainService<Model.Entites.Mes.WorkTask>
    {
        Task<PageDto<TaskDto>> GetList(GetTaskListReq req);

        //Task<PageDto<WorkTask>> List(GetTaskListReq req);

        Task<List<WorkTask>> GetDrillTaskList(GetDrillTaskReq req);

        Task<RouteInfoAndProcessInfo> GetRouteAndProcessList(GetRouteAndProcessByItemReq req);

        Task<PageDto<TaskDto>> GetEquipmentList(GetTaskListReq req);

        Task<PageDto<FitWorkStationDto>> GetFitWorkStationList(GetFitWorkStationListReq req);

        Task<PageDto<TaskDto>> GetTaskByDevice(GetDrillOrAgvDeviceInfoReq req);

        Task<TaskDto> GetTaskByCode(string code);
    }
}
