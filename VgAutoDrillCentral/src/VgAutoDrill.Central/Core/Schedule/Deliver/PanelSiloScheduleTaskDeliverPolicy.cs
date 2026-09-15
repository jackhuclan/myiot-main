using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Schedule.Deliver;

internal class PanelSiloScheduleTaskDeliverPolicy : PinAndUnpinBaseDeliverPolicy, IScheduleTaskDeliverPolicy
{
    private readonly ILogger<PanelSiloScheduleTaskDeliverPolicy> _logger;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;
    private readonly IDeviceManager _deviceHolder;
    private readonly IScheduleTaskManager _scheduleTaskManager;

    public PanelSiloScheduleTaskDeliverPolicy(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<PanelSiloScheduleTaskDeliverPolicy>>();
        _taskScheduleOptions = (serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>()).Value;
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
    }

    public async Task<AgvAllocationResult> DeliverScheduleTask(DeliverScheduleTaskRequirement scheduleTaskRequirement)
    {
        DeviceProxy callerDevice = scheduleTaskRequirement.CallerDevice;
        var agvDevice = scheduleTaskRequirement.AgvDevice;
        DeviceEventReportRequest eventRequest = scheduleTaskRequirement.ScheduleTask.EventRequest;
        ScheduleTaskWithRequest scheduleTask = scheduleTaskRequirement.ScheduleTask;

        var eventTraceId = eventRequest.TraceId;
        _logger.LogWarning($"PanelSilo PrepareScheduleTask，开始分配,current eventTraceId:{eventTraceId},source device:{eventRequest.DeviceId}");

        var convertedBehavior = (InteractionBehavior)eventRequest.RequestInteractionBehavior;
        if (convertedBehavior.InteractionSequence == InteractionSequence.UnloadOnly)
        {
            return await UnloadSilo(callerDevice, agvDevice, eventRequest, scheduleTask);
        }
        else if (convertedBehavior.InteractionSequence == InteractionSequence.LoadOnly)
        {
            return await LoadSilo(callerDevice, agvDevice, eventRequest, scheduleTask);
        }
        else
        {
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
        }
    }

    private async Task<AgvAllocationResult> UnloadSilo(DeviceProxy callerDevice, Agv agvDevice, DeviceEventReportRequest eventRequest, ScheduleTask scheduleTask)
    {
        //任务开始分发，锁定AGV为已分配，禁止其他线程
        agvDevice.TargetDevice = callerDevice.Descriptor.DeviceId;
        agvDevice.TargetLocation = scheduleTask.LocationCode;
        agvDevice.Properties["changedSpindles"] = "";
        agvDevice.Properties["changedBehavior"] = "";
        agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ID] = "";
        agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = "";
        eventRequest.Params["TotalPlanRawCount"] = 1;
        eventRequest.Params[ScheduleConstants.PARAMS_PLAN_RAW_COUNT] = 1;
        eventRequest.Params["IsAllPanelSent"] = true;
        eventRequest.Params["ExistRawNum"] = 0;

        var originSpindles = eventRequest.Params.ContainsKey("Spindles") ? eventRequest.Params["Spindles"].ToStr() : "";
        var originBehavior = eventRequest.Params.ContainsKey("SpindleBehavior") ? eventRequest.Params["SpindleBehavior"].ToStr() : "";
        _logger.LogWarning($"Spindles:{originSpindles},originSpindles.Count():{originSpindles.Count()},originBehavior.Count():{originBehavior.Count()} ");

        var serviceRequest = new DeviceServiceInvokeRequest
        {
            ProductId = eventRequest.ProductId,
            DeviceId = eventRequest.DeviceId,
            ClientId = eventRequest.ClientId,
            ServiceId = Topics.Services.SCHEDULE_TASK_SERVICE_ID,
            EventId = eventRequest.EventId,
            EventName = eventRequest.EventName,
            HostAddress = callerDevice.Descriptor.HostAddress,
            CallerRequestInteractionDirection = eventRequest.RequestInteractionDirection,
            CallerRequestMaterialKind = eventRequest.RequestMaterialKind,
            CallerRequestInteractionBehavior = eventRequest.RequestInteractionBehavior,
            CallerRequestInputProductStatus = eventRequest.RequestInputProductStatus,
            CallerRequestOutputProductStatus = eventRequest.RequestOutputProductStatus,
            //CallerInputCapabilities = callerDevice.Descriptor.InputCapabilities,
            //CallerOutputCapabilities = callerDevice.Descriptor.OutputCapabilities,
            TargetProductId = agvDevice.Descriptor.ProductId,
            TargetDeviceId = agvDevice.Descriptor.DeviceId,
            TargetClientId = agvDevice.ClientId,
            TargetHostAddress = agvDevice.Descriptor.HostAddress,
            Params = eventRequest.Params,
            PayloadPanels = eventRequest.PayloadPanels,
            LocationCode = eventRequest.LocationCode,
        };
        serviceRequest.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME] = DateTime.MinValue;
        serviceRequest.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID] = eventRequest.TraceId;

        _logger.LogInformation($"device {agvDevice.Descriptor.ProductId}-{agvDevice.Descriptor.DeviceId}-DelayedEventConsumer begin to call ScheduleTask,");

        if (callerDevice.DeviceKind == DeviceKind.PanelSiloFork
            || callerDevice.DeviceKind == DeviceKind.PublicPanelSiloWIP)
        {
            var responseActiveCall = await agvDevice.InvokeService(serviceRequest);
            if (responseActiveCall != null && responseActiveCall.Code == ErrorCodes.Sys.SUCCESS)
            {
                _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} {eventRequest.ProductId}-{eventRequest.DeviceId}  AGV调用请求 已下发给 {agvDevice.Descriptor.DeviceId}！");
                await _scheduleTaskManager.UpdateScheduleForChangingSilo(eventRequest.TraceId, agvDevice, "");

                return new AgvAllocationResult(AgvAllocationResultCode.Success, AgvAllocationFailedReason.Completed);
            }

            //任务分发失败，释放AGV标志
            agvDevice.TargetDevice = string.Empty;
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
        }

        return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
    }

    private async Task<AgvAllocationResult> LoadSilo(DeviceProxy callerDevice, Agv agvDevice, DeviceEventReportRequest eventRequest, ScheduleTask scheduleTask)
    {
        agvDevice.TargetDevice = callerDevice.Descriptor.DeviceId;
        agvDevice.TargetLocation = scheduleTask.LocationCode;
        agvDevice.Properties["changedSpindles"] = "";
        agvDevice.Properties["changedBehavior"] = "";
        agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ID] = "";
        agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = "";
        eventRequest.Params["TotalPlanRawCount"] = 1;
        eventRequest.Params[ScheduleConstants.PARAMS_PLAN_RAW_COUNT] = 1;
        eventRequest.Params["IsAllPanelSent"] = true;
        eventRequest.Params["ExistRawNum"] = 0;

        ScheduleDto? matchedSchedule = null;
        if (agvDevice.DeviceKind == DeviceKind.ShelfSiloAgv
                && agvDevice.PayloadPanels.IsEmptyPayload
                && scheduleTask.IsAuxiliary == false)
        {
            matchedSchedule = await GetMatchedSchedule(eventRequest);
            if (matchedSchedule == null)
            {
                agvDevice.TargetDevice = string.Empty;
                return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
            }

            var eventRequestRelateDevice = JsonSerializer.Deserialize<DeviceEventReportRequest>(matchedSchedule.RequestJson);
            if (eventRequestRelateDevice == null)
            {
                agvDevice.TargetDevice = string.Empty;
                _logger.LogWarning($"FailedToConvertRequestJson,{matchedSchedule.RequestJson}.");
                return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.FailedToConvertRequestJson);
            }
            var callerDeviceRelate = _deviceHolder.GetOnlineDevice(eventRequestRelateDevice.DeviceId);
            if (callerDeviceRelate == null)
            {
                agvDevice.TargetDevice = string.Empty;
                _logger.LogWarning($"callerDeviceRelate,{eventRequestRelateDevice.DeviceId}, is not online.");
                return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.CallerOffline);
            }
            agvDevice.TargetDevice = eventRequestRelateDevice.DeviceId;
            agvDevice.TargetLocation = eventRequestRelateDevice.LocationCode;
            //agvDevice.Properties["IsMaster"] = true;

            var serviceRequestRelateDevice = new DeviceServiceInvokeRequest
            {
                ProductId = eventRequestRelateDevice.ProductId,
                DeviceId = eventRequestRelateDevice.DeviceId,
                ClientId = eventRequestRelateDevice.ClientId,
                ServiceId = Topics.Services.SCHEDULE_TASK_SERVICE_ID,
                EventId = eventRequestRelateDevice.EventId,
                EventName = eventRequestRelateDevice.EventName,
                HostAddress = callerDeviceRelate.Descriptor.HostAddress,
                CallerRequestInteractionDirection = eventRequestRelateDevice.RequestInteractionDirection,
                CallerRequestMaterialKind = eventRequestRelateDevice.RequestMaterialKind,
                CallerRequestInteractionBehavior = eventRequestRelateDevice.RequestInteractionBehavior,
                CallerRequestInputProductStatus = eventRequestRelateDevice.RequestInputProductStatus,
                CallerRequestOutputProductStatus = eventRequestRelateDevice.RequestOutputProductStatus,
                CallerInputCapabilities = callerDeviceRelate.Descriptor.InputCapabilities,
                CallerOutputCapabilities = callerDeviceRelate.Descriptor.OutputCapabilities,
                TargetProductId = agvDevice.Descriptor.ProductId,
                TargetDeviceId = agvDevice.Descriptor.DeviceId,
                TargetClientId = agvDevice.ClientId,
                TargetHostAddress = agvDevice.Descriptor.HostAddress,
                Params = eventRequestRelateDevice.Params,
                PayloadPanels = eventRequestRelateDevice.PayloadPanels,
                LocationCode = eventRequestRelateDevice.LocationCode,
            };
            serviceRequestRelateDevice.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME] = DateTime.MinValue;
            serviceRequestRelateDevice.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID] = eventRequestRelateDevice.TraceId;

            _logger.LogInformation($"device {agvDevice.Descriptor.ProductId}-{agvDevice.Descriptor.DeviceId}- begin to call ScheduleTask,");
            var responseAboutRelateDevice = await agvDevice.InvokeService(serviceRequestRelateDevice);
            if (responseAboutRelateDevice != null && responseAboutRelateDevice.Code == ErrorCodes.Sys.SUCCESS)
            {
                //todo//var schedulePath = $"调度路线：{matchedSchedule.SubDeviceCode}[{matchedSchedule.Id}]==>{loadSchedule.LocationCode}[{loadSchedule.Id}]";

                _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} {eventRequestRelateDevice.ProductId}-{eventRequestRelateDevice.DeviceId}  AGV调用请求 已下发给 {agvDevice.Descriptor.DeviceId}！");
                await _scheduleTaskManager.UpdateScheduleForChangingSilo(eventRequestRelateDevice.TraceId, agvDevice, "");
                await _scheduleTaskManager.SpecifyFollowedSchedule(scheduleTask.Code, agvDevice.DeviceId, eventRequestRelateDevice.TraceId, "");

                return new AgvAllocationResult(AgvAllocationResultCode.Success, AgvAllocationFailedReason.Completed);
            }

            agvDevice.TargetDevice = string.Empty;
            return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
        }

        var serviceRequest = new DeviceServiceInvokeRequest
        {
            ProductId = eventRequest.ProductId,
            DeviceId = eventRequest.DeviceId,
            ClientId = eventRequest.ClientId,
            ServiceId = Topics.Services.SCHEDULE_TASK_SERVICE_ID,
            EventId = eventRequest.EventId,
            EventName = eventRequest.EventName,
            HostAddress = callerDevice.Descriptor.HostAddress,
            CallerRequestInteractionDirection = eventRequest.RequestInteractionDirection,
            CallerRequestMaterialKind = eventRequest.RequestMaterialKind,
            CallerRequestInteractionBehavior = eventRequest.RequestInteractionBehavior,
            CallerRequestInputProductStatus = eventRequest.RequestInputProductStatus,
            CallerRequestOutputProductStatus = eventRequest.RequestOutputProductStatus,
            //CallerInputCapabilities = callerDevice.Descriptor.InputCapabilities,
            //CallerOutputCapabilities = callerDevice.Descriptor.OutputCapabilities,
            TargetProductId = agvDevice.Descriptor.ProductId,
            TargetDeviceId = agvDevice.Descriptor.DeviceId,
            TargetClientId = agvDevice.ClientId,
            TargetHostAddress = agvDevice.Descriptor.HostAddress,
            Params = eventRequest.Params,
            PayloadPanels = eventRequest.PayloadPanels,
            LocationCode = eventRequest.LocationCode,
        };
        serviceRequest.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME] = DateTime.MinValue;
        serviceRequest.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID] = eventRequest.TraceId;

        _logger.LogInformation($"device {agvDevice.Descriptor.ProductId}-{agvDevice.Descriptor.DeviceId}- begin to call ScheduleTask,");

        var response = await agvDevice.InvokeService(serviceRequest);

        if (response != null && response.Code == ErrorCodes.Sys.SUCCESS)
        {
            //todo//var schedulePath = $"调度路线：{matchedSchedule.SubDeviceCode}[{matchedSchedule.Id}]==>{loadSchedule.LocationCode}[{loadSchedule.Id}]";

            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} {eventRequest.ProductId}-{eventRequest.DeviceId}  AGV调用请求 已下发给 {agvDevice.Descriptor.DeviceId}！");
            await _scheduleTaskManager.UpdateScheduleForChangingSilo(eventRequest.TraceId, agvDevice, "");

            return new AgvAllocationResult(AgvAllocationResultCode.Success, AgvAllocationFailedReason.Completed);
        }
        else
        {
            //任务分发失败，释放AGV标志
            agvDevice.TargetDevice = string.Empty;
        }

        return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
    }
}
