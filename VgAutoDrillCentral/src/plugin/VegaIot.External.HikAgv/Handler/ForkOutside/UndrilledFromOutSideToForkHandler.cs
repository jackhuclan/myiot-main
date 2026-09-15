using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.Hik;
using VegaIot.External.HikAgv.Validator;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;
using static VegaIot.External.Agv.ErrorCodes;
using static VegaIot.External.AgvEntity.Hik.HikArrivedRequestEntity;

namespace VegaIot.External.HikAgv.Handler;

/// <summary>
/// 生料从外部生料区运转到内部中转位
/// </summary>
public class UndrilledFromOutSideToForkHandler
{
    private readonly ILogger<UndrilledFromOutSideToForkHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationManager _locationManager;
    private readonly IObjectFactory _objectFactory;
    private readonly HikAgvConfig _hikAgvConfig;

    public UndrilledFromOutSideToForkHandler(IServiceProvider serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<UndrilledFromOutSideToForkHandler>();

        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _workOrderTaskAdapter = serviceProvider.GetRequiredService<IWorkOrderTaskAdapter>();
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _objectFactory = serviceProvider.GetRequiredService<IObjectFactory>();
        _hikAgvConfig = serviceProvider.GetRequiredService<HikAgvConfig>();
    }

    public async Task<HikArrivedResponseEntity> Handle(TransferJob agvTask,
        HikArrivedRequestEntity status)
    {
        var response = new HikArrivedResponseEntity();

        _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},Start Excute Handle==========================\r\n");

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

        _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},The verification of device and locations has been passed.\r\n");

        var endDevice = _deviceHolder.GetOnlineDevice(agvTask.EndDeviceId!);
        var endLocation = _locationManager.GetLocation(agvTask.EndLocationCode!);

        if (status.data != null && string.IsNullOrEmpty(status.data.ToString()))
        {
            _logger.LogWarning($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},status data is null.");
            return new HikArrivedResponseEntity
            {
                code = ERR_CODE,
                message = MATERIAL_DATA_IS_NULL
            };
        }

        if (agvTask.EndScheduleId.HasValue
            && _scheduleTaskManager.TryGetScheduleTaskById(agvTask.EndScheduleId.Value, out var endSchedule)
            && endSchedule != null
            && endDevice != null
            && endLocation != null
            && endSchedule.Code != null
            && endSchedule.EventRequest != null)
        {
            var retMaterialData = JsonSerializer.Deserialize<StatusMaterialData>(status.data!.ToString()!);

            _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},lot:{retMaterialData?.lot},materialCode:{retMaterialData?.materialCode},qty:{retMaterialData?.qty}.\r\n");

            //获取板料信息
            var panelCount = retMaterialData!.qty!.ToInt();
            var specGroup = await _workOrderTaskAdapter.GetWorkOrderInfo(agvTask.InternalLotNo!);
            if (_hikAgvConfig.IsCustomPanelCount() && specGroup.ContainsKey("PanelCount"))
            {
                panelCount %= (int)specGroup["PanelCount"]!;
            }
            _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},panelCount:{panelCount},specGroup:{specGroup["PanelCount"]}.\r\n");

            var panels = endSchedule.EventRequest.PayloadPanels;
            panels.SetEmpty("", status.podCode!);

            var position = endLocation.Index;
            var itemcode = agvTask.InternalLotNo!;
            var externalLotNo = agvTask.ExternalLotNo ?? "";
            var pcs = _hikAgvConfig.IsCustomPanelCount() ? (int)specGroup["PanelCount"]! : 1;
            var productStatus = ProductStatus.PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_1;

            panels.UpdatePanelInfo(position, panelCount, itemcode, externalLotNo, pcs, productStatus);

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

            _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},Set Schedule Finished.\r\n");

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
                EventId = endSchedule.EventRequest.EventId,
                TargetHostAddress = endDevice.Descriptor.HostAddress,
                Params = new Dictionary<string, object?>
                {
                    { "LoadingPanel", new { PanelList = panels}},
                    { "ShelfIndex",endLocation.Index},
                },
            });

            _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},Device Invoke Service.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = endSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status.robotCode}"
            });

            _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},Add Schedule Log Request.\r\n");

            //更新任务表信息
            agvTask.SiloCode = status.podCode;
            agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
            agvTask.CompletedTime = DateTime.Now;
            await _transferPlanManager.TryUpdateTransferJob(agvTask);

            _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},Update Database agvTask.\r\n");
        }

        _logger.LogInformation($"HikAgvHandler - UndrilledFromOutSideToForkHandler,task:{agvTask.Id},End Excute Handle==========================\r\n");

        return response;
    }
}
