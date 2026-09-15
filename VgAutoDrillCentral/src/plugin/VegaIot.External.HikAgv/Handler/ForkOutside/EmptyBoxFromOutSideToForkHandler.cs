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
/// 空料仓从外部空仓区运转到内部中转位
/// </summary>
public class EmptyBoxFromOutSideToForkHandler
{
    private readonly ILogger<EmptyBoxFromOutSideToForkHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationManager _locationManager;
    private readonly IObjectFactory _objectFactory;

    public EmptyBoxFromOutSideToForkHandler(IServiceProvider serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<EmptyBoxFromOutSideToForkHandler>();

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

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromOutSideToForkHandler,task:{agvTask.Id},Start Excute Handle==========================\r\n");

        var deviceValidator = _objectFactory.GetOrCreate<DeviceValidator>();
        var deviceValidationResult = deviceValidator.Validate(agvTask.EndDeviceId);
        if (deviceValidationResult.code == ERR_CODE)
        {
            return deviceValidationResult;
        }

        var locationValidator = _objectFactory.GetOrCreate<LocationValidator>();
        var locationValidationResult = locationValidator.Validate(agvTask.EndLocationCode);
        if (locationValidationResult.code == ERR_CODE)
        {
            return locationValidationResult;
        }

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromOutSideToForkHandler,task:{agvTask.Id},The verification of device and locations has been passed.\r\n");

        var endDevice = _deviceHolder.GetOnlineDevice(agvTask.EndDeviceId!);
        var endLocation = _locationManager.GetLocation(agvTask.EndLocationCode!);

        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && endDevice != null
            && endLocation != null
            && endSchedule.Code != null
            && endSchedule.EventRequest != null)
        {
            var panels = endSchedule.EventRequest.PayloadPanels;
            panels.SetEmpty("", status.podCode!);

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromOutSideToForkHandler,task:{agvTask.Id},Set Panels is Empty.\r\n");

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

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromOutSideToForkHandler,task:{agvTask.Id},Set Schedule Finished.\r\n");

            await Task.Delay(500);

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
                TargetHostAddress = endDevice.Descriptor.HostAddress,
                EventId = endSchedule.EventRequest.EventId,
                Params = new Dictionary<string, object?>
                {
                    { "LoadingPanel", new  { PanelList = panels }},
                    { "ShelfIndex",endLocation.Index},
                }
            });

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromOutSideToForkHandler,task:{agvTask.Id},Device Invoke Service.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = endSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status.robotCode}"
            });

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromOutSideToForkHandler,task:{agvTask.Id},Add Schedule Log Request.\r\n");

            agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
            agvTask.CompletedTime = DateTime.Now;
            await _transferPlanManager.TryUpdateTransferJob(agvTask);

            _logger.LogInformation($"HikAgvHandler - EmptyBoxFromOutSideToForkHandler,task:{agvTask.Id},Update Database agvTask.\r\n");
        }

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromOutSideToForkHandler,task:{agvTask.Id},End Excute Handle==========================\r\n");

        return response;
    }
}
