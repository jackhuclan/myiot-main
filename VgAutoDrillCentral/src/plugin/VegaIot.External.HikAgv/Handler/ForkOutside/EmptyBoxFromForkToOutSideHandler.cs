using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.Hik;
using VegaIot.External.HikAgv.Validator;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.HikAgv.Handler;

/// <summary>
/// 空料仓从内部中转位运转外部空仓区
/// </summary>
public class EmptyBoxFromForkToOutSideHandler
{
    private readonly ILogger<EmptyBoxFromForkToOutSideHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationManager _locationManager;
    private readonly IObjectFactory _objectFactory;

    public EmptyBoxFromForkToOutSideHandler(IServiceProvider serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromForkToOutSideHandler>();

        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _objectFactory = serviceProvider.GetRequiredService<IObjectFactory>();
    }

    public async Task<HikArrivedResponseEntity> Handle(TransferJob agvTask,
        HikArrivedRequestEntity status)
    {
        var response = new HikArrivedResponseEntity();

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromForkToOutSideHandler,task:{agvTask.Id},Start Excute Handle==========================\r\n");

        var deviceValidator = _objectFactory.GetOrCreate<DeviceValidator>();
        var deviceValidationResult = deviceValidator.Validate(agvTask.StartDeviceId);
        if (deviceValidationResult.code == ERR_CODE)
        {
            return deviceValidationResult;
        }

        var locationValidator = _objectFactory.GetOrCreate<LocationValidator>();
        var locationValidationResult = locationValidator.Validate(agvTask.StartLocationCode);
        if (locationValidationResult.code == ERR_CODE)
        {
            return locationValidationResult;
        }

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromForkToOutSideHandler,task:{agvTask.Id},The verification of device and locations has been passed.\r\n");

        var startDevice = _deviceHolder.GetOnlineDevice(agvTask.StartDeviceId!);
        var startLocation = _locationManager.GetLocation(agvTask.StartLocationCode!);

        if (agvTask.StartScheduleId.HasValue
             && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.StartScheduleId.Value, out var startSchedule)
             && startSchedule != null
             && startDevice != null
             && startLocation != null
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
                    { "ShelfIndex",startLocation.Index }
                }
            });

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromForkToOutSideHandler,task:{agvTask.Id},Device Invoke Service.\r\n");

            await Task.Delay(500);

            await _scheduleTaskManager.SetScheduleAsFinished(new CompleteScheduleTaskRequest
            {
                ProductId = startDevice.ProductId,
                DeviceId = startDevice.DeviceId,
                TraceId = startSchedule.Code,
                Params = new Dictionary<string, object?>
                {
                    { "routingKey", startSchedule.RoutingKey }
                }
            }, null);

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromForkToOutSideHandler,task:{agvTask.Id},Set Schedule Finished.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = startSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status.robotCode}"
            });

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromForkToOutSideHandler,task:{agvTask.Id},Add Schedule Log Request.\r\n");

            agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
            agvTask.CompletedTime = DateTime.Now;
            await _transferPlanManager.TryUpdateTransferJob(agvTask);

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromForkToOutSideHandler,task:{agvTask.Id},Update Database agvTask.\r\n");
        }

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromForkToOutSideHandler,task:{agvTask.Id},End Excute Handle==========================\r\n");

        return response;
    }
}
