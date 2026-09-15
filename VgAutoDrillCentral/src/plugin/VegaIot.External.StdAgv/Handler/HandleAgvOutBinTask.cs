using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VegaIot.External.StdAgv.Handler;

internal class HandleAgvOutBinTask
{
    private readonly ILogger<HandleAgvOutBinTask> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;

    public HandleAgvOutBinTask(ILogger<HandleAgvOutBinTask> logger,
        IScheduleTaskManager scheduleTaskManager)
    {
        _logger = logger;
        _scheduleTaskManager = scheduleTaskManager;
    }

    public async Task Handle(TransferJob agvTask,
        String status,
        DeviceProxy startDevice,
        Location startLocation,
        string message,
        string operationName)
    {
        if (agvTask.StartScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
            && startSchedule != null
            && startSchedule.Code != null
            && startSchedule.EventRequest != null)
        {
            await startDevice.InvokeService(new DeviceServiceInvokeRequest
            {
                ProductId = startDevice.ProductId,
                DeviceId = startDevice.DeviceId,
                ClientId = startDevice.ClientId,
                HostAddress = startDevice.Descriptor.HostAddress,
                ServiceId = Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID,
                ServiceName = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
                TargetProductId = startDevice.ProductId,
                TargetDeviceId = startDevice.DeviceId,
                TargetClientId = startDevice.ClientId,
                EventId = startSchedule.EventRequest.EventId,
                TargetHostAddress = startDevice.Descriptor.HostAddress,
                Params = new Dictionary<string, object?>
                {
                    { "ShelfIndex",startLocation.Index}
                }
            });

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},Start Device Invoke Service.\r\n");

            await Task.Delay(500);

            await _scheduleTaskManager.SetScheduleAsFinished(new CompleteScheduleTaskRequest
            {
                ProductId = startDevice.ProductId,
                DeviceId = startDevice.DeviceId,
                TraceId = startSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    { "routingKey", startSchedule.RoutingKey }
                },
                Message = $"{message}"
            }, null);

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},Start Device Set Schedule Finished.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = startSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status}"
            });

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},Start Device Add Schedule Log.\r\n");
        }
    }
    public async Task Handle(TransferJob agvTask,
        StdArrivedRequestEntityV2 status,
        DeviceProxy startDevice,
        Location startLocation,
        string message,
        string operationName)
    {
        if (agvTask.StartScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
            && startSchedule != null
            && startSchedule.Code != null
            && startSchedule.EventRequest != null)
        {
            await startDevice.InvokeService(new DeviceServiceInvokeRequest
            {
                ProductId = startDevice.ProductId,
                DeviceId = startDevice.DeviceId,
                ClientId = startDevice.ClientId,
                HostAddress = startDevice.Descriptor.HostAddress,
                ServiceId = Topics.Services.COMPLETE_UNLOAD_MATERIAL_SERVICE_ID,
                ServiceName = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
                TargetProductId = startDevice.ProductId,
                TargetDeviceId = startDevice.DeviceId,
                TargetClientId = startDevice.ClientId,
                EventId = startSchedule.EventRequest.EventId,
                TargetHostAddress = startDevice.Descriptor.HostAddress,
                Params = new Dictionary<string, object?>
                {
                    { "ShelfIndex",startLocation.Index}
                }
            });

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},Start Device Invoke Service.\r\n");

            await Task.Delay(500);

            await _scheduleTaskManager.SetScheduleAsFinished(new CompleteScheduleTaskRequest
            {
                ProductId = startDevice.ProductId,
                DeviceId = startDevice.DeviceId,
                TraceId = startSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    { "routingKey", startSchedule.RoutingKey }
                },
                Message = $"{message}"
            }, null);

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},Start Device Set Schedule Finished.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = startSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status.robotCode}"
            });

            _logger.LogInformation($"StdAgvHandler - {operationName},task:{agvTask.Id},Start Device Add Schedule Log.\r\n");
        }
    }
}
