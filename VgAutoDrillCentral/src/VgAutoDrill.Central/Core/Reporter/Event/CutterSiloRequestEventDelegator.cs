using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal class CutterSiloRequestEventDelegator : IDeviceEventDelegator
{
    private readonly ILogger<DefaultRequestEventDelegator> _logger;
    private readonly IDistributedCache _distributedCache;
    private readonly IDeviceManager _deviceHolder;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly IScheduleTaskAdapter _scheduleTaskAdapter;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleTaskManager _taskScheduler;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;

    public CutterSiloRequestEventDelegator(IDistributedCache distributedCache,
        IDeviceManager deviceHolder,
        IWorkOrderTaskAdapter workOrderTaskAdapter,
        IScheduleTaskAdapter scheduleTaskAdapter,
        ISysConfigManager sysConfigManager,
        IScheduleTaskManager taskScheduler,
        IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DefaultRequestEventDelegator>();
        _distributedCache = distributedCache;
        _deviceHolder = deviceHolder;
        _workOrderTaskAdapter = workOrderTaskAdapter;
        _scheduleTaskAdapter = scheduleTaskAdapter;
        _sysConfigManager = sysConfigManager;
        _taskScheduler = taskScheduler;
        _taskScheduleOptions = serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>().Value;
    }

    public async Task<DeviceEventReportResponse> HandleEvent(DeviceEventReportRequest request)
    {
        string eventTraceId;
        var routingKey = $"{request.ProductId}.{request.DeviceId}.event.panelSilo".ToLower();
        request.Params["routingKey"] = routingKey;

        if (!request.Params.ContainsKey("Spindles"))
        {
            await _distributedCache.RemoveAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"缺少设备参数，请检查配置文件：{request.DeviceId}/Extra/Spindles"
            };
        }

        if (!request.Params.ContainsKey("SpindleBehavior")
            || !request.Params.ContainsKey("ShelfInnerPos"))
        {
            await _distributedCache.RemoveAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"缺少运行参数，请检查：{request.DeviceId} Params/SpindleBehavior ShelfInnerPos"
            };
        }

        var convertedInteractionBehavior = (InteractionBehavior)request.RequestInteractionBehavior;
        if (convertedInteractionBehavior == null || convertedInteractionBehavior.DeviceKind == DeviceKind.Unknown)
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"{request.DeviceId} - Invalid request.RequestInteractionBehavior {request.RequestInteractionBehavior}, Cannot convert to InteractionBehavior"
            };
        }

        var calllerProxy = _deviceHolder.GetOnlineDevice(request.DeviceId);
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

        ////料仓中是全熟料时，必须存在unpin的呼叫进料仓请求时，才能呼叫料仓的下料事件。
        //if(convertedInteractionBehavior.MaterialKind == MaterialKind.PanelSilo
        //    && convertedInteractionBehavior.InteractionSequence == InteractionSequence.UnloadOnly
        //    //&& 存在unpin的呼叫
        //    )
        //{
        //}

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

        var exsistRoutingSchedule = await _scheduleTaskAdapter.HasUnstartedOrDoingScheduleTaskByRoutingKey(routingKey);

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

        return await _taskScheduler.ScheduleAGVDevcieViaMysql(request, calllerProxy);
    }
}
