using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Schedule;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Domain;

public class PanelAgv : Agv
{
    private readonly ILogger _logger;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationManager _locationManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;

    [ActivatorUtilitiesConstructor]
    public PanelAgv(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<PanelAgv>();
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
    }

    /// <summary>
    /// 所拥有的库位
    /// </summary>
    public Location Location => _locationManager.Locations.FirstOrDefault(x => x.HostDevice?.DeviceId == this.DeviceId) ?? new Location();

    /// <summary>
    /// 所拥有的板料
    /// </summary>
    public PanelList Panels => Location.Panels;

    public override bool IsIdle => base.IsIdle && RouteCodes.Any() && Location.Panels.Any();

    /// <summary>
    /// agv 将身上的料仓放到emptyForkScheduleTask，然后去followedForkSchedule托一个料仓
    /// </summary>
    /// <param name="emptyForkScheduleTask"></param>
    /// <param name="followedForkSchedule"></param>
    /// <returns></returns>
    public async Task<bool> TryExchangeSilo(ScheduleTaskWithRequest emptyForkScheduleTask, ScheduleTaskWithRequest followedForkSchedule)
    {
        var eventRequestEmptyFork = emptyForkScheduleTask.EventRequest;
        var emptyFork = _deviceHolder.GetOnlineDevice(eventRequestEmptyFork.DeviceId);
        if (emptyFork == null)
        {
            _logger.LogWarning($"schedulePath:Not any online fork found when changing silo for agv {DeviceId},{DeviceKind},{DeviceStandbyTime}");
            return false;
        }
        TargetDevice = eventRequestEmptyFork.DeviceId;
        TargetLocation = eventRequestEmptyFork.LocationCode;

        _logger.LogInformation($"schedulePath:ChangeSiloForBackPanelAgv device {Descriptor.ProductId}-{Descriptor.DeviceId}- begin to call ScheduleTask,");
        var responseFromEmptyFork = await AssignScheduleTask(this, eventRequestEmptyFork, emptyFork);
        if (responseFromEmptyFork != null && responseFromEmptyFork.Code == ErrorCodes.Sys.SUCCESS)
        {
            emptyForkScheduleTask.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;
            followedForkSchedule.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;

            var schedulePath = $"调度路线：{emptyForkScheduleTask.LocationCode}[{emptyForkScheduleTask.Id}]==>{followedForkSchedule.LocationCode}[{followedForkSchedule.Id}]";
            _logger.LogInformation($"schedulePath:{Environment.NewLine}-----------------{Environment.NewLine} 下发任务成功 ，{schedulePath},agv:{DeviceId}{Environment.NewLine}");
            await _scheduleTaskManager.UpdateScheduleForChangingSilo(eventRequestEmptyFork.TraceId, this, schedulePath);

            _logger.LogWarning($"schedulePath:{Environment.NewLine}-----------------{Environment.NewLine} 尝试下一个插齿 agv:{DeviceId}，fork:{followedForkSchedule.InteractionSequence}{followedForkSchedule.RequestInteractionBehaviorName}{Environment.NewLine}");
            await _scheduleTaskManager.SpecifyFollowedSchedule(followedForkSchedule.Code, DeviceId, eventRequestEmptyFork.TraceId, $"承接-{schedulePath}");

            return true;
        }
        else
        {
            _logger.LogWarning($"schedulePath:{Environment.NewLine}-----------------{Environment.NewLine} 下发任务失败 agv:{DeviceId}{Environment.NewLine}");
            TargetDevice = string.Empty;
            return false;
        }
    }

    public async Task<AgvAllocationResultCode> TransferSiloBetweenForkAndShelf(ScheduleTaskWithRequest unloadSchedule, ScheduleTaskWithRequest loadSchedule)
    {
        var unloadDevice = _deviceHolder.GetOnlineDevice(unloadSchedule.CallerDeviceId);
        var loadDevice = _deviceHolder.GetOnlineDevice(loadSchedule.CallerDeviceId);
        if (unloadDevice == null || loadDevice == null)
        {
            _logger.LogWarning($"HandleExportSiloFromOneFork unloadDevice {unloadSchedule.CallerDeviceId} or loadDevice {loadSchedule.CallerDeviceId} is not online.");
            return AgvAllocationResultCode.Failed;
        }

        var requestOfUnloadSchedule = unloadSchedule.EventRequest;
        if (requestOfUnloadSchedule == null)
        {
            _logger.LogWarning($"HandleExportSiloFromOneFork requestOfUnloadSchedule RequestJson is null.");
            return AgvAllocationResultCode.Failed;
        }
        var eventRequestEmptyFork = loadSchedule.EventRequest;
        if (eventRequestEmptyFork == null)
        {
            _logger.LogWarning($"HandleExportSiloFromOneFork eventRequestEmptyFork RequestJson is null.");
            return AgvAllocationResultCode.Failed;
        }

        TargetDevice = unloadDevice.DeviceId;
        TargetLocation = requestOfUnloadSchedule.LocationCode;
        _logger.LogInformation($"ChangeSiloForBackPanelAgv device {Descriptor.ProductId}-{Descriptor.DeviceId}- begin to call ScheduleTask,");
        var responseFromUnloadRequest = await AssignScheduleTask(this, requestOfUnloadSchedule, unloadDevice);
        if (responseFromUnloadRequest != null && responseFromUnloadRequest.Code == ErrorCodes.Sys.SUCCESS)
        {
            unloadSchedule.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;
            loadSchedule.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;
            var schedulePath = $"调度路线：{unloadSchedule.LocationCode}[{unloadSchedule.Id}]==>{loadSchedule.LocationCode}[{loadSchedule.Id}]";

            _logger.LogInformation($"{Environment.NewLine}-----------------{Environment.NewLine} 下发任务成功 agv:{DeviceId}{Environment.NewLine}");
            await _scheduleTaskManager.UpdateScheduleForChangingSilo(requestOfUnloadSchedule.TraceId, this, schedulePath);

            _logger.LogWarning($"{Environment.NewLine}-----------------{Environment.NewLine} 尝试下一个插齿 agv:{DeviceId}，fork:{loadSchedule.InteractionSequence}{loadSchedule.RequestInteractionBehaviorName}{Environment.NewLine}");
            await _scheduleTaskManager.SpecifyFollowedSchedule(loadSchedule.Code, DeviceId, requestOfUnloadSchedule.TraceId, schedulePath);

            return AgvAllocationResultCode.Success;
        }
        else
        {
            _logger.LogWarning($"{Environment.NewLine}-----------------{Environment.NewLine} 下发任务失败 agv:{DeviceId}{Environment.NewLine}");
            TargetDevice = string.Empty;
            return AgvAllocationResultCode.Failed;
        }
    }

    /// <summary>
    /// 放下一个料仓
    /// </summary>
    /// <param name="emptyForkScheduleTask"></param>
    /// <returns></returns>
    public async Task<bool> PutSiloToFork(ScheduleTaskWithRequest emptyForkScheduleTask)
    {
        var eventRequestEmptyFork = emptyForkScheduleTask.EventRequest;
        var emptyFork = _deviceHolder.GetOnlineDevice(eventRequestEmptyFork.DeviceId);
        if (emptyFork == null)
        {
            _logger.LogWarning($"Not any online fork found when changing silo for agv {DeviceId},{DeviceKind},{DeviceStandbyTime}");
            return false;
        }
        TargetDevice = eventRequestEmptyFork.DeviceId;
        TargetLocation = eventRequestEmptyFork.LocationCode;

        _logger.LogInformation($"ChangeSiloForBackPanelAgv device {Descriptor.ProductId}-{Descriptor.DeviceId}- begin to call ScheduleTask,");
        var responseFromEmptyFork = await AssignScheduleTask(this, eventRequestEmptyFork, emptyFork);
        if (responseFromEmptyFork != null && responseFromEmptyFork.Code == ErrorCodes.Sys.SUCCESS)
        {
            emptyForkScheduleTask.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;

            var schedulePath = $"schedulePath>>调度路线：agv[{this.DeviceId}]前往{emptyForkScheduleTask.LocationCode}放下料仓";
            _logger.LogInformation(schedulePath);
            await _scheduleTaskManager.UpdateScheduleForChangingSilo(eventRequestEmptyFork.TraceId, this, schedulePath.Replace("schedulePath>>", ""));
            return true;
        }
        else
        {
            _logger.LogWarning($"{Environment.NewLine}-----------------{Environment.NewLine} 下发任务失败 agv:{DeviceId}{Environment.NewLine}");
            TargetDevice = string.Empty;
            return false;
        }
    }

    /// <summary>
    /// agv从插齿上拿料仓
    /// </summary>
    /// <param name="forkSchedule"></param>
    /// <returns></returns>
    public async Task<bool> GetSiloFromFork(ScheduleTaskWithRequest forkSchedule)
    {
        return await GetSiloFromForkThenServeDrill(forkSchedule, null);
    }

    /// <summary>
    /// 从插齿上拿到料仓然后去服务钻机
    /// </summary>
    /// <param name="forkSchedule"></param>
    /// <param name="drillRequest"></param>
    /// <returns></returns>
    public async Task<bool> GetSiloFromForkThenServeDrill(ScheduleTaskWithRequest forkSchedule, ScheduleTaskWithRequest? drillRequest)
    {
        var locationCode = forkSchedule.EventRequest.LocationCode;
        _logger.LogWarning($"-----------------已指定空负载状态的agv:{DeviceId}， 去 {locationCode} 装载料仓.");

        var forkDevice = _deviceHolder.GetOnlineDevice(forkSchedule.EventRequest.DeviceId);
        if (forkDevice == null)
        {
            _logger.LogWarning($"Fork {forkSchedule.EventRequest.DeviceId} is not online when changing silo for agv {DeviceId},{DeviceKind},{DeviceStandbyTime}");
            return false;
        }

        TargetDevice = forkSchedule.EventRequest.DeviceId;
        TargetLocation = forkSchedule.LocationCode;
        var response = await AssignScheduleTask(this, forkSchedule.EventRequest, forkDevice);
        if (response != null && response.Code == ErrorCodes.Sys.SUCCESS)
        {
            forkSchedule.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;
            var schedulePath = $"schedulePath>>调度路线：agv[{this.DeviceId}]前往{forkSchedule.LocationCode}取到料仓";
            _logger.LogDebug($"CentralVerifyFunction01 --10-1 agv={this.DeviceId} GetSiloFromForkThenServeDrill={schedulePath}");
            await _scheduleTaskManager.UpdateScheduleForChangingSilo(forkSchedule.EventRequest.TraceId, this, schedulePath.Replace("schedulePath>>", ""));

            if (drillRequest != null)
            {
                drillRequest.ScheduledTaskStatus = ScheduledTaskStatus.Allocated;
                schedulePath = $"schedulePath>>调度路线：agv[{this.DeviceId}]在{forkSchedule.LocationCode}取到料仓后去{drillRequest.CallerDeviceId}上下料";
                _logger.LogDebug($"CentralVerifyFunction01 --10-2 agv={this.DeviceId} GetSiloFromForkThenServeDrill={schedulePath}");
                await _scheduleTaskManager.SpecifyFollowedSchedule(drillRequest.Code, DeviceId, forkSchedule.EventRequest.TraceId, schedulePath);
            }

            return true;
        }
        else
        {
            _logger.LogDebug($"CentralVerifyFunction01 --10-3 agv={this.DeviceId} GetSiloFromForkThenServeDrill 下发任务失败");
            TargetDevice = string.Empty;
            return false;
        }
    }

    protected async Task<DeviceServiceInvokeResponse> AssignScheduleTask(DeviceProxy agv,
        DeviceEventReportRequest forkEventRequest,
        DeviceProxy emptyFork)
    {
        var serviceRequestRelateDevice = new DeviceServiceInvokeRequest
        {
            ProductId = forkEventRequest.ProductId,
            DeviceId = forkEventRequest.DeviceId,
            ClientId = forkEventRequest.ClientId,
            ServiceId = Topics.Services.SCHEDULE_TASK_SERVICE_ID,
            EventId = forkEventRequest.EventId,
            EventName = forkEventRequest.EventName,
            HostAddress = emptyFork.Descriptor.HostAddress,
            CallerRequestInteractionDirection = forkEventRequest.RequestInteractionDirection,
            CallerRequestMaterialKind = forkEventRequest.RequestMaterialKind,
            CallerRequestInteractionBehavior = forkEventRequest.RequestInteractionBehavior,
            CallerRequestInputProductStatus = forkEventRequest.RequestInputProductStatus,
            CallerRequestOutputProductStatus = forkEventRequest.RequestOutputProductStatus,
            //CallerInputCapabilities = emptyFork.Descriptor.InputCapabilities,
            //CallerOutputCapabilities = emptyFork.Descriptor.OutputCapabilities,
            TargetProductId = Descriptor.ProductId,
            TargetDeviceId = Descriptor.DeviceId,
            TargetClientId = ClientId,
            TargetHostAddress = Descriptor.HostAddress,
            Params = forkEventRequest.Params,
            PayloadPanels = forkEventRequest.PayloadPanels,
            LocationCode = forkEventRequest.LocationCode,
        };
        serviceRequestRelateDevice.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME] = DateTime.MinValue;
        serviceRequestRelateDevice.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID] = forkEventRequest.TraceId;
        var responseFromEmptyFork = await InvokeService(serviceRequestRelateDevice);
        return responseFromEmptyFork;
    }
}
