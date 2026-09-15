using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal class CutterRequestEventDelegator : IDeviceEventDelegator
{
    private readonly ILogger<PanelRequestEventDelegator> _logger;
    private readonly IDeviceManager _deviceManager;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleService _scheduleService;
    private readonly IDistributedCache _distributedCache;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;

    public CutterRequestEventDelegator(IDeviceManager deviceHolder,
        ISysConfigManager sysConfigManager,
        IScheduleService scheduleService,
        IDistributedCache distributedCache,
        IServiceProvider serviceProvider,
        IScheduleTaskManager taskScheduler,
        ILoggerFactory loggerFactory)
    {
        _deviceManager = deviceHolder;
        _logger = loggerFactory.CreateLogger<PanelRequestEventDelegator>();
        _sysConfigManager = sysConfigManager;
        _scheduleService = scheduleService;
        _distributedCache = distributedCache;
        _scheduleTaskManager = taskScheduler;
        _taskScheduleOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;
    }

    public async Task<DeviceEventReportResponse> HandleEvent(DeviceEventReportRequest request)
    {
        var convertedInteractionBehavior = (InteractionBehavior)request.RequestInteractionBehavior;
        if (convertedInteractionBehavior == null || convertedInteractionBehavior.DeviceKind == DeviceKind.Unknown)
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"{request.DeviceId} - Invalid request.RequestInteractionBehavior {request.RequestInteractionBehavior}, Cannot convert to InteractionBehavior"
            };
        }

        var calllerProxy = _deviceManager.GetOnlineDevice(request.DeviceId);
        if (calllerProxy == null)
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = $"{ErrorCodes.Sys.WRONG_DEVICE_MESSAGE} - {request.DeviceId}"
            };
        }

        await calllerProxy.SetCutterTrays(request.PayloadCutterTrays);

        if (!CentralFlags.SystemPreloadCompleted)
        {
            _logger.LogWarning($"系统已启用预加载模式，但是尚未加载完成，请等待.");
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"系统已启用预加载模式，但是尚未加载完成，请等待"
            };
        }

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"系统即将停机维护");
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"系统即将停机维护"
            };
        }

        string eventTraceId;
        var routingKey = $"{request.ProductId}.{request.DeviceId}.event.{request.EventId}".ToLower();
        request.Params["routingKey"] = routingKey;

        if (await _distributedCache.GetStringAsync(routingKey) != null)
        {
            var traceId = await _distributedCache.GetStringAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = $"info:{ErrorCodes.Sys.DUPLICATE_EVENT_SCHEDULE_REQUEST_MESSAGE},device:{request.DeviceId}",
                TraceId = traceId,
            };
        }

        //如果自带trace id，则应用设备自带的traceid
        if (!string.IsNullOrEmpty(request.TraceId))
        {
            eventTraceId = request.TraceId;
        }
        else
        {
            eventTraceId = Guid.NewGuid().ToString();
        }

        await _distributedCache.SetStringAsync(routingKey, eventTraceId);
        _logger.LogDebug($"routingKey:{routingKey},eventTraceId:{eventTraceId}");
        request.TraceId = eventTraceId;
        request.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID] = eventTraceId;

        var exsistRoutingSchedule = await _scheduleService.ExsistSchedule(new GetScheduleListReq
        {
            ScheduledTaskStatusList = new List<ScheduledTaskStatus?>
                    {
                        ScheduledTaskStatus.Created,
                        ScheduledTaskStatus.Allocated,
                        ScheduledTaskStatus.Running,
                        ScheduledTaskStatus.PartCompleted,
                    },
            RoutingKey = routingKey,
        });

        if (exsistRoutingSchedule)
        {
            _logger.LogInformation($"当前存在在途的调度申请,{request.DeviceId}");
            await _distributedCache.SetStringAsync(routingKey, eventTraceId);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"当前存在在途的调度申请,{request.DeviceId}",
                TraceId = ""
            };
        }

        var taskCode = string.Empty;        // 下个待生产的任务代号
        var taskItemCode = string.Empty;    // 任务中的物料编码
        int taskItemCount = 0;              // 任务中的领取物料数量

        request.Params[ScheduleConstants.PARAMS_TASK_ID] = taskCode;
        request.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = taskItemCode;
        request.Params[ScheduleConstants.PARAMS_TASK_ITEM_COUNT] = taskItemCount;

        //获取钻机等主叫设备的点位
        //钻机缺料时参数中必须带入当前可用的轴数的AGVposition
        if (!request.Params.ContainsKey("Spindles"))
        {
            await _distributedCache.RemoveAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"缺少设备参数，请检查配置文件：{request.DeviceId}/Extra/Spindles"
            };
        }

        if (!request.Params.ContainsKey("SpindleBehavior"))
        {
            await _distributedCache.RemoveAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"缺少运行参数，请检查：{request.DeviceId} Params/SpindleBehavior"
            };
        }

        return await _scheduleTaskManager.ScheduleAGVDevcieViaMysql(request, calllerProxy);
    }
}
