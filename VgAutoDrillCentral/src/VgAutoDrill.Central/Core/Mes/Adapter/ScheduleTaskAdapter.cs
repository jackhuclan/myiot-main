using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Logging;

using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Mes;

public class ScheduleTaskAdapter : IScheduleTaskAdapter
{
    private readonly IScheduleService _scheduleService;
    private readonly ITaskService _taskService;
    private readonly ILogger<ScheduleTaskAdapter> _logger;
    private readonly IMapper _mapper;
    private readonly IScheduleLogService _scheduleLogService;

    public ScheduleTaskAdapter(IScheduleService scheduleService,
        ITaskService taskService,
        IScheduleLogService scheduleLogService,
        ILogger<ScheduleTaskAdapter> logger,
        IMapper mapper)
    {
        _scheduleService = scheduleService;
        _taskService = taskService;
        _logger = logger;
        _mapper = mapper;
        _scheduleLogService = scheduleLogService;
    }

    public async Task<bool> HasUnstartedOrDoingScheduleTaskByRoutingKey(string routingKey)
    {
        var queryCondition = new GetScheduleListReq
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus?>
                    {
                       ScheduledTaskStatus.Created,
                       ScheduledTaskStatus.Allocated,
                       ScheduledTaskStatus.Running,
                       ScheduledTaskStatus.PartCompleted,
                    },
            RoutingKey = routingKey
        };
        var hasAnyTodoSchedule = await _scheduleService.ExsistSchedule(queryCondition);

        return hasAnyTodoSchedule;
    }

    public async Task<bool> HasCompletedScheduleWithNotStartedWorkTask(string taskId)
    {
        var queryCondition = new GetScheduleListReq
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus?>
            {
                ScheduledTaskStatus.Completed,
            },
            IsAllPanelSent = true,
            TaskId = taskId,
        };
        var findFinishedSchedule = await _scheduleService.ExsistSchedule(queryCondition);
        if (findFinishedSchedule)
        {
            var notBeginTasks = await _taskService.GetList(new Admin.Model.ViewModels.Mes.ProTask.GetTaskListReq
            {
                Code = taskId,
                TaskStatusList = new List<Admin.Model.Enum.TaskStatusEnum>
                {
                     Admin.Model.Enum.TaskStatusEnum.COMMITED,
                     Admin.Model.Enum.TaskStatusEnum.SENDING,
                     //Admin.Model.Enum.TaskStatusEnum.BUFFERED,
                },
            });

            if (notBeginTasks.Data.List.Any())
            {
                return true;
            }
        }

        return false;
    }

    public async Task<ScheduleTaskWithRequest> FindScheduleByTraceId(string traceId)
    {
        var schedule = await _scheduleService.FindScheduleByTraceId(traceId);
        if (schedule == null) return null;

        var task = _mapper.Map<ScheduleTaskWithRequest>(schedule);
        task.EventRequest = task.RequestJson.FromJson<DeviceEventReportRequest>();
        return task;
    }

    public async Task<ScheduleTask> FindMasterSchedule(ScheduleTask inputSchedule)
    {
        if (!inputSchedule.MasterScheduleId.HasValue || inputSchedule.MasterScheduleId == 0)
            return null;

        var response = await _scheduleService.QueryByID((int)inputSchedule.MasterScheduleId);
        if (response == null || response.Data == null) return null;

        var task = _mapper.Map<ScheduleTask>(response.Data);
        return task;
    }

    public async Task<ScheduleTaskWithRequest> FindTodoScheduleByRoutingKey(string routingKey)
    {
        var schedule = await _scheduleService.FindScheduleByRoutingKey(routingKey);
        return _mapper.Map<ScheduleTaskWithRequest>(schedule);
    }

    public async Task<List<T>> GetSchedule<T>(QueryScheduleRequest request)
        where T : ScheduleTaskWithRequest
    {
        var condition = _mapper.Map<GetScheduleListReq>(request);
        condition.PageSize = int.MaxValue;
        var result = await _scheduleService.GetScheduleTasks(condition);
        var list = _mapper.Map<List<ScheduleDto>, List<T>>(result.Data);
        list.ForEach(x => x.EventRequest = JsonSerializer.Deserialize<DeviceEventReportRequest>(x.RequestJson));
        return list.Where(x => x.EventRequest != null).ToList();
    }

    /// <summary>
    /// 获取未完成的钻机调度请求
    /// </summary>
    /// <returns></returns>
    public async Task<List<DrillScheduleTask>> GetNotStartedDrillSchedule()
    {
        return await GetSchedule<DrillScheduleTask>(new QueryScheduleRequest
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.PartCompleted
            },
            RequestDeviceKindList = new List<DeviceKind>
            {
                DeviceKind.CNC84Drill,
                DeviceKind.CNC95Drill
            },
            HasAgvSetted = false,
            StartTime = DateTime.Now.AddDays(-1),
        });
    }

    /// <summary>
    /// 获取未完成的钻机调度请求
    /// </summary>
    /// <returns></returns>
    public async Task<List<DrillScheduleTask>> GetNotStartedDrillSchedule(string callderDeviceId)
    {
        return await GetSchedule<DrillScheduleTask>(new QueryScheduleRequest
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.PartCompleted
            },
            RequestDeviceKindList = new List<DeviceKind>
            {
                DeviceKind.CNC84Drill,
                DeviceKind.CNC95Drill
            },
            SourceDeviceId = callderDeviceId,
            StartTime = DateTime.Now.AddDays(-1),
        });
    }

    /// <summary>
    /// 获取未完成的pin调度请求
    /// </summary>
    /// <returns></returns>
    public async Task<List<PinScheduleTask>> GetNotStartedPinSchedule()
    {
        return await GetSchedule<PinScheduleTask>(new QueryScheduleRequest
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.PartCompleted
            },
            RequestDeviceKindList = new List<DeviceKind>
            {
                DeviceKind.Pin,
            },
            HasAgvSetted = false,
            StartTime = DateTime.Now.AddDays(-1),
        });
    }

    /// <summary>
    /// 获取未完成的unpin调度请求
    /// </summary>
    /// <returns></returns>
    public async Task<List<UnpinScheduleTask>> GetNotStartedUnpinSchedule()
    {
        return await GetSchedule<UnpinScheduleTask>(new QueryScheduleRequest
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created,
                ScheduledTaskStatus.PartCompleted
            },
            RequestDeviceKindList = new List<DeviceKind>
            {
                DeviceKind.UnPin,
            },
            HasAgvSetted = false,
            StartTime = DateTime.Now.AddDays(-1),
        });
    }

    /// <summary>
    /// 获取未完成的插齿和料架调度请求
    /// </summary>
    /// <returns></returns>
    public async Task<List<ShelfScheduleTask>> GetNotStartedForkOrShelfSchedule()
    {
        return await GetSchedule<ShelfScheduleTask>(new QueryScheduleRequest
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created,
            },
            RequestDeviceKindList = new List<DeviceKind>
            {
                DeviceKind.PanelSiloFork,
                DeviceKind.PublicPanelSiloWIP,
            },
            HasAgvSetted = false,
            StartTime = DateTime.Now.AddDays(-1),
        });
    }

    /// <summary>
    /// 获取待执行的配对调度中的后一个调度请求
    /// </summary>
    /// <returns></returns>
    public async Task<List<ScheduleTaskWithRequest>> GetNotStartedFollowedSchedule()
    {
        return await GetSchedule<ScheduleTaskWithRequest>(new QueryScheduleRequest
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus>
                {
                    ScheduledTaskStatus.Created,
                },
            HasAgvSetted = true,
            StartTime = DateTime.Now.AddDays(-1),
        });
    }

    /// <summary>
    /// 获取待执行的主叫的调度请求
    /// </summary>
    /// <returns></returns>
    public async Task<List<ScheduleTaskWithRequest>> GetNotStartedSpecifiedAgvSchedule()
    {
        return await GetSchedule<ScheduleTaskWithRequest>(new QueryScheduleRequest
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus>
            {
                ScheduledTaskStatus.Created
            },
            IsMaster = true,
            RequestDeviceKindList = new List<DeviceKind>
            {
                DeviceKind.UnPin,
                DeviceKind.Pin,
                DeviceKind.PanelSiloFork,
                DeviceKind.PublicPanelSiloWIP,
            },
            StartTime = DateTime.Now.AddDays(-1),
        });
    }
    //public async Task<int> RefreshTimeoutSchedule()
    //{
    //    var count = await _scheduleService.ClearTimeoutSchedule();
    //    _logger.LogDebug($"RefreshTimeoutSchedule, count:{count}");
    //    return count;
    //}

    //public async Task BatchCancelSchedule(List<long> scheduleIds)
    //{
    //    await _scheduleService.BulkCanceled(scheduleIds);
    //}

    public async Task<string> CancelSingleSchedule(string traceId, bool isForced = false, string reason = "")
    {
        return await _scheduleService.CancelSingle(traceId, isForced, reason);
    }

    public async Task<string> FindSingleBySubDeviceCode(string subDeviceCode)
    {
        return await _scheduleService.FindSingleBySubDeviceCode(subDeviceCode);
    }

    public async Task SpecifyFollowedSchedule(string currentScheduleCode, string macthedAgv, string relateCode, string schedulePath)
    {
        await _scheduleService.SetMatchSchedule(currentScheduleCode, macthedAgv, relateCode, schedulePath);
    }

    public async Task UpdateScheduleForChangingSilo(string eventTraceId, DeviceProxy agvDevice, string schedulePath)
    {
        if (!string.IsNullOrEmpty(eventTraceId))
        {
            var changedSpindles = agvDevice.Properties["changedSpindles"].ToStr();
            var changedBehavior = agvDevice.Properties["changedBehavior"].ToStr();
            var taskCode = agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ID].ToStr();
            var itemCode = agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr();
            var isMaster = agvDevice.Properties.ContainsKey("IsMaster") ? agvDevice.Properties["IsMaster"].ToBool() : false;

            var updateDto = new AddOrUpdateScheduleReq
            {
                Code = eventTraceId,
                RequireDeviceId = agvDevice.Descriptor.DeviceId,
                ScheduledTaskStatus = ScheduledTaskStatus.Allocated,
                ChangedSpindles = changedSpindles,
                ChangedBehavior = changedBehavior,
                ItemCode = itemCode,
                TaskId = taskCode,
                IsAllPanelSent = true,
                RequestInteractionBehaviorName = schedulePath,
                AGVPayloadPanels = agvDevice.PayloadPanels.SummaryPanelInfo(),
            };
            if (isMaster)
            {
                updateDto.IsMaster = true;
            }
            await _scheduleService.UpdateCentralTask(updateDto);
        }
    }

    public async Task<ScheduleTaskWithRequest> GetSchedule(long scheduleId)
    {
        var schedule = await _scheduleService.QueryByID((int)scheduleId);
        return _mapper.Map<ScheduleTaskWithRequest>(schedule);
    }

    public async Task<bool> HasUnstartedOrDoingScheduleTaskByAllocatedAgv(string allocatedAgv)
    {
        var queryCondition = new GetScheduleListReq
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus?>
                    {
                        ScheduledTaskStatus.Created,
                        ScheduledTaskStatus.Allocated,
                        ScheduledTaskStatus.Running,
                        ScheduledTaskStatus.PartCompleted,
                    },
            RequireDeviceId = allocatedAgv
        };
        var hasAnyTodoSchedule = await _scheduleService.ExsistSchedule(queryCondition);

        return hasAnyTodoSchedule;
    }

    public async Task AddScheduleLog(long masterId, string message)
    {
        await _scheduleLogService.Add(new AddOrUpdateScheduleLogReq
        {
            Status = 1,
            MasterId = masterId,
            Message = message
        });
    }

    public async Task<string> UpdateDbScheduleTaskTable(AddOrUpdateScheduleReq updateDto)
    {
        return (await _scheduleService.UpdateCentralTask(updateDto)).Message;
    }

    public async Task<string> AddData(AddOrUpdateScheduleReq updateDto)
    {
        return (await _scheduleService.AddData(updateDto)).Message;
    }

    public async Task UpdateAllocatedAgv(ScheduleTaskWithRequest task)
    {
        if (!string.IsNullOrEmpty(task.Code))
        {
            var updateDto = new AddOrUpdateScheduleReq
            {
                Code = task.Code,
                RequireDeviceId = task.AllocatedAgv
            };
            await _scheduleService.UpdateCentralTask(updateDto);
        }
    }
}
