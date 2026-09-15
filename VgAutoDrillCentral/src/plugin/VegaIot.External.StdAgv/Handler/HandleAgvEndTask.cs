using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VegaIot.External.StdAgv.Handler;

internal class HandleAgvEndTask
{
    private readonly ILogger<HandleAgvEndTask> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;

    public HandleAgvEndTask(ILogger<HandleAgvEndTask> logger,
        IScheduleTaskManager scheduleTaskManager,
        ITransferPlanManager transferPlanManager)
    {
        _logger = logger;
        _scheduleTaskManager = scheduleTaskManager;
        _transferPlanManager = transferPlanManager;
    }

    public async Task Handle(TransferJob agvTask,
        String status,
        DeviceProxy endDevice,
        Location endLocation,
        string operationName,
        PanelList panelList)
    {
        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && endSchedule.Code != null
            && endSchedule.EventRequest != null)
        {
            if (agvTask.StartScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
                && startSchedule != null
                && startSchedule.EventRequest != null)
            {
                await endDevice.InvokeService(new DeviceServiceInvokeRequest
                {
                    ProductId = endDevice.ProductId,
                    DeviceId = endDevice.DeviceId,
                    ClientId = endDevice.ClientId,
                    HostAddress = endDevice.Descriptor.HostAddress,
                    ServiceId = Topics.Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID,
                    ServiceName = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
                    TargetProductId = endDevice.ProductId,
                    TargetDeviceId = endDevice.DeviceId,
                    TargetClientId = endDevice.ClientId,
                    EventId = endSchedule.EventRequest.EventId,
                    TargetHostAddress = endDevice.Descriptor.HostAddress,
                    Params = new Dictionary<string, object?>
                    {
                        //{ "LoadingPanel", new { PanelList =panelList}},
                        { "LoadingPanel", new { PanelList =startSchedule.EventRequest.PayloadPanels}},
                        { "ShelfIndex", endLocation.Index},
                    },
                    CallerRequestInputProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1
                });

                _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},End Device Invoke Service.\r\n");
            }

            await Task.Delay(500);

            await _scheduleTaskManager.SetScheduleAsFinished(new CompleteScheduleTaskRequest
            {
                ProductId = endDevice.ProductId,
                DeviceId = endDevice.DeviceId,
                TraceId = endSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    { "routingKey", endSchedule.RoutingKey }
                }
            }, null);

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},End Device Set Schedule Finished.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = endSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status}"
            });

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},End Device Add Schedule Log Request.\r\n");

            agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
            agvTask.CompletedTime = DateTime.Now;
            await _transferPlanManager.TryUpdateTransferJob(agvTask);

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},End Device Update Database agvTask.\r\n");
        }
    }
    public async Task Handle(TransferJob agvTask,
        StdArrivedRequestEntityV2 status,
        DeviceProxy endDevice,
        Location endLocation,
        string operationName)
    {
        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && endSchedule.Code != null
            && endSchedule.EventRequest != null)
        {
            if (agvTask.StartScheduleId.HasValue
                && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
                && startSchedule != null
                && startSchedule.EventRequest != null)
            {
                await endDevice.InvokeService(new DeviceServiceInvokeRequest
                {
                    ProductId = endDevice.ProductId,
                    DeviceId = endDevice.DeviceId,
                    ClientId = endDevice.ClientId,
                    HostAddress = endDevice.Descriptor.HostAddress,
                    ServiceId = Topics.Services.COMPLETE_LOAD_MATERIAL_SERVICE_ID,
                    ServiceName = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
                    TargetProductId = endDevice.ProductId,
                    TargetDeviceId = endDevice.DeviceId,
                    TargetClientId = endDevice.ClientId,
                    EventId = endSchedule.EventRequest.EventId,
                    TargetHostAddress = endDevice.Descriptor.HostAddress,
                    Params = new Dictionary<string, object?>
                    {
                        { "LoadingPanel", new { PanelList =startSchedule.EventRequest.PayloadPanels}},
                        { "ShelfIndex", endLocation.Index},
                    },
                    CallerRequestInputProductStatus = ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1
                });

                _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},End Device Invoke Service.\r\n");
            }

            await Task.Delay(500);

            await _scheduleTaskManager.SetScheduleAsFinished(new CompleteScheduleTaskRequest
            {
                ProductId = endDevice.ProductId,
                DeviceId = endDevice.DeviceId,
                TraceId = endSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    { "routingKey", endSchedule.RoutingKey }
                }
            }, null);

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},End Device Set Schedule Finished.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = endSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status.robotCode}"
            });

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},End Device Add Schedule Log Request.\r\n");

            agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
            agvTask.CompletedTime = DateTime.Now;
            await _transferPlanManager.TryUpdateTransferJob(agvTask);

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},End Device Update Database agvTask.\r\n");
        }
    }
}
