using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Schedule.Deliver;

internal class CutterSiloShelfScheduleTaskDeliverPolicy : IScheduleTaskDeliverPolicy
{
    private readonly ILogger<CutterSiloShelfScheduleTaskDeliverPolicy> _logger;
    private readonly MysqlTaskSchedulerOptions _taskScheduleOptions;
    private readonly IScheduleService _schedulementService;

    public CutterSiloShelfScheduleTaskDeliverPolicy(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetRequiredService<ILogger<CutterSiloShelfScheduleTaskDeliverPolicy>>();
        _taskScheduleOptions = (serviceProvider.GetRequiredService<IOptions<MysqlTaskSchedulerOptions>>()).Value;
        _schedulementService = serviceProvider.GetRequiredService<IScheduleService>();
    }

    public async Task<AgvAllocationResult> DeliverScheduleTask(DeliverScheduleTaskRequirement scheduleTaskRequirement)
    {
        DeviceProxy callerDevice = scheduleTaskRequirement.CallerDevice;
        var agvDevice = scheduleTaskRequirement.AgvDevice;
        DeviceEventReportRequest eventRequest = scheduleTaskRequirement.ScheduleTask.EventRequest;
        ScheduleTask scheduleTask = scheduleTaskRequirement.ScheduleTask;

        var eventTraceId = eventRequest.TraceId;
        _logger.LogWarning($"ScheduleDbCheck检查通过，开始分配,current eventTraceId:{eventTraceId},source device:{eventRequest.DeviceId}");

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
            PayloadCutterTrays = eventRequest.PayloadCutterTrays,
            LocationCode = eventRequest.LocationCode,
        };
        serviceRequest.Params[ScheduleConstants.PARAMS_TASK_SENDED_TIME] = DateTime.MinValue;
        serviceRequest.Params[ScheduleConstants.PARAMS_TASK_EVENT_TRACE_ID] = eventTraceId;

        _logger.LogInformation($"device {agvDevice.Descriptor.ProductId}-{agvDevice.Descriptor.DeviceId}-DelayedEventConsumer begin to call ScheduleTask,");
        //redisClient.SaveDeviceEventReportRequest(eventRequest);
        var response = await agvDevice.InvokeService(serviceRequest);

        if (response != null && response.Code == ErrorCodes.Sys.SUCCESS)
        {
            await UpdateAgvScheduleLog(eventTraceId, agvDevice);
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()} {eventRequest.ProductId}-{eventRequest.DeviceId}  AGV调用请求 已下发给 {agvDevice.Descriptor.DeviceId}！");
            return new AgvAllocationResult(AgvAllocationResultCode.Success, AgvAllocationFailedReason.Completed);
        }
        else
        {
            //任务分发失败，释放AGV标志
            agvDevice.TargetDevice = string.Empty;
        }

        return new AgvAllocationResult(AgvAllocationResultCode.Failed, AgvAllocationFailedReason.Failed);
    }

    private async Task UpdateAgvScheduleLog(string eventTraceId, DeviceProxy agvDevice)
    {
        if (!string.IsNullOrEmpty(eventTraceId))
        {
            var changedSpindles = agvDevice.Properties["changedSpindles"].ToStr();
            var changedBehavior = agvDevice.Properties["changedBehavior"].ToStr();
            var taskCode = agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ID].ToStr();
            var itemCode = agvDevice.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE].ToStr();

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
            };

            var response = await _schedulementService.UpdateCentralTask(updateDto);

            _logger.LogWarning($"response for traceid :{eventTraceId},:{response}");
        }
    }
}
