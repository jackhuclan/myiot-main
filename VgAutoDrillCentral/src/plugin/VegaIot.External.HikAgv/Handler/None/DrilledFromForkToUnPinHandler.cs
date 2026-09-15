using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.Hik;
using VegaIot.External.HikAgv.Validator;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.HikAgv.Handler;

public class DrilledFromForkToUnPinHandler
{
    private readonly ILogger<DrilledFromForkToUnPinHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationManager _locationManager;
    private readonly IObjectFactory _objectFactory;

    public DrilledFromForkToUnPinHandler(ILogger<DrilledFromForkToUnPinHandler> logger,
        IScheduleTaskManager scheduleTaskManager,
        ITransferPlanManager transferPlanManager,
        IDeviceManager deviceHolder,
        ILocationManager locationManager,
        IObjectFactory objectFactory)
    {
        _logger = logger;
        _scheduleTaskManager = scheduleTaskManager;
        _transferPlanManager = transferPlanManager;
        _deviceHolder = deviceHolder;
        _locationManager = locationManager;
        _objectFactory = objectFactory;
    }

    public async Task<HikArrivedResponseEntity> Handle(TransferJob agvTask,
        HikArrivedRequestEntity status)
    {
        var response = new HikArrivedResponseEntity();

        _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},Start Excute Handle==========================\r\n");

        var deviceValidator = _objectFactory.GetOrCreate<DeviceValidator>();
        var locationValidator = _objectFactory.GetOrCreate<LocationValidator>();

        var startDeviceValidationResult = deviceValidator.Validate(agvTask.StartDeviceId);
        if (startDeviceValidationResult.code == ERR_CODE)
        {
            return startDeviceValidationResult;
        }

        var endDeviceValidationResult = deviceValidator.Validate(agvTask.EndDeviceId);
        if (endDeviceValidationResult.code == ERR_CODE)
        {
            return endDeviceValidationResult;
        }

        var startLocationValidationResult = locationValidator.Validate(agvTask.StartLocationCode);
        if (startLocationValidationResult.code == ERR_CODE)
        {
            return startLocationValidationResult;
        }

        var endLocationValidationResult = locationValidator.Validate(agvTask.EndLocationCode);
        if (endLocationValidationResult.code == ERR_CODE)
        {
            return endLocationValidationResult;
        }

        _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},The verification of device and locations has been passed.\r\n");

        var startDevice = _deviceHolder.GetOnlineDevice(agvTask.StartDeviceId!);
        var endDevice = _deviceHolder.GetOnlineDevice(agvTask.EndDeviceId!);
        var startLocation = _locationManager.GetLocation(agvTask.StartLocationCode!);
        var endLocation = _locationManager.GetLocation(agvTask.EndLocationCode!);

        switch (status.method.ToLower())
        {
            case "outbin": // 走出储位
                await HandleAgvTaskOutBin(agvTask, status, startDevice!, startLocation!);
                break;

            case "end": // 任务结束
                await HandleAgvTaskEnd(agvTask, status, endDevice!, endLocation!);
                break;
        }

        _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},End Excute Handle==========================\r\n");

        return response;
    }

    private async Task HandleAgvTaskOutBin(TransferJob agvTask, HikArrivedRequestEntity status, DeviceProxy startDevice, Location startLocation)
    {
        _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,开始-执行第一段任务,料仓任务:{agvTask.Id}.\r\n");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"海康已调度潜伏AGV:【{status.robotCode}】进行料仓转运···"
        });

        //叉齿（中转区）->下PIN。此时已离开叉齿，第一段任务结束
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

            _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},Start Device Invoke Service.\r\n");

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
                Message = $"叉齿（中转区）->上PIN/下PIN，离开中转位,下发完成信息"
            }, null);

            _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},Start Device Set Schedule Finished.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = startSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status.robotCode}"
            });

            _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},Start Device Add Schedule Log.\r\n");
        }

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"车辆已到达，离开叉齿，结束-执行第一段任务."
        });

        _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,结束-执行第一段任务,料仓任务:{agvTask.Id}.\r\n");
    }

    private async Task HandleAgvTaskEnd(TransferJob agvTask, HikArrivedRequestEntity status, DeviceProxy endDevice, Location endLocation)
    {
        _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,开始-执行第二段任务,料仓任务:{agvTask.Id}.\r\n");

        //叉齿（中转区）->下PIN。此时已离开下PIN，第二段任务结束
        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && endSchedule.Code != null
            && endSchedule.EventRequest != null)
        {
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

            _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},End Device Set Schedule Finished.\r\n");

            await Task.Delay(500);

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

                _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},End Device Invoke Service.\r\n");
            }

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = endSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status.robotCode}"
            });

            _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},End Device Add Schedule Log Request.\r\n");

            agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
            agvTask.CompletedTime = DateTime.Now;
            await _transferPlanManager.TryUpdateTransferJob(agvTask);

            _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,task:{agvTask.Id},End Device Update Database agvTask.\r\n");
        }

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"车辆已到达，离开下PIN机，结束-执行第二段任务."
        });

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"料仓转运任务结束."
        });

        _logger.LogInformation($"HikAgvHandler - DrilledFromForkToUnPinHandler,结束-执行第二段任务,料仓任务:{agvTask.Id}.\r\n");
    }
}
