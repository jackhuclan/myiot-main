using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Mes;

public interface IWorkOrderTaskAdapter
{
    Task<WorkOrderTaskResponse> ApplyNextWorkOrderTask(WorkOrderTaskRequest request);
    Task<BeginTaskResponse> BeginTask(BeginTaskRequest request);
    Task<BeginTaskResponse> BeginTask(string taskId);
    Task<FinishTaskResponse> FinishTask(FinishTaskRequest request);
    Task<Dictionary<string, object?>> GetWorkOrderInfo(string itemCode);
    Task<Dictionary<string, object?>> GetTaskInfo(string deviceId, string itemCode);
    Task<List<WorkOrderTask>> GetPendingTask(List<string> deviceIds);
    Task<Dictionary<string, object?>> GetDrillPath(string deviceId, string beforeDrillPath, string itemCode, string machineSize = "");
    Task<BeginTaskResponse> SetBufferReady(string taskId);
}
