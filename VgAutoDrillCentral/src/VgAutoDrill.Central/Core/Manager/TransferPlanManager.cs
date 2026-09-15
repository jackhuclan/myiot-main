using System.Collections.Concurrent;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule.Summary;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Fundation.Utils;
using Partition = VgAutoDrill.Central.Core.Mes.Model.Partition;
using TransferJob = VgAutoDrill.Central.Core.Domain.TransferJob;

namespace VgAutoDrill.Central.Core.Manager;

public class TransferPlanManager : ITransferPlanManager
{
    private readonly ILocationManager _locationManager;
    private readonly ILogger<TransferPlanManager> _logger;
    private readonly ITransferJobAdapter _transferJobAdapter;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ConcurrentDictionary<string, TransferJob> _transferJobs = new();
    private readonly IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask> _panelSiloForkManager;
    private readonly IAlarmLogManager _alarmLogManager;
    private readonly ITransferJobLogService _transferJobLogService;
    private readonly ITransferJobListener _transferJobListener;
    private readonly IMapper _mapper;

    public TransferPlanManager(IServiceProvider serviceProvider)
    {
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _transferJobAdapter = serviceProvider.GetRequiredService<ITransferJobAdapter>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _panelSiloForkManager = serviceProvider.GetRequiredService<IPanelSiloRackManager<PanelSiloFork, ForkScheduleTask>>();
        _alarmLogManager = serviceProvider.GetRequiredService<IAlarmLogManager>();
        _transferJobLogService = serviceProvider.GetRequiredService<ITransferJobLogService>();
        _transferJobListener = serviceProvider.GetRequiredService<ITransferJobListener>();
        _mapper = serviceProvider.GetRequiredService<IMapper>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<TransferPlanManager>();
    }

    public IReadOnlyList<TransferJob> TransferJobs => _transferJobs.Values.ToList().AsReadOnly();

    public IReadOnlyList<ScheduledTaskStatus?> ScheduledTaskStatusList => new List<ScheduledTaskStatus?>()
        {
            ScheduledTaskStatus.Created,
            ScheduledTaskStatus.Allocated,
            ScheduledTaskStatus.Running,
        }.AsReadOnly();

    public async Task Refresh()
    {
        var transferJobs = await _transferJobAdapter.GetTodoTransferRacks();

        foreach (var job in transferJobs)
        {
            AddOrUpdateTransferJob(job);
        }

        var expiredJobs = _transferJobs.Values.Where(old => old.CreateTime < DateTime.Now.AddDays(-1))
                        .Select(x => x.Code)
                        .ToList();
        foreach (var old in expiredJobs)
        {
            _transferJobs.TryRemove(old, out _);
        }

        //料仓任务已结束后，释放对应的原有库位
        foreach (var forkSchedule in _scheduleTaskManager.NotStartedForkSchedules.Where(x => x.Appointed))
        {
            if (!_transferJobs.Values.Any(x => x.IsNotCompleted
                    && x.ForkCode == forkSchedule.LocationCode))
            {
                forkSchedule.Appointed = false;
            }
        }

        var scheduledTaskStatusList = new List<ScheduledTaskStatus?>
        {
            ScheduledTaskStatus.Created,
            ScheduledTaskStatus.Allocated,
            ScheduledTaskStatus.Running,
        };

        foreach (var forkOrShelfSchedule in _scheduleTaskManager.NotStartedForkOrShelfSchedules.Where(x => x.Appointed))
        {
            if (!_transferJobs.Values.Any(x => x.IsNotCompleted
             && (x.StartLocationCode == forkOrShelfSchedule.LocationCode || x.EndLocationCode == forkOrShelfSchedule.LocationCode)))
            {
                forkOrShelfSchedule.Appointed = false;
            }
        }

        foreach (var pinSchedule in _scheduleTaskManager.NotStartedPinSchedules.Where(x => x.Appointed))
        {
            if (!_transferJobs.Values.Any(x => x.IsNotCompleted
             && (x.StartLocationCode == pinSchedule.LocationCode || x.EndLocationCode == pinSchedule.LocationCode)))
            {
                pinSchedule.Appointed = false;
            }
        }

        foreach (var unPinSchedule in _scheduleTaskManager.NotStartedUnpinSchedules.Where(x => x.Appointed))
        {
            if (!_transferJobs.Values.Any(x => x.IsNotCompleted
             && (x.StartLocationCode == unPinSchedule.LocationCode || x.EndLocationCode == unPinSchedule.LocationCode)))
            {
                unPinSchedule.Appointed = false;
            }
        }

        foreach (var todoTrans in _transferJobs.Values.Where(x => scheduledTaskStatusList.Contains(x.ScheduledTaskStatus)))
        {
            //已经取消或者删除了的料仓任务， 同步内存中的任务的状态
            var dbTran = await _transferJobAdapter.QueryByID(todoTrans.Id);
            if (dbTran == null)
            {
                todoTrans.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
                _transferJobs.TryRemove(todoTrans.Code, out _);
                continue;
            }
            else if (!dbTran.IsNotCompleted)
            {
                todoTrans.ScheduledTaskStatus = dbTran.ScheduledTaskStatus;
                continue;
            }

            //对应的钻机请求已被取消
            if (todoTrans.MasterScheduleId > 0
                && string.IsNullOrEmpty(todoTrans.StartDeviceId)
                && string.IsNullOrEmpty(todoTrans.EndDeviceId)
                && todoTrans.InteractionSequence == InteractionSequence.LoadOnly
                && todoTrans.ScheduledTaskStatus == ScheduledTaskStatus.Created
                && !_scheduleTaskManager.NotStartedDrillSchedules.Any(s => s.Id == todoTrans.MasterScheduleId
                && (string.IsNullOrEmpty(todoTrans.RelatedDrillScheduleIds) || todoTrans.RelatedDrillScheduleIds.Contains(s.Id.ToStr()))))
            {
                if (await _transferJobAdapter.TryCancel(todoTrans.Code, "对应的钻机请求已被取消"))
                {
                    todoTrans.IsCancel = true;
                    var forkSchedule = _scheduleTaskManager.NotStartedForkSchedules.FirstOrDefault(x => x.InteractionSequence == Fundation.Iot.Models.InteractionSequence.LoadOnly && x.LocationCode == todoTrans.ForkCode);
                    if (forkSchedule != null)
                    {
                        forkSchedule.Appointed = false;
                    }
                }
            }

            //手动创建的任务，标注叉齿的调度为已appointed
            if (todoTrans.MasterScheduleId is null)
            {
                var forkSchedule = _scheduleTaskManager.NotStartedForkSchedules.FirstOrDefault(x => x.LocationCode == todoTrans.ForkCode && x.Code == todoTrans.ForkLocationScheduleId && !x.Appointed);
                if (forkSchedule != null)
                {
                    forkSchedule.Appointed = true;
                    forkSchedule.AppointedMessage = "已创建了料仓任务";
                }
            }
            var list = new List<ScheduledTaskStatus?> { ScheduledTaskStatus.Created, ScheduledTaskStatus.Running };

            if (list.Contains(todoTrans.ScheduledTaskStatus)
                && _scheduleTaskManager.NotStartedForkSchedules.Any(x => x.LocationCode == todoTrans.ForkCode && x.Code == todoTrans.ForkLocationScheduleId && !x.Appointed))
            {
                var forkSchedule = _scheduleTaskManager.NotStartedForkSchedules.FirstOrDefault(x => x.LocationCode == todoTrans.ForkCode && x.Code == todoTrans.ForkLocationScheduleId && !x.Appointed);
                if (forkSchedule != null)
                {
                    forkSchedule.Appointed = true;
                    forkSchedule.AppointedMessage = "已创建了料仓任务";
                }
            }
        }
    }

    public async Task<ForkScheduleTask?> TryReleaseTransferJobAsync(string? partitionCode)
    {
        ForkScheduleTask? emptyForkSchedule = null;

        var transferInTask = TransferJobs.FirstOrDefault(x => x.PartitionCode == partitionCode
            && x.InteractionSequence == Fundation.Iot.Models.InteractionSequence.LoadOnly
            && x.ScheduledTaskStatus == Fundation.Iot.Schedule.ScheduledTaskStatus.Created);

        if (transferInTask != null)
        {
            emptyForkSchedule = _scheduleTaskManager.NotStartedForkSchedules.FirstOrDefault(x => x.LocationCode == transferInTask.ForkCode && x.InteractionSequence == Fundation.Iot.Models.InteractionSequence.LoadOnly);

            if (emptyForkSchedule != null)
            {
                if (await _transferJobAdapter.TryCancel(transferInTask.Code, "库位已用于大车更换料仓"))
                {
                    emptyForkSchedule.Appointed = false;
                }
            }

            return emptyForkSchedule;
        }

        return null;
    }

    public async Task<bool> TryAddTransferJob(TransferJob trackJob)
    {
        trackJob.Code = Guid.NewGuid().ToString();
        trackJob.ScheduledTaskStatus = ScheduledTaskStatus.Created;
        AddOrUpdateTransferJob(trackJob);

        var responseMsg = await _transferJobAdapter.Create(trackJob);
        _logger.LogInformation($"_transferJobAdapter.Create, input {trackJob}, response {responseMsg}");
        var succeeded = string.IsNullOrEmpty(responseMsg);
        if (succeeded)
        {
            await Refresh(trackJob.Code);
        }
        else
        {
            _transferJobs.TryRemove(trackJob.Code, out var _);
        }

        return succeeded;
    }

    public async Task Refresh(string jobCode)
    {
        var job = await _transferJobAdapter.FindTransferJobByCode(jobCode);
        if (job == null) return;
        AddOrUpdateTransferJob(job);
    }

    public async Task<ServiceResponse> UpdateStatus(ServiceRequest request, ScheduledTaskStatus scheduledTaskStatus)
    {
        if (string.IsNullOrEmpty(request.TraceId))
        {
            return new ServiceResponse
            {
                Code = ErrorCodes.Sys.MISSING_TRACE_ID_CODE,
                Message = ErrorCodes.Sys.MISSING_TRACE_ID_MESSAGE
            };
        }
        var transDto = TransferJobs.FirstOrDefault(x => x.Code == request.TraceId);
        if (transDto == null)
        {
            return new ServiceResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"trace id:{request.TraceId},cannot find this transferJob.",
            };
        }

        var response = await _transferJobAdapter.UpdateStatus(request, scheduledTaskStatus);

        if (response.Code == ErrorCodes.Sys.SUCCESS)
        {
            if (scheduledTaskStatus == ScheduledTaskStatus.Canceled
                || scheduledTaskStatus == ScheduledTaskStatus.Failed)
            {
                MakeForkSchedulable(transDto);
            }
            else
            {
                transDto.ScheduledTaskStatus = scheduledTaskStatus;
            }
        }

        return response;
    }

    /// <summary>
    /// 使插齿调度恢复可被调度状态
    /// </summary>
    /// <param name="transDto"></param>
    private void MakeForkSchedulable(TransferJob transDto)
    {
        transDto.ScheduledTaskStatus = ScheduledTaskStatus.Created;
        var appointedForkSchedules = _scheduleTaskManager.Tasks.Where(x => x.Id == transDto.StartScheduleId || x.Id == transDto.EndScheduleId);
        foreach (var item in appointedForkSchedules)
        {
            item.Appointed = false;
        }
    }

    public IReadOnlyList<Partition> GetRelatedPartitions(TransferJob transferJob)
    {
        return (transferJob.RelatedDrillScheduleIds ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x =>
          {
              if (_scheduleTaskManager.TryGetScheduleTaskById(Convert.ToInt64(x), out var scheduledTask)
              && scheduledTask != null
              && !string.IsNullOrWhiteSpace(scheduledTask.LocationCode)
              && _locationManager.TryGetLocation(scheduledTask.LocationCode, out var location)
              && location != null)
              {
                  return location.Partition;
              }

              return null;
          }).Where(x => x != null).Select(x => x!).ToList().AsReadOnly();
    }

    public IReadOnlyList<Location> GetRelatedLocations(TransferJob transferJob)
    {
        return (transferJob.RelatedDrillScheduleIds ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x =>
        {
            if (_scheduleTaskManager.TryGetScheduleTaskById(Convert.ToInt64(x), out var scheduledTask)
            && scheduledTask != null
            && !string.IsNullOrWhiteSpace(scheduledTask.LocationCode)
            && _locationManager.TryGetLocation(scheduledTask.LocationCode, out var location))
            {
                return location;
            }

            return null;
        }).Where(x => x != null).Select(x => x!).ToList().AsReadOnly();
    }

    private void AddOrUpdateTransferJob(TransferJob job)
    {
        _ = _transferJobs.AddOrUpdate(job.Code,
                            (code) =>
                            {
                                job.OnStatusChanged += (t) => _transferJobListener.OnStatusChanged(t);
                                return job;
                            },
                            (code, oldJob) =>
                            {
                                if (oldJob.ScheduledTaskStatus != ScheduledTaskStatus.Created
                                    && job.ScheduledTaskStatus == ScheduledTaskStatus.Created)
                                {
                                    _logger.LogWarning($"重新加载料仓任务时，任务状态异常，db状态：{job.ScheduledTaskStatus}，已修正为内存状态：{oldJob.ScheduledTaskStatus.ToString()}；");
                                    _logger.LogWarning($"重新加载料仓任务时，任务状态异常，db HikResponseKey：{job.HikResponseKey}，已修正为：{oldJob.HikResponseKey}；");

                                    job.ScheduledTaskStatus = oldJob.ScheduledTaskStatus;
                                    job.HikResponseKey = oldJob.HikResponseKey;
                                }

                                //override oldTask's properties with new task's properties
                                var convertedTask = _mapper.Map(job, oldJob);
                                return convertedTask;
                            });
    }

    public bool HasNotReadyPartitionFork(string? partCode) => _panelSiloForkManager.PartitionEnabledLocations(partCode)
            .Any(x => x.Schedule == null || x.Schedule.IsCompleted);

    public async Task<bool> TryFindAnyNotReadyFork(string? partCode)
    {
        if (HasNotReadyPartitionFork(partCode))
        {
            var notReadyForks = _panelSiloForkManager.PartitionEnabledLocations(partCode)
                .Where(x => x.Schedule == null || x.Schedule.IsCompleted)
                .OrderBy(x => x.Code)
                .ToList();

            _logger.LogWarning($"中转区[{partCode}]未发出调度申请，不能计算生料转入、空仓转入的任务！请尽快检查库位： {JsonSerializer.Serialize(notReadyForks.Select(x => x.Code))}.");

            _ = _alarmLogManager.AddLogAsync(new AlarmLog
            {
                AlarmCode = $"NotReadyFork-{partCode}",
                AlarmTime = DateTime.Now,
                AlarmName = $"中转区[{partCode}]未发出调度申请，不能计算生料转入、空仓转入的任务！请尽快检查库位： {JsonSerializer.Serialize(notReadyForks.Select(x => x.Code))}."
            });

            return true;
        }
        else
        {
            _ = await _alarmLogManager.ResetAlarm($"NotReadyFork-{partCode}");
        }

        return false;
    }

    public async Task<AddTranserJobResponse> AddTransferJobLog(AddTranserJobLogRequest request)
    {
        var response = await _transferJobLogService.Add(new AddOrUpdateTransferJobLogReq
        {
            MasterId = request.TransferJobId,
            Message = request.Message,
        });

        if (response == null)
        {
            return new AddTranserJobResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"写入日志失败,TransferJobId:{request.TransferJobId},原因：response is null."
            };
        }
        else if (!string.IsNullOrEmpty(response.Message))
        {
            return new AddTranserJobResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"写入日志失败,TransferJobId:{request.TransferJobId},原因：{response.Message}"
            };
        }

        return new AddTranserJobResponse
        {
            TransferJobId = request.TransferJobId,
            DeviceId = request.DeviceId,
            Code = ErrorCodes.Sys.SUCCESS,
            Message = ""
        };
    }

    public async Task<bool> TryUpdateTransferJob(TransferJob trackJob)
    {
        var responseMsg = await _transferJobAdapter.Update(trackJob);
        _logger.LogInformation($"_transferJobAdapter.Update, input {trackJob}, response {responseMsg}");
        await Refresh(trackJob.Code);
        return string.IsNullOrEmpty(responseMsg);
    }

    public bool TryGetAgvTask(string taskCode, out TransferJob? hikAgvTask)
    {
        _ = _transferJobs.TryGetValue(taskCode, out hikAgvTask);
        return hikAgvTask != null;
    }

    public async Task<bool> TryAddManualTransferJob(TransferJob trackJob)
    {
        var responseMsg = await _transferJobAdapter.CreateManualJob(trackJob);
        _logger.LogInformation($"TryAddManualTransferJob.Create, input {JsonSerializer.Serialize(trackJob)}, response {responseMsg}");
        var succeeded = string.IsNullOrEmpty(responseMsg);
        if (succeeded)
        {
            await Refresh(trackJob.Code);
        }

        return succeeded;
    }

    public IReadOnlyList<UndrilledItemSummary> PartitionUndrilledItemSummaries(string partitionCode) =>
        TransferJobs.Where(x => x.TransportationKind == TransportationKind.Raw
        && !string.IsNullOrWhiteSpace(x.InternalLotNo)
        && x.IsNotCompleted
        && string.Equals(x.PartitionCode, partitionCode, StringComparison.OrdinalIgnoreCase))
        .GroupBy(x => x.InternalLotNo)
        .Select(x => new UndrilledItemSummary
        {
            ItemCode = x.Key!,
            ItemCount = x.Sum(k => k.RawCount ?? 0)
        })
        .ToList()
        .AsReadOnly();

    public async Task<bool> TryGetTransByItemCode(string itemCode, InteractionSequence InteractionSequence, TransportationKind TransportationKind)
    {
        if (_transferJobs.Values.Any(x => x.InternalLotNo == itemCode && ScheduledTaskStatusList.Contains(x.ScheduledTaskStatus)
                                                                      && x.InteractionSequence == InteractionSequence
                                                                      && x.TransportationKind == TransportationKind))
        {
            return true;
        }

        return false;
    }
}
