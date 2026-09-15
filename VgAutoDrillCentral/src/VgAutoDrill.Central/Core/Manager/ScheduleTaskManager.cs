using System.Collections.Concurrent;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Central.Core.Schedule.Logic;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Manager;

public class ScheduleTaskManager : IScheduleTaskManager
{
    private readonly IDeviceManager _deviceManager;
    private readonly IDeviceServiceInvocationLogger _deviceServiceInvocationLogger;
    private readonly ILocationManager _locationManager;
    private readonly ILogger<ScheduleTaskManager> _logger;
    private readonly IMapper _mapper;
    private readonly IScheduleTaskAdapter _scheduleTaskAdapter;
    private readonly IScheduleTaskListener _scheduleTaskListener;
    private readonly TimeoutScheduleTaskLogic _timeoutScheduleTaskHandler;
    private readonly IScheduleLogService _scheduleLogService;
    private readonly IPartitionManager _partitionManager;
    private readonly ConcurrentDictionary<string, ScheduleTaskWithRequest> _scheduleTasks = new();
    private readonly IScheduleService _scheduleService;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleTracker _scheduleTracker;
    private readonly IDistributedCache _distributedCache;
    private readonly ILocationAdapter _locationAdapter;
    private readonly IPanelBarCodeValidator _panelBarCodeValidator;

    public ScheduleTaskManager(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _logger = serviceProvider.GetRequiredService<ILogger<ScheduleTaskManager>>();
        _scheduleTaskAdapter = serviceProvider.GetRequiredService<IScheduleTaskAdapter>();
        _deviceServiceInvocationLogger = serviceProvider.GetRequiredService<IDeviceServiceInvocationLogger>();
        _mapper = serviceProvider.GetRequiredService<IMapper>();
        _scheduleTaskListener = serviceProvider.GetRequiredService<IScheduleTaskListener>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _distributedCache = serviceProvider.GetRequiredService<IDistributedCache>();
        _timeoutScheduleTaskHandler = objectFactory.CreateObject<TimeoutScheduleTaskLogic>();
        _scheduleLogService = serviceProvider.GetRequiredService<IScheduleLogService>();
        _partitionManager = serviceProvider.GetRequiredService<IPartitionManager>();
        _scheduleService = serviceProvider.GetRequiredService<IScheduleService>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTracker = serviceProvider.GetRequiredService<IScheduleTracker>();
        _locationAdapter = serviceProvider.GetRequiredService<ILocationAdapter>();
        _panelBarCodeValidator = serviceProvider.GetRequiredService<IPanelBarCodeValidator>();
    }

    public IReadOnlyList<DrillScheduleTask> NotStartedDrillSchedules => NotStartedSchedules
                                                                        .OfType<DrillScheduleTask>()
                                                                        .OrderByDescending(s => s.Percentage)
                                                                        .ThenBy(s => s.Id)
                                                                        .ToList()
                                                                        .AsReadOnly();

    public IReadOnlyList<ForkScheduleTask> NotStartedForkSchedules => NotStartedSchedules
                                                                        .OfType<ForkScheduleTask>()
                                                                        .OrderByDescending(x => x.Id)
                                                                        .ToList()
                                                                        .AsReadOnly();

    public IReadOnlyList<ForkScheduleTask> ForkSchedules => Tasks
                                                            .OfType<ForkScheduleTask>()
                                                            .OrderByDescending(x => x.Id)
                                                            .ToList()
                                                            .AsReadOnly();

    public IReadOnlyList<PinScheduleTask> NotStartedPinSchedules => NotStartedSchedules
                                                                        .OfType<PinScheduleTask>()
                                                                        .OrderByDescending(x => x.Id)
                                                                        .ToList()
                                                                        .AsReadOnly();

    public IReadOnlyList<ScheduleTaskWithRequest> NotStartedSchedules => _scheduleTasks.Values
                                                                        .Where(x => x.IsNotStarted && string.IsNullOrEmpty(x.AllocatedAgv) && !x.IsLocked)
                                                                        .OrderByDescending(x => x.Id)
                                                                        .ToList()
                                                                        .AsReadOnly();

    public IReadOnlyList<ScheduleTaskWithRequest> NotStartedSpecifiedAgvSchedules => _scheduleTasks.Values
                                                                        .Where(x => x.IsNotStarted && !string.IsNullOrEmpty(x.AllocatedAgv))
                                                                        .OrderByDescending(x => x.Id)
                                                                        .ToList()
                                                                        .AsReadOnly();

    public IReadOnlyList<ScheduleTaskWithRequest> AllTodoOrDoingSchedules => _scheduleTasks.Values
                                                                        .Where(x => x.IsNotStarted)
                                                                        .OrderByDescending(x => x.Id)
                                                                        .ToList()
                                                                        .AsReadOnly();

    public IReadOnlyList<ShelfScheduleTask> NotStartedShelfSchedules => NotStartedSchedules
                                                                        .OfType<ShelfScheduleTask>()
                                                                        .OrderByDescending(x => x.Id)
                                                                        .ToList()
                                                                        .AsReadOnly();

    public IReadOnlyList<ScheduleTaskWithRequest> NotStartedForkOrShelfSchedules => NotStartedSchedules
                                                                                .Where(x => new List<DeviceKind>
                                                                                    {
                                                                                        DeviceKind.PanelSiloFork,
                                                                                        DeviceKind.PublicPanelSiloWIP,
                                                                                    }.Contains(x.RequestDeviceKind ?? DeviceKind.Unknown))
                                                                                .OrderByDescending(x => x.Id)
                                                                                .ToList()
                                                                                .AsReadOnly();

    public IReadOnlyList<UnpinScheduleTask> NotStartedUnpinSchedules => NotStartedSchedules
                                                                        .OfType<UnpinScheduleTask>()
                                                                        .OrderByDescending(x => x.Id)
                                                                        .ToList()
                                                                        .AsReadOnly();

    public IReadOnlyList<ScheduleTaskWithRequest> Tasks => _scheduleTasks.Values.ToList().AsReadOnly();

    public async Task<string> AllocateDrillScheduleTask(ScheduleTaskWithRequest schedule, Agv agv, InteractionSequence interactionSequence)
    {
        var changedSpindles = agv.Properties["changedSpindles"].ToStr();
        var changedBehavior = agv.Properties["changedBehavior"].ToStr();
        var taskCode = agv.Properties[ScheduleConstants.PARAMS_TASK_ID].ToStr();
        var itemCode = agv.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr();
        var behavior = Convert.ToUInt16(agv.Properties["RequestInteractionBehavior"].ToStr());
        var agvRemainRawCount = agv.Properties["agvRemainRawCount"].ToInt();
        var taskItemCount = agv.Properties["taskItemCount"].ToInt();
        var remarkForOnlyLoadUndrillItem = agv.Properties["RemarkForOnlyLoadUndrillItem"].ToStr();
        var agvDeviceId = agv.Properties["AgvDeviceId"].ToStr();

        var updateDto = new AddOrUpdateScheduleReq
        {
            Code = schedule.Code,
            RequireDeviceId = agvDeviceId,
            ScheduledTaskStatus = ScheduledTaskStatus.Allocated,
            ChangedSpindles = changedSpindles,
            ChangedBehavior = changedBehavior,
            ItemCode = itemCode,
            TaskId = taskCode,
            RequestInteractionBehavior = behavior,
            InteractionSequence = interactionSequence,
            IsAllPanelSent = agv.Properties["IsAllPanelSent"].ToBool(),
            AGVPayloadPanels = agv.PayloadPanels.SummaryPanelInfo(),
        };

        if (!string.IsNullOrEmpty(changedBehavior))
        {
            updateDto.Remark = $"changedSpindles：{changedSpindles}," +
                $"{Environment.NewLine}changedBehavior：{changedBehavior}," +
                $"{Environment.NewLine}AGV余料:{agvRemainRawCount}，任务数量：{taskItemCount},";
        }

        if (!string.IsNullOrEmpty(remarkForOnlyLoadUndrillItem))
        {
            updateDto.Remark += $"{Environment.NewLine}{remarkForOnlyLoadUndrillItem}";
        }

        var response = await _scheduleTaskAdapter.UpdateDbScheduleTaskTable(updateDto);
        if (string.IsNullOrEmpty(response))
        {
            await Refresh(schedule.Code);

            await _partitionManager.TryUnbookPartition(agv.DeviceId);
        }
        else
        {
            _logger.LogError($"分配调度{schedule.Id}时，保存失败; response:{response}");
        }

        return response;
    }

    public async Task Evict()
    {
        var expiredDays = await GetLoadScheduleDays();

        var expiredTaskCodes = _scheduleTasks.Values.Where(x => DateTime.Now.Subtract(x.CreateTime).Days > expiredDays)
            .Select(x => x.Code)
            .ToList();

        foreach (var taskCode in expiredTaskCodes)
        {
            if (string.IsNullOrEmpty(taskCode))
                continue;

            if (_scheduleTasks.TryRemove(taskCode, out var removedTask))
            {
                removedTask.OnTimeout -= (t) => _timeoutScheduleTaskHandler.OnTimeout(t);
                removedTask.OnStatusChanged -= (t) => _scheduleTaskListener.OnStatusChanged(t);
            }
        }

        await _scheduleService.CancelExpireSchedule();
    }

    public bool HasUncompletedTaskOfRoutingKey(string routingKey)
    {
        var notEndedScheduleStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.Allocated,
                ScheduledTaskStatus.Running,
                ScheduledTaskStatus.PartCompleted,
            };
        var isAny = _scheduleTasks.Values.Any(x => notEndedScheduleStatusList.Contains(x.ScheduledTaskStatus ?? ScheduledTaskStatus.None)
                    && x.RoutingKey == routingKey);
        return isAny;
    }

    public bool HasUncompletedTaskOfAgv(string allocatedAgv, out long[] uncompletedScheduleIds, out string[] uncompletedDeviceIds)
    {
        var notEndedScheduleStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.Allocated,
                ScheduledTaskStatus.Running,
                ScheduledTaskStatus.PartCompleted,
            };

        var tuples = _scheduleTasks.Values.Where(x => notEndedScheduleStatusList.Contains(x.ScheduledTaskStatus ?? ScheduledTaskStatus.None)
                    && x.AllocatedAgv == allocatedAgv).Select(x => new Tuple<long, string>(x.Id, x.CallerDeviceId ?? string.Empty))
                    .ToArray();
        uncompletedScheduleIds = tuples.Select(x => x.Item1).ToArray();
        uncompletedDeviceIds = tuples.Select(x => x.Item2).ToArray();
        return tuples.Any();
    }

    public async Task Refresh()
    {
        var defaultloadScheduleTaskDays = await GetLoadScheduleDays();

        var tasks = await _scheduleTaskAdapter.GetSchedule<ScheduleTaskWithRequest>(new QueryScheduleRequest
        {
            StartTime = DateTime.Now.AddDays(-1 * defaultloadScheduleTaskDays),
        });

        foreach (var task in tasks)
        {
            AddOrUpdateScheduleTask(task);
            if (task.IsCompleted)
            {
                await _distributedCache.RemoveAsync(task.RoutingKey);
            }
        }
    }

    public async Task<string> SetScheduleUrgent(long id)
    {
        var schedule = _scheduleTasks.Values.FirstOrDefault(x => x.Id == id);
        if (schedule == null)
        {
            return $"中控系统中，未查找这个记录，ID：{id}";
        }

        var response = await _scheduleService.SetScheduleUrgent(id);
        if (response != null && response.Code == Admin.Model.ViewModels.ResponseCode.Success)
        {
            await Refresh(schedule.Code);
        }

        return string.Empty;
    }

    private async Task<long> Refresh(string scheduleCode)
    {
        var schedule = await _scheduleTaskAdapter.FindScheduleByTraceId(scheduleCode);
        if (schedule != null)
        {
            AddOrUpdateScheduleTask(schedule);

            return schedule.Id;
        }

        return 0;
    }

    public async Task<DeviceEventReportResponse> ScheduleAGVDevcieViaMysql(DeviceEventReportRequest eventRequest, DeviceProxy callerDevice)
    {
        var routingKey = eventRequest.Params.ContainsKey("routingKey") ? eventRequest.Params["routingKey"].ToStr() : string.Empty;
        if (string.IsNullOrEmpty(routingKey))
        {
            routingKey = $"{eventRequest.ProductId}.{eventRequest.DeviceId}.event.{eventRequest.EventId}".ToLower();
        }

        eventRequest.Params[ScheduleConstants.PARAMS_TASK_EVENT_ID] = eventRequest.EventId;
        var eventTraceId = eventRequest.TraceId;
        eventRequest.Params[ScheduleConstants.PARAMS_TASK_EVENT_NAME] = eventRequest.EventName;

        var convertedInteractionBehavior = (InteractionBehavior)eventRequest.RequestInteractionBehavior;
        var SpindleBehavior = "";
        var taskCode = eventRequest.Params[ScheduleConstants.PARAMS_TASK_ID].ToStr();  //下个待生产的任务代号
        var taskItemCode = eventRequest.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr();

        if (eventRequest.RequestDeviceKind == DeviceKind.UnPin
            && convertedInteractionBehavior.InteractionSequence == InteractionSequence.LoadOnly
            && eventRequest.Params.ContainsKey("UnpinItemCode")
            && !string.IsNullOrEmpty(eventRequest.Params["UnpinItemCode"].ToStr())
            )
        {
            taskItemCode = eventRequest.Params["UnpinItemCode"].ToStr();
        }

        if (eventRequest.Params.ContainsKey("SpindleBehavior"))
        {
            SpindleBehavior = eventRequest.Params["SpindleBehavior"].ToStr();
        }
        var InteractivePosition = "";
        if (eventRequest.Params.ContainsKey("InteractivePosition"))
        {
            InteractivePosition = eventRequest.Params["InteractivePosition"].ToStr();
        }

        //本次申请的生料数量 RawSpindleNum
        var totalRawCount = 0;
        if (DeviceKindExtensions.IsDrill(callerDevice.Descriptor.DeviceKind))
        {
            totalRawCount = eventRequest.Params.ContainsKey("RawSpindleNum") ? eventRequest.Params["RawSpindleNum"].ToInt() : 0;
        }
        else if (callerDevice.Descriptor.DeviceKind == DeviceKind.PanelSiloFork || callerDevice.Descriptor.DeviceKind == DeviceKind.PublicPanelSiloWIP)
        {
            totalRawCount = eventRequest.PayloadPanels.CountUndrilledPanels();
        }

        var scheduleStatus = ScheduledTaskStatus.Created;
        if (eventRequest.Params.ContainsKey("ExistRawNum")
            && eventRequest.Params["ExistRawNum"].ToInt() > 0
            && convertedInteractionBehavior.InteractionSequence != InteractionSequence.UnloadOnly)
        {
            scheduleStatus = ScheduledTaskStatus.PartCompleted;
        }

        var isAuxiliaryDevice = DeviceKindExtensions.IsAuxiliary(callerDevice.DeviceKind);
        bool isAuxiliaryRequest = isAuxiliaryDevice;
        var isForceCall = false;

        //如果request 中 标志为 料架主叫时，可以调整为false
        if (isAuxiliaryDevice
            && eventRequest.Params.ContainsKey("IsAuxiliary")
            && !eventRequest.Params["IsAuxiliary"].ToBool())
        {
            isAuxiliaryRequest = false;
            isForceCall = true;
        }

        if ((callerDevice.DeviceKind == DeviceKind.Pin || callerDevice.DeviceKind == DeviceKind.UnPin)
            && eventRequest.Params.ContainsKey("ForceCall")
            && eventRequest.Params["ForceCall"].ToBool())
        {
            isForceCall = true;
        }

        InteractionSequence interactionSequence = convertedInteractionBehavior.InteractionSequence;
        var location = string.IsNullOrEmpty(eventRequest.LocationCode) ? eventRequest.DeviceId : eventRequest.LocationCode;
        string remark = $"{convertedInteractionBehavior.InteractionSequence.ToString()},{SpindleBehavior},{InteractivePosition}";

        if (DeviceKindExtensions.IsDrill(eventRequest.RequestDeviceKind))
        {
            if (string.IsNullOrEmpty(taskCode)
                && (convertedInteractionBehavior.InteractionSequence == InteractionSequence.LoadThenUnload
                        || convertedInteractionBehavior.InteractionSequence == InteractionSequence.UnloadThenLoad))
            {
                interactionSequence = InteractionSequence.UnloadOnly;
                eventRequest.RequestInteractionBehavior = convertedInteractionBehavior.ChangeSequence(InteractionSequence.UnloadOnly);
                remark += $"{Environment.NewLine}未匹配到合适的生产任务，变更为仅下料请求";
            }
        }

        var req = new AddOrUpdateScheduleReq
        {
            SourceDeviceId = eventRequest.DeviceId,
            EndLocation = eventRequest.Params.ContainsKey("Spindles") ? eventRequest.Params["Spindles"].ToStr() : string.Empty,
            Code = eventTraceId,
            TaskId = taskCode,
            ItemCode = taskItemCode,
            IsAuxiliary = isAuxiliaryRequest,
            RouteCode = eventRequest.Params.ContainsKey(ScheduleConstants.PARAMS_TASK_ROUTE_CODE) ? eventRequest.Params[ScheduleConstants.PARAMS_TASK_ROUTE_CODE].ToStr() : "",
            RequestJson = JsonSerializer.Serialize(eventRequest),
            ScheduledTaskStatus = scheduleStatus,
            IsAllPanelSent = true,
            RoutingKey = routingKey,
            RequestDeviceKind = callerDevice.DeviceKind,
            InteractionSequence = interactionSequence,
            IsMaster = isForceCall,
            Remark = remark,
            TotalRawCount = totalRawCount,
            SubDeviceCode = location
        };

        if (eventRequest.RequestMaterialKind == MaterialKind.PanelSilo)
        {
            req.RequestSummaryInfo = eventRequest.PayloadPanels.SummaryPanelInfo();
        }
        else if (eventRequest.RequestMaterialKind == MaterialKind.Panel)
        {
            req.RequestSummaryInfo = eventRequest.PayloadPanels.SummaryDrillPanelInfo();
        }

        if (isForceCall
            && eventRequest.Params.ContainsKey("AssignAGV")
            && !string.IsNullOrEmpty(eventRequest.Params["AssignAGV"].ToStr()))
        {
            req.RequireDeviceId = eventRequest.Params["AssignAGV"].ToStr();
        }

        await _scheduleTaskAdapter.AddData(req);

        var scheduleId = await Refresh(eventTraceId);

        await callerDevice.SetPanels(eventRequest.PayloadPanels, true);

        _ = TraceLocationPanels(eventRequest, scheduleId);

        if (TryGetScheduleTaskByTraceId(eventTraceId, out var scheduleTask))
        {
            _scheduleTracker.StartScheduleActivity(scheduleTask);
        }

        _logger.LogInformation($"已处理{eventRequest.DeviceId}-{eventRequest.LocationCode}的AGV调用请求，已排队等候调度");
        return new DeviceEventReportResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = $"已处理{eventRequest.DeviceId}-{eventRequest.LocationCode}的AGV调用请求，已排队等候调度",
            TraceId = eventTraceId,
            Data = eventRequest.Params,
        };
    }

    private async Task TraceLocationPanels(DeviceEventReportRequest eventRequest, long scheduleId)
    {
        switch (eventRequest.RequestDeviceKind)
        {
            case DeviceKind.CNC84Drill:
            case DeviceKind.CNC95Drill:
                if (await _sysConfigManager.GetBoolValue("EnableTraceLocationPanels-Drill"))
                {
                    await _locationAdapter.TraceLocationPanels(eventRequest.PayloadPanels, "发起调度", scheduleId);
                }
                break;

            case DeviceKind.Pin:
                if (await _sysConfigManager.GetBoolValue("EnableTraceLocationPanels-Pin"))
                {
                    await _locationAdapter.TraceLocationPanels(eventRequest.PayloadPanels, "发起调度", scheduleId);
                }
                break;

            case DeviceKind.UnPin:
                if (await _sysConfigManager.GetBoolValue("EnableTraceLocationPanels-UnPin"))
                {
                    await _locationAdapter.TraceLocationPanels(eventRequest.PayloadPanels, "发起调度", scheduleId);
                }
                break;

            case DeviceKind.PanelSiloFork:
                if (await _sysConfigManager.GetBoolValue("EnableTraceLocationPanels-PanelSiloFork"))
                {
                    await _locationAdapter.TraceLocationPanels(eventRequest.PayloadPanels, "发起调度", scheduleId);
                }
                break;

            case DeviceKind.PublicPanelSiloWIP:
                if (await _sysConfigManager.GetBoolValue("EnableTraceLocationPanels-PublicPanelSiloWIP"))
                {
                    await _locationAdapter.TraceLocationPanels(eventRequest.PayloadPanels, "发起调度", scheduleId);
                }
                break;
        }
    }

    public async Task<string> SetScheduleAsException(FailScheduleTaskRequest request, Agv? allocatedAgv = null)
    {
        var scheduledTaskStatusList = new List<ScheduledTaskStatus?>
            {
                ScheduledTaskStatus.Running,
                ScheduledTaskStatus.Allocated,
            };
        var schedules = _scheduleTasks.Values.Where(x => x.AllocatedAgv?.ToLower() == request.DeviceId.ToLower() && scheduledTaskStatusList.Contains(x.ScheduledTaskStatus));
        if (!schedules.Any())
        {
            return $"没有找到对应的调度记录";
        }

        foreach (var schedule in schedules)
        {
            var updateDto = new AddOrUpdateScheduleReq
            {
                Code = schedule.Code,
                ScheduledTaskStatus = ScheduledTaskStatus.Failed,
            };

            if (request.Params.ContainsKey("WarningCode"))
            {
                updateDto.WarningCode = request.Params["WarningCode"].ToStr();
            }
            if (request.Params.ContainsKey("WarningMessage"))
            {
                updateDto.WarningMessage = request.Params["WarningMessage"].ToStr();
            }
            if (request.Params.ContainsKey("CurrentLoadedCount"))
            {
                updateDto.TotalRawCount = schedule.TotalRawCount + request.Params["CurrentLoadedCount"].ToInt();
            }
            if (request.Params.ContainsKey("AllocatedAgv"))
            {
                updateDto.RequireDeviceId = request.Params["AllocatedAgv"].ToStr();
            }

            var response = await _scheduleTaskAdapter.UpdateDbScheduleTaskTable(updateDto);
            if (!string.IsNullOrEmpty(response))
            {
                _logger.LogError($"AGV报告异常，更新数据库时，发生异常，schedule id：{schedule.Id}, 异常：{response}.");
                _ = _scheduleTaskAdapter.AddScheduleLog(schedule.Id, $"AGV报告异常，更新数据库时，发生异常，schedule id：{schedule.Id}, 异常：{response}.");
            }

            //await Refresh(schedule.Code);
            schedule.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            //if (allocatedAgv != null) allocatedAgv.TargetDevice = string.Empty;

            _ = TraceLocationPanels(schedule.EventRequest, schedule.Id);

            //通知Drill原有任务异常终止，可以开始下一次呼叫
            _ = EnableDeviceCalling(schedule, request.Params, ScheduledTaskStatus.Failed);
        }

        return string.Empty;
    }

    public async Task<string> SetScheduleAsFinished(CompleteScheduleTaskRequest request, Agv? allocatedAgv = null)
    {
        var schedule = _scheduleTasks.Values.FirstOrDefault(x =>
                            //x.AllocatedAgv?.ToLower() == request.DeviceId.ToLower()
                            //&& x.ScheduledTaskStatus == ScheduledTaskStatus.Running
                            //&&
                            x.Code!.ToLower() == request.TraceId.ToLower()
                            );
        if (schedule == null)
        {
            return $"没有找到对应的调度记录";
        }

        var updateDto = new AddOrUpdateScheduleReq
        {
            Code = schedule.Code,
            ScheduledTaskStatus = ScheduledTaskStatus.Completed,
        };

        if (request.Params.ContainsKey("PlanRawCount"))
        {
            updateDto.TotalRawCount = schedule.TotalRawCount + request.Params["PlanRawCount"].ToInt();
        }

        if (request.Params.ContainsKey("AllocatedAgv"))
        {
            updateDto.RequireDeviceId = request.Params["AllocatedAgv"].ToStr();
        }

        //允许调度分多次执行，完成上料请求
        //当已上料数量未满足钻机请求时，更新状态为PartCompleted
        //SpindleUseNum ,钻机共需要的数量
        //RawSpindleNum，已有参数 钻机发出，本次生料申请数量；并增加到schedule表中
        //ExistRawNum,已有参数 钻机发出，已有生料数量
        //新增参数 TotalPlanRawCount = min(任务计划数量taskCount， 钻机共需要呼叫数量 SpindleUseNum)
        //新增参数 IsAllPanelSent, 下发AGV时，应当知道本次是否是最后任务
        //统计经过尾料处理后，计划下发的生料数量PlanRawCount；并增加到schedule表中
        //此前累计生料上机数量 TotalRawCount，增加到schedule表中
        //多次上料后，记录schedule detail ，记录本次计划下发完成的数量,以及原始需求生料的数量

        //var IsAllPanelSent = request.Params.ContainsKey("IsAllPanelSent") ? request.Params["IsAllPanelSent"].ToBool() : false;
        //var PlanRawCount = request.Params.ContainsKey("PlanRawCount") ? request.Params["PlanRawCount"].ToInt() : 0;
        //var ExistRawNum = request.Params.ContainsKey("ExistRawNum") ? request.Params["ExistRawNum"].ToInt() : 0;
        //var TotalPlanRawCount = request.Params.ContainsKey("TotalPlanRawCount") ? request.Params["TotalPlanRawCount"].ToInt() : 0;

        var response = await _scheduleTaskAdapter.UpdateDbScheduleTaskTable(updateDto);
        if (!string.IsNullOrEmpty(response))
        {
            _logger.LogError($"AGV报告完成，更新数据库时，发生异常，schedule id：{schedule.Id}, 异常：{response}");
            _ = _scheduleTaskAdapter.AddScheduleLog(schedule.Id, $"AGV报告完成，更新数据库时，发生异常，schedule id：{schedule.Id}, 异常：{response}");
        }

        //await Refresh(schedule.Code);
        schedule.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
        //if (allocatedAgv != null) allocatedAgv.TargetDevice = string.Empty;

        _ = SendCompleteAction(request, schedule);

        return string.Empty;
    }

    private async Task SendCompleteAction(CompleteScheduleTaskRequest request, ScheduleTaskWithRequest? schedule)
    {
        await _scheduleTaskAdapter.AddScheduleLog(schedule!.Id, $"调度已完成，IsAllPanelSent:{schedule.IsAllPanelSent}");

        //钻机完成部分上料时，不下发完成信号
        if (DeviceKindExtensions.IsDrill(schedule!.RequestDeviceKind)
            && schedule.InteractionSequence != InteractionSequence.UnloadOnly
            && !schedule.IsAllPanelSent)
        {
            return;
        }
        //钻机完成了所有的上料时，下发完成信号
        else if (DeviceKindExtensions.IsDrill(schedule!.RequestDeviceKind)
            && schedule.InteractionSequence != InteractionSequence.UnloadOnly
            && schedule.IsAllPanelSent)
        {
            await Task.Delay(5000);
            await _panelBarCodeValidator.ValidatePanelBarCodeAsync(schedule);
            //通知设备可以开始下一次呼叫
            await EnableDeviceCalling(schedule, request.Params, ScheduledTaskStatus.Completed);
            //设置生产任务为BUFFER 就绪
            //await _workOrderTaskAdapter.SetBufferReady(schedule.TaskId);
        }
        else
        {
            //通知设备可以开始下一次呼叫
            await EnableDeviceCalling(schedule, request.Params, ScheduledTaskStatus.Completed);
        }
    }

    //public async Task<string> SetScheduleAsFinished(CompleteScheduleTaskRequest request)
    //{
    //    var schedule = _scheduleTasks.Values.FirstOrDefault(x => x.Code == request.TraceId);
    //    if (schedule == null)
    //    {
    //        return $"没有找到对应的调度记录";
    //    }

    //    var updateDto = new AddOrUpdateScheduleReq
    //    {
    //        Code = schedule.Code,
    //        ScheduledTaskStatus = ScheduledTaskStatus.Completed,
    //    };

    //    var response = await _scheduleTaskAdapter.UpdateDbScheduleTaskTable(updateDto);
    //    if (string.IsNullOrEmpty(response))
    //    {
    //        await Refresh(schedule.Code);
    //        await _scheduleTaskAdapter.AddScheduleLog(schedule.Id, request.Message);
    //    }
    //    else
    //    {
    //        await _scheduleTaskAdapter.AddScheduleLog(schedule.Id, $"数据库更新失败-{response}");
    //    }

    //    return response;
    //}

    public async Task<string> SetScheduleAsWorking(StartScheduleTaskRequest startScheduleTaskRequest, Agv agv = null)
    {
        var eventTraceId = startScheduleTaskRequest.TraceId;
        if (string.IsNullOrEmpty(eventTraceId))
        {
            _logger.LogWarning($"TraceId not found. request:{JsonSerializer.Serialize(startScheduleTaskRequest)}");
            return "TraceId not found";
        }

        var schedule = _scheduleTasks.Values.FirstOrDefault(x => x.Code?.ToLower() == eventTraceId.ToLower());
        if (schedule == null)
        {
            return $"没有找到对应的调度记录，trace id：{eventTraceId}";
        }

        //防止混收熟料，再次检查
        if (DeviceKindExtensions.IsDrill(schedule.RequestDeviceKind)
            && agv != null
            && schedule.DrilledItemCodes.Any()
            && schedule.InteractionSequence != InteractionSequence.LoadOnly)
        {
            var drilledItem = schedule.DrilledItemCodes.FirstOrDefault();
            if (agv.PayloadPanels.DrilledItemCodes.Any(x => x.ToLower() != drilledItem!.ToLower()))
            {
                _logger.LogError("识别到混收熟料，暂停本次分配--SetScheduleAsWorking");
                return "识别到混收熟料，暂停本次分配--SetScheduleAsWorking";
            }
        }

        var updateDto = new AddOrUpdateScheduleReq
        {
            Code = schedule.Code,
            ScheduledTaskStatus = ScheduledTaskStatus.Running,
        };

        if (startScheduleTaskRequest.Params.ContainsKey("CarCurrentPos"))
        {
            updateDto.StartLocation = startScheduleTaskRequest.Params["CarCurrentPos"].ToStr();
        }

        if (startScheduleTaskRequest.Params.ContainsKey("BehaivorName"))
        {
            updateDto.RequestInteractionBehaviorName = startScheduleTaskRequest.Params["BehaivorName"].ToStr();
        }

        if (startScheduleTaskRequest.Params.ContainsKey("requireDevice"))
        {
            updateDto.RequireDeviceId = startScheduleTaskRequest.Params["requireDevice"].ToStr();
        }

        if (startScheduleTaskRequest.Params.ContainsKey("AllocatedAgv"))
        {
            updateDto.RequireDeviceId = startScheduleTaskRequest.Params["AllocatedAgv"].ToStr();
        }

        var response = await _scheduleTaskAdapter.UpdateDbScheduleTaskTable(updateDto);
        if (string.IsNullOrEmpty(response))
        {
            await Refresh(schedule.Code);
        }

        return response;
    }

    public async Task<string> CancelSingleSchedule(string traceId, bool isForced, string reason = "")
    {
        var schedule = _scheduleTasks.Values.FirstOrDefault(x => x.Code!.ToLower() == traceId.ToLower());
        if (schedule == null)
        {
            _logger.LogError($"取消调度记录，更新中控内存时，发生异常，schedule code：{traceId}, 异常：未找到这个调度记录");
            return $"取消异常，原因：未找到这个库位的调度记录";
        }
        else
        {
            if (schedule.Appointed && !isForced && await _sysConfigManager.GetBoolValue("EnableLockAppointedSchedule"))
            {
                _logger.LogError($"不能取消已被预约的库位调度，库位：{schedule.LocationCode}，调度ID：{schedule.Id},预约信息：{schedule.AppointedMessage}");
                return $"取消异常，原因：库位：{schedule.LocationCode}已经被预约，不能取消。预约信息：{schedule.AppointedMessage}，";
            }

            await AddLog(schedule.Id, reason);

            var response = await _scheduleTaskAdapter.CancelSingleSchedule(traceId, isForced, reason);
            if (!string.IsNullOrEmpty(response))
            {
                _logger.LogError($"取消调度记录，更新数据库时，发生异常，schedule code：{traceId}, 异常：{response}");
                return $"取消调度记录，更新数据库时，发生异常，schedule code：{traceId}, 异常：{response}";
            }

            schedule.ScheduledTaskStatus = ScheduledTaskStatus.Canceled;
            return string.Empty;
        }
    }

    public async Task<int> RefreshTimeoutSchedule()
    {
        int defaultCreatedScheduleTimeout = 5;
        var valueCreatedTimeout = await _sysConfigManager.GetIntValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_CREATED_TIMEOUT);
        if (valueCreatedTimeout > defaultCreatedScheduleTimeout)
        {
            defaultCreatedScheduleTimeout = valueCreatedTimeout;
        }

        var affectedRows = 0;
        //设备在线，但是所有任务都已经结束（或取消、异常中止）时，清除redis标识
        // var inWorkingStatus = new List<ScheduledTaskStatus?> { ScheduledTaskStatus.PartCompleted, ScheduledTaskStatus.Created, ScheduledTaskStatus.Allocated, ScheduledTaskStatus.Running };
        var newStatus = new List<ScheduledTaskStatus?> { ScheduledTaskStatus.Created, ScheduledTaskStatus.PartCompleted };

        List<string> clearedKeys = new List<string>();
        //清除过期无效的任务: 上报已超时5分钟
        var timeoutScheduleList = _scheduleTasks.Values.Where(x => newStatus.Contains(x.ScheduledTaskStatus)
                                                            && DateTime.Now.AddSeconds(-1 * defaultCreatedScheduleTimeout) > x.CreateTime
                                                            && !x.Appointed
                                                            && string.IsNullOrEmpty(x.AllocatedAgv));
        foreach (var schedule in timeoutScheduleList)
        {
            var response = await CancelSingleSchedule(schedule.Code, false, "超时自动取消");
            if (string.IsNullOrEmpty(response))
            {
                affectedRows++;
            }
        }

        _logger.LogDebug($"RefreshTimeoutSchedule, affectedRows:{affectedRows}");
        return affectedRows;
    }

    public async Task SpecifyFollowedSchedule(string currentScheduleCode, string macthedAgv, string relateCode, string schedulePath = "")
    {
        await _scheduleTaskAdapter.SpecifyFollowedSchedule(currentScheduleCode, macthedAgv, relateCode, schedulePath);
        await Refresh(currentScheduleCode);
    }

    public bool TryGetScheduleTaskByTraceId(string eventTraceId, out ScheduleTaskWithRequest? scheduleTask)
    {
        return _scheduleTasks.TryGetValue(eventTraceId, out scheduleTask);
    }

    public bool TryGetScheduleTaskById(long id, out ScheduleTaskWithRequest? scheduleTask)
    {
        scheduleTask = _scheduleTasks.Values.FirstOrDefault(x => x.Id == id);
        return scheduleTask != null;
    }

    public bool TryGetLatestScheduleTaskOfLocation(string locationCode, out ScheduleTaskWithRequest? scheduleTask)
    {
        scheduleTask = _scheduleTasks.Values.Where(x => x.LocationCode == locationCode)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();
        return scheduleTask != null;
    }

    public async Task UpdateScheduleForChangingSilo(string eventTraceId, DeviceProxy agvDevice, string schedulePath = "")
    {
        await _scheduleTaskAdapter.UpdateScheduleForChangingSilo(eventTraceId, agvDevice, schedulePath);

        await Refresh(eventTraceId);
    }

    private void AddOrUpdateScheduleTask(ScheduleTaskWithRequest task)
    {
        //task.EventRequest = JsonSerializer.Deserialize<DeviceEventReportRequest>(task.RequestJson);

        switch (task.RequestDeviceKind)
        {
            case DeviceKind.CNC84Drill:
            case DeviceKind.CNC95Drill:
                AddOrUpdateScheduleTask<DrillScheduleTask>(task);
                break;

            case DeviceKind.Pin:
                AddOrUpdateScheduleTask<PinScheduleTask>(task);
                break;

            case DeviceKind.UnPin:
                AddOrUpdateScheduleTask<UnpinScheduleTask>(task);
                break;

            case DeviceKind.PanelSiloFork:
                AddOrUpdateScheduleTask<ForkScheduleTask>(task);
                break;

            case DeviceKind.PublicPanelSiloWIP:
                AddOrUpdateScheduleTask<ShelfScheduleTask>(task);
                break;
        }
    }

    private void AddOrUpdateScheduleTask<T>(ScheduleTaskWithRequest task)
            where T : ScheduleTaskWithRequest
    {
        _scheduleTasks.AddOrUpdate(task.Code,
                            (code) =>
                            {
                                var newTask = _mapper.Map<T>(task);
                                newTask.EventRequest = task.EventRequest;
                                newTask.OnTimeout += (t) => _timeoutScheduleTaskHandler.OnTimeout(t);
                                newTask.OnStatusChanged += (t) => _scheduleTaskListener.OnStatusChanged(t);
                                //newTask.OnStatusChanged += (t) => DrillReactOnScheduleStatusChangedLogic.OnStatusChanged(t);
                                //newTask.OnStatusChanged += (t) => DrillReactOnScheduleStatusChangedLogic.OnStatusChanged(t);
                                //newTask.OnStatusChanged += (t) => DrillReactOnScheduleStatusChangedLogic.OnStatusChanged(t);
                                //newTask.OnStatusChanged += (t) => DrillReactOnScheduleStatusChangedLogic.OnStatusChanged(t);

                                task.EventRequest = null;

                                if (!string.IsNullOrEmpty(newTask.LocationCode)
                                    && _locationManager.TryGetLocation(newTask.LocationCode, out var location)
                                    && location != null)
                                {
                                    if (location.Schedule == null || location.Schedule.Id < newTask.Id)
                                        location.Schedule = newTask;
                                }

                                RefreshDrillTask(newTask);

                                return newTask;
                            },

                            (code, oldTask) =>
                            {
                                //override oldTask's properties with new task's properties
                                var convertedTask = _mapper.Map(task, oldTask);
                                RefreshDrillTask(convertedTask);
                                return convertedTask;
                            });
    }

    private void RefreshDrillTask<T>(T newTask) where T : ScheduleTaskWithRequest
    {
        if (newTask is DrillScheduleTask drillScheduleTask
            && _deviceManager.TryGetOnlineDevice<Drill>(drillScheduleTask.CallerDeviceId, out var drill)
            && drill != null)
        {
            drillScheduleTask.Percentage = drill.Percentage;
            drillScheduleTask.DrillBoardPositionStatus = drill.DrillBoardPositionStatus;
            drillScheduleTask.BufferClinkerLayerBoardStatus = drill.BufferClinkerLayerBoardStatus;
            drillScheduleTask.BufferRawMaterialLayerBoardStatus = drill.BufferRawMaterialLayerBoardStatus;
        }
    }

    /// <summary>
    /// 通知生产设备原有任务已完成或已异常，可以开始下一次呼叫,
    /// 部分完成时，不需要通知钻机；
    /// </summary>
    /// <param name="eventTraceId"></param>
    /// <param name="parameters"></param>
    /// <param name="scheduledTaskStatus"></param>
    /// <returns></returns>
    private async Task EnableDeviceCalling(ScheduleTaskWithRequest scheduleTask, Dictionary<string, object?> parameters, ScheduledTaskStatus scheduledTaskStatus)
    {
        //await Task.Delay(5000);

        //await CheckPanelBarCode(scheduleTask);
        await SendComplete(scheduleTask, parameters, scheduledTaskStatus);
    }

    private async Task SendComplete(ScheduleTaskWithRequest scheduleTask, Dictionary<string, object?> parameters, ScheduledTaskStatus scheduledTaskStatus)
    {
        if (parameters.ContainsKey("routingKey"))
        {
            //查找主叫设备
            var callerProxy = _deviceManager.GetOnlineDevice(scheduleTask.CallerDeviceId);
            if (callerProxy == null)
            {
                //叫料设备，当前不在线
                await _scheduleTaskAdapter.AddScheduleLog(scheduleTask.Id, $"未执行下发，叫料设备，当前不在线");
                _logger.LogError($"EnableDeviceCalling，DeviceId:{scheduleTask.CallerDeviceId},叫料设备，当前不在线, trace id:{scheduleTask.Code}");
                return;
            }

            var serviceRequest = new DeviceServiceInvokeRequest()
            {
                ProductId = callerProxy.ProductId,
                DeviceId = callerProxy.DeviceId,
                ServiceId = Topics.Services.COMPLETE_SCHEDULE_SERVICE_ID,
                EventId = scheduleTask.EventRequest.EventId,
                ClientId = callerProxy.ClientId,
                TargetClientId = callerProxy.ClientId,
                TargetProductId = callerProxy.ProductId,
                TargetDeviceId = callerProxy.DeviceId,
                LocationCode = scheduleTask.LocationCode,
                Params = new Dictionary<string, object?> { { ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID, scheduleTask.Code } }
            };

            if (scheduledTaskStatus == ScheduledTaskStatus.Failed)
            {
                //如果agv未到达钻机前，或者首次定位未满足时，AGV报工了异常，
                //那么，钻机仅做取消调度处理，不做异常处理。
                if (parameters.ContainsKey("SendCancelBeforeArrivedDevice") && parameters["SendCancelBeforeArrivedDevice"].ToBool())
                {
                    serviceRequest.ScheduledStatus = ScheduledTaskStatus.Canceled;
                    _ = _scheduleTaskAdapter.AddScheduleLog(scheduleTask.Id, $"AGV报告失败，按Canceled下发指令");
                }
                else
                {
                    serviceRequest.ScheduledStatus = ScheduledTaskStatus.Failed;
                    _ = _scheduleTaskAdapter.AddScheduleLog(scheduleTask.Id, $"AGV报告失败，按Failed下发指令");
                }
            }
            else if (scheduledTaskStatus == ScheduledTaskStatus.Completed)
            {
                serviceRequest.ScheduledStatus = ScheduledTaskStatus.Completed;
            }
            else
            {
                _logger.LogError($"DeviceId:{callerProxy.DeviceId},not support scheduledTaskStatus:{scheduledTaskStatus.ToString()}");
                throw new Exception($"not support scheduledTaskStatus:{scheduledTaskStatus.ToString()}");
            }

            var response = await callerProxy.InvokeService(serviceRequest);

            if (response == null)
            {
                await _scheduleTaskAdapter.AddScheduleLog(scheduleTask.Id, $"下发消息失败，response is null ,serviceRequest，{JsonSerializer.Serialize(serviceRequest)}");
                _logger.LogError($"EnableDeviceCalling，DeviceId:{callerProxy.DeviceId},response:there is no response! trace id:{scheduleTask.Code}");
            }
            else if (response != null && response.Code != ErrorCodes.Sys.SUCCESS)
            {
                await _scheduleTaskAdapter.AddScheduleLog(scheduleTask.Id, $"下发消息失败，response.Code != ErrorCodes.Sys.SUCCESS");
                _logger.LogError($"EnableDeviceCalling,DeviceId:{callerProxy.DeviceId},response:{response.Code},{response.Message},trace id:{scheduleTask.Code}");
                var inputRequest = JsonSerializer.Serialize(serviceRequest);
                serviceRequest.PayloadPanels.Clear();

                await _deviceServiceInvocationLogger.OnInvocationFailure(new DeviceServiceInvocationArgs
                {
                    MessageId = scheduleTask.Code,
                    RequestTopic = serviceRequest.RequestTopic,
                    ResponseTopic = serviceRequest.ReplyTopic,
                    Payload = inputRequest,
                    Reason = response.Code,
                    ServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce,
                    RoutingKey = parameters["routingKey"].ToStr(),
                });
            }
            else
            {
                await _scheduleTaskAdapter.AddScheduleLog(scheduleTask.Id, $"下发消息成功: ScheduledStatus：{scheduledTaskStatus.ToStr()}");
                await _distributedCache.RemoveAsync(scheduleTask.RoutingKey);
                _logger.LogWarning($"EnableDeviceCalling successfully，调度记录 id:{scheduleTask.Id},DeviceId:{scheduleTask.CallerDeviceId}.");
            }
        }
        else
        {
            await _scheduleTaskAdapter.AddScheduleLog(scheduleTask.Id, $"未执行下发，Params 未包含参数：routingKey,调度记录 id:{scheduleTask.Id},DeviceId:{scheduleTask.CallerDeviceId}");
            _logger.LogError($"Params 未包含参数：routingKey ,调度记录 id:{scheduleTask.Id},DeviceId:{scheduleTask.CallerDeviceId}");
            return;
        }
    }

    public ScheduleTaskWithRequest FindScheduleByTraceId(string traceId)
    {
        _scheduleTasks.TryGetValue(traceId, out var scheduleTask);

        return scheduleTask;
    }

    public async Task AddLog(long masterId, string reason)
    {
        await _scheduleTaskAdapter.AddScheduleLog(masterId, reason);
    }

    public async Task<AddScheduleLogResponse> AddScheduleLog(AddScheduleLogRequest request)
    {
        if (string.IsNullOrEmpty(request.TraceId))
        {
            _logger.LogWarning($"TraceId not found. request:{JsonSerializer.Serialize(request)}");
            return new AddScheduleLogResponse
            {
                Code = ErrorCodes.Sys.MISSING_TRACE_ID_CODE,
                Message = ErrorCodes.Sys.MISSING_TRACE_ID_MESSAGE
            };
        }
        var schedule = FindScheduleByTraceId(request.TraceId);
        if (schedule == null)
        {
            return new AddScheduleLogResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"没有找到对应的调度记录，trace id：{request.TraceId}"
            };
        }
        var response = await _scheduleLogService.Add(new AddOrUpdateScheduleLogReq
        {
            MasterId = schedule.Id,
            Message = request.Message,
            Spindle = request.Spindle,
            Status = 1
        });
        if (response == null || !string.IsNullOrEmpty(response.Message))
        {
            return new AddScheduleLogResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"写入调度明细失败，trace id:{request.TraceId},原因：{response.Message}"
            };
        }

        return new AddScheduleLogResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = ""
        };
    }

    public bool TryGetNotStartedScheduleByLocationCode(string locationCode, out ScheduleTaskWithRequest? scheduleTask)
    {
        scheduleTask = NotStartedSchedules.FirstOrDefault(x => x.LocationCode?.ToLower() == locationCode.ToLower());

        return scheduleTask != null;
    }

    public bool TryGetTodoOrDoingScheduleByLocationCode(string locationCode, out ScheduleTaskWithRequest? scheduleTask)
    {
        scheduleTask = AllTodoOrDoingSchedules.FirstOrDefault(x => x.LocationCode?.ToLower() == locationCode.ToLower());

        return scheduleTask != null;
    }

    private async Task<int> GetLoadScheduleDays()
    {
        int defaultloadScheduleTaskDays = 1;
        var loadScheduleTaskDays = await _sysConfigManager.GetIntValue(MESConfigConstants.LOAD_SCHEDULE_TASK_SETTINGSDAYS_DATA);
        if (loadScheduleTaskDays > defaultloadScheduleTaskDays)
        {
            defaultloadScheduleTaskDays = loadScheduleTaskDays;
        }

        return defaultloadScheduleTaskDays;
    }

    public async Task UpdateAllocatedAgv(ScheduleTaskWithRequest task)
    {
        await _scheduleTaskAdapter.UpdateAllocatedAgv(task);

        await Refresh(task.Code);
    }
}
