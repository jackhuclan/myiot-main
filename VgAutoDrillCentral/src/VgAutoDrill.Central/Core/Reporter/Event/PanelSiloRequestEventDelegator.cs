using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal class PanelSiloRequestEventDelegator : IDeviceEventDelegator
{
    private readonly ILogger<DefaultRequestEventDelegator> _logger;
    private readonly IDistributedCache _distributedCache;
    private readonly IDeviceManager _deviceManager;
    private readonly ISiloAdapter _siloAdapter;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILocationManager _locationManager;

    public PanelSiloRequestEventDelegator(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<DefaultRequestEventDelegator>>();
        _distributedCache = serviceProvider.GetRequiredService<IDistributedCache>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
        _siloAdapter = serviceProvider.GetRequiredService<ISiloAdapter>();
        _sysConfigManager = serviceProvider.GetRequiredService<ISysConfigManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
    }

    public async Task<DeviceEventReportResponse> HandleEvent(DeviceEventReportRequest request)
    {
        if (!CentralFlags.SystemPreloadCompleted)
        {
            _logger.LogWarning($"系统已启用预加载模式，但是尚未加载完成，请等待.");
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"系统已启用预加载模式，但是尚未加载完成，请等待"
            };
        }

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
        {
            _logger.LogWarning($"系统即将停机维护");
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"系统即将停机维护"
            };
        }

        string eventTraceId;
        var locationCode = request.LocationCode;
        var routingKey = $"{request.ProductId}.{request.DeviceId}.event.panelSilo.{locationCode}".ToLower();
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
            || !request.Params.ContainsKey("ShelfInnerPos")
            || !request.Params.ContainsKey("TransSpindles")
            || !request.Params.ContainsKey("TransShelfInnerPos")
            )
        {
            await _distributedCache.RemoveAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"缺少运行参数，请检查：{request.DeviceId} Params/SpindleBehavior ShelfInnerPos TransSpindles TransShelfInnerPos"
            };
        }

        if (!_locationManager.TryGetLocation(locationCode, out var location)
            || location == null)
        {
            await _distributedCache.RemoveAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"尚未创建此库位，或者刚创建请稍侯,[{locationCode}]，暂时不能发起呼叫。请检查中控后台管理系统。"
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

        var calllerProxy = _deviceManager.GetOnlineDevice(request.DeviceId);
        if (calllerProxy == null)
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = $"{ErrorCodes.Sys.WRONG_DEVICE_MESSAGE} - {request.DeviceId}"
            };
        }

        //中转位或者料架暂存区 识别到混收熟料时，禁止发出调度申请
        if (request.RequestDeviceKind == DeviceKind.PanelSiloFork || request.RequestDeviceKind == DeviceKind.PublicPanelSiloWIP)
        {
            if (request.PayloadPanels.DrilledItemCodes.Count() > 1)
            {
                _logger.LogError($"{request.LocationCode} - 识别到库位料仓含有不同的熟料，已禁止发起调度申请 -- PanelSiloRequestEventDelegator");
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"{request.LocationCode} - 识别到库位料仓含有不同的熟料，已禁止发起调度申请"
                };
            }

            if (request.PayloadPanels.Any(x => x.ProductStatus == ProductStatus.EmptyPayload)
                && request.PayloadPanels.Any(x => x.ProductStatus > 0))
            {
                _logger.LogError($"{request.LocationCode} - 板料状态维护错误，不能同时含有空负载和非空负载的板料，已禁止发起调度申请 -- PanelSiloRequestEventDelegator");
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"{request.LocationCode} - 板料状态维护错误，不能同时含有空负载和非空负载的板料，已禁止发起调度申请"
                };
            }

            if (request.PayloadPanels.Any(x => string.IsNullOrEmpty(x.SiloCode) || request.LocationCode.ToLower() == x.SiloCode.ToLower())
                && request.PayloadPanels.Any(x => x.ProductStatus > 0))
            {
                _logger.LogError($"{request.LocationCode} - 板料状态维护错误，含有非空负载的板料时，必须指定料仓号，已禁止发起调度申请 -- PanelSiloRequestEventDelegator");
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"{request.LocationCode} - 板料状态维护错误，含有非空负载的板料时，必须指定料仓号，已禁止发起调度申请"
                };
            }
        }

        var traceId = await _distributedCache.GetStringAsync(routingKey);
        if (!string.IsNullOrEmpty(traceId))
        {
            var IsActive = _scheduleTaskManager.HasUncompletedTaskOfRoutingKey(routingKey);
            if (IsActive)
            {
                _logger.LogInformation($"{ErrorCodes.Sys.DUPLICATE_EVENT_SCHEDULE_REQUEST_MESSAGE},{locationCode}");
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
                _logger.LogWarning($"当前存在过期的调度申请,{locationCode}，已重置redis value");
            }
        }

        if ((request.RequestDeviceKind == DeviceKind.PanelSiloFork
                || request.RequestDeviceKind == DeviceKind.PublicPanelSiloWIP
                || request.RequestDeviceKind == DeviceKind.UnPin
                || request.RequestDeviceKind == DeviceKind.Pin)
                &&
            (string.IsNullOrEmpty(request.LocationCode) || !request.Params.ContainsKey("SiloCode"))
        )
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"缺少运行参数，请检查：{request.LocationCode},{request.EventId},Params/DeviceCode SiloCode"
            };
        }

        if (DeviceKindExtensions.IsAuxiliary(request.RequestDeviceKind)
            && request.Params.ContainsKey("IsAuxiliary")
            && !request.Params["IsAuxiliary"].ToBool()
            && (!request.Params.ContainsKey("AssignAGV") || string.IsNullOrEmpty(request.Params["AssignAGV"].ToStr())))
        {
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"辅助设备主叫AGV时，缺少运行参数，请检查：{request.LocationCode},{request.EventId},Params/AssignAGV"
            };
        }

        var siloCode = request.Params["SiloCode"].ToStr();

        if (request.Params.ContainsKey("AssignAGV")
            && !string.IsNullOrEmpty(request.Params["AssignAGV"].ToStr())
            && request.Params.ContainsKey("ForceCall")
            && request.Params["ForceCall"].ToBool() == true)
        {
            var agv = _deviceManager.GetOnlineDevice(request.Params["AssignAGV"].ToStr());
            if (agv == null)
            {
                await _distributedCache.RemoveAsync(routingKey);
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"主叫设置的AGV {request.Params["AssignAGV"].ToStr()} ，当前不在线；[{locationCode}]，[{request.RequestDeviceKind}]，不能发起呼叫。"
                };
            }
            if (convertedInteractionBehavior.DeviceKind == DeviceKind.PublicPanelSiloWIP
                && (agv.DeviceKind == DeviceKind.BackPanelAgv || agv.DeviceKind == DeviceKind.FrontPanelAgv))
            {
                await _distributedCache.RemoveAsync(routingKey);
                return new DeviceEventReportResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"主叫设置的AGV {request.Params["AssignAGV"].ToStr()}，agv.DeviceKind {agv.DeviceKind} ，与当前设备不匹配；[{locationCode}]，[{request.RequestDeviceKind}]，不能发起呼叫。"
                };
            }
        }

        //上下料仓时，设备都必须处于启用状态
        if (location.Status != 1)
        {
            await _distributedCache.RemoveAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"此库位被禁用,[{locationCode}]，暂时不能发起呼叫。请检查中控后台管理系统。"
            };
        }

        request.PartitionCode = location.PartitionCode;

        //下料仓时，还需要料仓不能是手动
        //料仓在AGV上时，以及AGV正在上下料仓过程当中，料仓是Auto Control状态
        if (convertedInteractionBehavior.InteractionSequence == InteractionSequence.UnloadOnly
                && SiloStatus.Ready != await _siloAdapter.GetSiloStatus(siloCode))
        {
            await _distributedCache.RemoveAsync(routingKey);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"暂存设备上的料仓，当前被禁用,[{locationCode}]，type[{request.RequestDeviceKind}]，silo code[{siloCode}],不能发起呼叫。请检查中控后台管理系统。"
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
            _logger.LogInformation($"当前存在在途的调度申请,{locationCode}");
            await _distributedCache.SetStringAsync(routingKey, eventTraceId);
            return new DeviceEventReportResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = $"当前存在在途的调度申请,{locationCode}",
                TraceId = ""
            };
        }

        var taskCode = string.Empty;        // 下个待生产的任务代号
        var taskItemCode = string.Empty;    // 任务中的物料编码
        int taskItemCount = 0;              // 任务中的领取物料数量

        request.Params[ScheduleConstants.PARAMS_TASK_ID] = taskCode;
        request.Params[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = taskItemCode;
        request.Params[ScheduleConstants.PARAMS_TASK_ITEM_COUNT] = taskItemCount;

        return await _scheduleTaskManager.ScheduleAGVDevcieViaMysql(request, calllerProxy);
    }
}
