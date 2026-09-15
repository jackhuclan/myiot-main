using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Central.Core.Manager;

public interface ITransferPlanManager
{
    IReadOnlyList<TransferJob> TransferJobs { get; }

    /// <summary>
    /// pull data from db
    /// </summary>
    /// <returns></returns>
    Task Refresh();

    Task<ForkScheduleTask?> TryReleaseTransferJobAsync(string? partitionCode);

    Task<bool> TryAddTransferJob(TransferJob trackJob);

    Task<ServiceResponse> UpdateStatus(ServiceRequest request, ScheduledTaskStatus scheduledTaskStatus);

    Task<AddTranserJobResponse> AddTransferJobLog(AddTranserJobLogRequest request);

    Task Refresh(string jobCode);

    Task<bool> TryFindAnyNotReadyFork(string? partCode);

    bool TryGetAgvTask(string taskCode, out TransferJob? hikAgvTask);

    Task<bool> TryUpdateTransferJob(TransferJob trackJob);

    Task<bool> TryAddManualTransferJob(TransferJob trackJob);

    /// <summary>
    /// 获取转运TransferJob任务相关的区域
    /// </summary>
    /// <param name="transferJob"></param>
    /// <returns></returns>
    IReadOnlyList<Partition> GetRelatedPartitions(TransferJob transferJob);

    /// <summary>
    /// 获取转运TransferJob任务相关的库位
    /// </summary>
    /// <param name="transferJob"></param>
    /// <returns></returns>
    IReadOnlyList<Location> GetRelatedLocations(TransferJob transferJob);

    /// <summary>
    /// 可转入的生料汇总
    /// </summary>
    /// <param name="partitionCode"></param>
    /// <returns></returns>
    IReadOnlyList<UndrilledItemSummary> PartitionUndrilledItemSummaries(string partitionCode);

    public Task<bool> TryGetTransByItemCode(string itemCode, InteractionSequence interactionSequence, TransportationKind transportationKind);
    bool HasNotReadyPartitionFork(string? partCode);
}
