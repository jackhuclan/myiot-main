using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Mes.Interface;

public interface ITransferJobAdapter
{
    /// <summary>
    /// 查询料仓转运任务
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<List<TransferJob>> GetTransportations(QueryTransportationRequest request);

    Task<List<TransferJob>> GetTodoTransferRacks();

    /// <summary>
    /// 更新状态
    /// </summary>
    /// <param name="request"></param>
    /// <param name="scheduledTaskStatus"></param>
    /// <returns></returns>
    Task<ServiceResponse> UpdateStatus(ServiceRequest request, ScheduledTaskStatus scheduledTaskStatus);

    Task<bool> TryCancel(string code, string reason);

    /// <summary>
    /// 新建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> Create(TransferJob input);

    Task<int> RefreshTimeoutTransportationTask();

    Task<string> Update(TransferJob input);

    Task<TransferJob?> FindTransferJobByCode(string jobCode);

    Task<TransferJob> QueryByID(long id);

    Task<string> CreateManualJob(TransferJob input);
}
