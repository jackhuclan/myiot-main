using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Cache.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal class PanelRequestEventDelegator : IDeviceEventDelegator
{
    private readonly ILogger<PanelRequestEventDelegator> _logger;
    private readonly IDeviceManager _deviceHolder;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly IScheduleTaskAdapter _scheduleTaskAdapter;
    private readonly IDeviceAdapter _deviceAdapter;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IRouteProcessAndWorkStationService _routeProcessAndWorkStationService;
    private readonly IDistributedCache _distributedCache;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IMemoryCacheManager _memoryCacheManager;

    public PanelRequestEventDelegator(IDeviceManager deviceHolder,
        IWorkOrderTaskAdapter workOrderTaskAdapter,
        IScheduleTaskAdapter scheduleTaskAdapter,
        IDeviceAdapter deviceAdapter,
        ISysConfigManager sysConfigManager,
        IRouteProcessAndWorkStationService routeProcessAndWorkStationService,
        IDistributedCache distributedCache,
        IScheduleTaskManager taskScheduler,
        IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory)
    {
        _deviceHolder = deviceHolder;
        _logger = loggerFactory.CreateLogger<PanelRequestEventDelegator>();
        _workOrderTaskAdapter = workOrderTaskAdapter;
        _scheduleTaskAdapter = scheduleTaskAdapter;
        _deviceAdapter = deviceAdapter;
        _sysConfigManager = sysConfigManager;
        _routeProcessAndWorkStationService = routeProcessAndWorkStationService;
        _distributedCache = distributedCache;
        _scheduleTaskManager = taskScheduler;
        _memoryCacheManager = serviceProvider.GetRequiredService<IMemoryCacheManager>();
    }

    public async Task<DeviceEventReportResponse> HandleEvent(DeviceEventReportRequest request)
    {
        if (!CentralFlags.SystemPreloadCompleted)
        {
            _logger.LogWarning($"系统已启用预加载模式，但是尚未加载完成，请等待.");
            //calllerProxy.Properties["CallAgvMessage"] = $"系统已启用预加载模式，但是尚未加载完成，请等待.";
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"系统已启用预加载模式，但是尚未加载完成，请等待"
            };
        }

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
        {
            _logger.LogWarning($"系统即将停机维护");
            //calllerProxy.Properties["CallAgvMessage"] = $"系统即将停机维护.";
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"系统即将停机维护"
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

        if (convertedInteractionBehavior.InteractionSequence != InteractionSequence.LoadOnly
            && request.PayloadPanels.Any(x => x.PanelCode.Contains("mock")
                && (x.ItemCode.Contains("11111") || x.ItemCode.Contains("22222")))
            && await _sysConfigManager.GetBoolValue("RejectDrillMockClicker11111"))
        {
            var mockItem = request.PayloadPanels.FirstOrDefault(x => x.PanelCode.Contains("mock")
                && (x.ItemCode.Contains("11111") || x.ItemCode.Contains("22222")));
            _logger.LogWarning($"{request.DeviceId}-存在疑似手动上板,物料号（{mockItem?.ItemCode}），请处理！！！");

            //CallAgvMessage
            if (calllerProxy != null)
            {
                calllerProxy.Properties["CallAgvMessage"] = $"存在疑似手动上板,物料号（{mockItem?.ItemCode}），请处理！！！";
            }

            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"存在疑似手动上板,物料号（{mockItem?.ItemCode}），请处理！！！"
            };
        }
        else
        {
            //CallAgvMessage
            if (calllerProxy != null)
            {
                calllerProxy.Properties["CallAgvMessage"] = $"";
            }
        }

        if (calllerProxy == null)
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = $"{ErrorCodes.Sys.WRONG_DEVICE_MESSAGE} - {request.DeviceId}"
            };
        }

        string eventTraceId;
        var routingKey = $"{request.ProductId}.{request.DeviceId}.event.{request.EventId}".ToLower();
        request.Params["routingKey"] = routingKey;

        var traceId = await _distributedCache.GetStringAsync(routingKey);
        if (!string.IsNullOrEmpty(traceId))
        {
            var isActive = _scheduleTaskManager.HasUncompletedTaskOfRoutingKey(routingKey);
            if (isActive)
            {
                _logger.LogInformation($"{routingKey}:当前存在在途的调度申请,{request.LocationCode}");
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.SUCCESS,
                    Message = $"info:{ErrorCodes.Sys.DUPLICATE_EVENT_SCHEDULE_REQUEST_MESSAGE},device:{request.DeviceId}",
                    TraceId = traceId
                };
            }
            else
            {
                await _distributedCache.RemoveAsync(routingKey);
                _logger.LogWarning($"{routingKey}:当前存在过期的调度申请,{request.LocationCode}，已重置redis value");
            }
        }

        if (_memoryCacheManager.Exists($"{request.LocationCode}-panel-request"))
        {
            _logger.LogWarning($"小于呼叫间隔，请等待");
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"小于呼叫间隔，请等待"
            };
        }
        _memoryCacheManager.SetBySeconds($"{request.LocationCode}-panel-request", DateTime.Now, 5);

        //检查设备是否已停用
        var device = await _deviceAdapter.GetDevice(request.DeviceId);
        if (device == null)
        {
            _logger.LogWarning($"找不到设备,{request.DeviceId}");
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"找不到设备,{request.DeviceId}"
            };
        }

        if (device.Status != 1)
        {
            _logger.LogWarning($"设备已禁用,{request.DeviceId}");
            calllerProxy.Properties["CallAgvMessage"] = $"设备已禁用.";
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"设备已禁用,{request.DeviceId}"
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

        var exsistRoutingSchedule = _scheduleTaskManager.HasUncompletedTaskOfRoutingKey(routingKey);

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
        var routeCode = string.Empty;       // 任务中的工艺路线编号
        if (DeviceKindExtensions.IsDrill(calllerProxy.Descriptor.DeviceKind))
        {
            if (!request.Params.ContainsKey("RawSpindleNum")
                || !request.Params.ContainsKey("SpindleUseNum")
                || !request.Params.ContainsKey("ExistRawNum"))
            {
                await _distributedCache.RemoveAsync(routingKey);
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"{request.DeviceId},缺少运行参数Params/RawSpindleNum,SpindleUseNum,ExistRawNum"
                };
            }

            //本次申请的生料数量 RawSpindleNum
            var rawSpindleNum = request.Params.ContainsKey("RawSpindleNum") ? request.Params["RawSpindleNum"].ToInt() : 0;
            //已有的生料数量
            var existRawNum = request.Params.ContainsKey("ExistRawNum") ? request.Params["ExistRawNum"].ToInt() : 0;
            //当前使用的轴数
            var spindleUseNum = request.Params.ContainsKey("SpindleUseNum") ? request.Params["SpindleUseNum"].ToInt() : 0;
            _logger.LogDebug($"本次申请的生料数量：{rawSpindleNum},已有的生料数量:{existRawNum},当前使用的轴数:{spindleUseNum}");
            // 呼叫仅下料时，不检查生产任务
            if (convertedInteractionBehavior.InteractionSequence != InteractionSequence.UnloadOnly)
            {
                var undrilledItemCodesFromDrill = request.PayloadPanels.UndrilledItemCodes;

                //传入本次本次呼叫的数量，当前已有的生料数量，当前使用的轴数
                //查找已提交任务，以及派送中的任务，兼容上料 部分完成的情况
                var applyTaskRequest = new WorkOrderTaskRequest
                {
                    DeviceId = request.DeviceId,
                    RealNeedCount = rawSpindleNum,
                    ExistRawNum = existRawNum,
                    SpindleUseNum = spindleUseNum,
                };
                applyTaskRequest.UndrilledItemCodesFromDrill.AddRange(undrilledItemCodesFromDrill);

                var response = await _workOrderTaskAdapter.ApplyNextWorkOrderTask(applyTaskRequest);
                _logger.LogInformation($"{request.DeviceId},没有合适的生产任务,原因是：{response.Message}。");

                var task = response.Data;

                if (task != null && !string.IsNullOrEmpty(task.Code))
                {
                    _logger.LogInformation($"taskService.GetNextWorkOrderTask(request.DeviceId),{request.DeviceId},{task.Code}，{task.ItemCode}");

                    //判定钻机此时已有生料，与任务中的计划任务是否一致；
                    //如果任务中分派的就是尾料，那么不处理此次呼叫；并返回消息，通知钻机已经上料结束；
                    if (existRawNum < spindleUseNum && existRawNum == task.NowWadCount)
                    {
                        _logger.LogInformation($"当前已有生料[{existRawNum}]，已经满足任务安排的数量[{task.NowWadCount}]！取消上生料任务！");

                        request.Params["RawSpindleNum"] = 0;

                        //_redisClient.ResetRoutingKey(routingKey);
                        //return new DeviceEventReportResponse
                        //{
                        //    Code = ErrorCodes.Sys.UNNEED_CALL_RAW_PANEL,
                        //    Message = $"当前已有生料[{existRawNum}]，已经满足任务安排的数量[{task.NowWadCount}]！不需要再发起叫料！",
                        //    TraceId = ""
                        //};
                    }

                    var exsistCompletedSchedule = await _scheduleTaskAdapter.HasCompletedScheduleWithNotStartedWorkTask(task.Code);

                    if (exsistCompletedSchedule)
                    {
                        _logger.LogInformation($"此生产任务{task.Code},尚未设置 Buffer就绪,请稍后再申请！");
                        await _distributedCache.RemoveAsync(routingKey);

                        _ = _workOrderTaskAdapter.SetBufferReady(task.Code);

                        return new DeviceEventReportResponse
                        {
                            Code = ErrorCodes.Sys.FAIL,
                            Message = $"此生产任务{task.Code}尚未设置 Buffer就绪,请稍后再申请！",
                            TraceId = ""
                        };
                    }

                    taskCode = task.Code;
                    taskItemCode = task.ItemCode?.Trim();
                    taskItemCount = Convert.ToInt32(task.NowWadCount);
                }
                // 呼叫只上料时，必须要有生产任务，否则不能呼叫
                else if (convertedInteractionBehavior.InteractionSequence == InteractionSequence.LoadOnly)
                {
                    await _distributedCache.RemoveAsync(routingKey);
                    return new DeviceEventReportResponse
                    {
                        Code = ErrorCodes.Sys.FAIL,
                        Message = $"{request.DeviceId},没有合适的生产任务,原因是：{response.Message}",
                        TraceId = ""
                    };
                }
            }

            var routes = await _routeProcessAndWorkStationService.GetRoutesByWorkStation(new Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation.GetRouteProcessAndWorkStationListReq
            {
                WorkStationCode = request.DeviceId,
            });

            if (routes != null
                && routes.Data.List.Any())
            {
                routeCode = routes.Data.List[0].Code;
            }
            else
            {
                await _distributedCache.RemoveAsync(routingKey);
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"{request.DeviceId},钻机没有配置工艺路线，不能发起调度",
                    TraceId = ""
                };
            }
        }

        request.Params[ScheduleConstants.PARAMS_TASK_ID] = taskCode;
        request.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = taskItemCode;
        request.Params[ScheduleConstants.PARAMS_TASK_ITEM_COUNT] = taskItemCount;
        request.Params[ScheduleConstants.PARAMS_TASK_ROUTE_CODE] = routeCode;

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
