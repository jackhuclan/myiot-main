using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.Kinwong;
using VegaIot.External.AgvEntity.STD;
using VegaIot.External.StdAgv.Validator;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.StdAgv.Handler;

/// <summary>
/// 熟料从内部中转位运转外部熟料区
/// </summary>
public class DrilledFromForkToOutSideUnPinHandler
{
    private readonly ILogger<DrilledFromForkToOutSideUnPinHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationManager _locationManager;
    private readonly IObjectFactory _objectFactory;

    public DrilledFromForkToOutSideUnPinHandler(IServiceProvider serviceProvider)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<DrilledFromForkToOutSideUnPinHandler>();

        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _workOrderTaskAdapter = serviceProvider.GetRequiredService<IWorkOrderTaskAdapter>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _deviceHolder = serviceProvider.GetRequiredService<IDeviceManager>();
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
        _objectFactory = serviceProvider.GetRequiredService<IObjectFactory>();
    }

    public async Task<StdArrivedResponseEntityV2> Handle(TransferJob agvTask, String methodName, String vehicleName)
    {
        var response = new StdArrivedResponseEntityV2();

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Start Excute Handle==========================\r\n");

        var deviceValidator = _objectFactory.GetOrCreate<DeviceValidator>();
        var deviceValidationResult = deviceValidator.Validate(agvTask.StartDeviceId);
        if (deviceValidationResult.Code == ERR_CODE)
        {
            return deviceValidationResult;
        }

        var locationValidator = _objectFactory.GetOrCreate<LocationValidator>();
        var locationValidationResult = locationValidator.Validate(agvTask.StartLocationCode);
        if (locationValidationResult.Code == ERR_CODE)
        {
            return locationValidationResult;
        }

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},The verification of device and locations has been passed.\r\n");

        var startDevice = _deviceHolder.GetOnlineDevice(agvTask.StartDeviceId!);
        var startLocation = _locationManager.GetLocation(agvTask.StartLocationCode!);

        var podLotNum = agvTask.ClinkerCount ?? 0;
        var specGroup = await _workOrderTaskAdapter.GetWorkOrderInfo(agvTask.InternalLotNo!);
        if (_stdAgvConfig.IsCustomPanelCount() && specGroup.ContainsKey("PanelCount"))
        {
            podLotNum *= (int)specGroup["PanelCount"]!;
        }

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Calculation result,podLotNum:{podLotNum},specGroup:{specGroup["PanelCount"]}\r\n");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"AGV:{vehicleName},已离开中转位。物料信息:料号，{agvTask.InternalLotNo};总片数，{podLotNum}"
        });

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
                    { "ShelfIndex",startLocation.Index},
                    { "AgvKind","Std"}
                }
            });

            _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Device Invoke Service.\r\n");


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
                Message = $"AGV:{vehicleName},已离开中转位。物料信息:料号，{agvTask.InternalLotNo};片数，{podLotNum}"
            }, null);

            _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Set Schedule Finished.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = startSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{vehicleName}"
            });

            _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Add Schedule Log Request.\r\n");

            //更新任务表信息
            agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
            agvTask.CompletedTime = DateTime.Now;
            await _transferPlanManager.TryUpdateTransferJob(agvTask);

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = agvTask.Id,
                Message = $"拉熟料到线边仓任务完成！"
            });

            _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Update Database agvTask.\r\n");
        }

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},End Excute Handle==========================\r\n");

        return response;
    }
    public async Task<StdArrivedResponseEntityV2> Handle(TransferJob agvTask,
        StdArrivedRequestEntityV2 status)
    {
        var response = new StdArrivedResponseEntityV2();

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Start Excute Handle==========================\r\n");

        var deviceValidator = _objectFactory.GetOrCreate<DeviceValidator>();
        var deviceValidationResult = deviceValidator.Validate(agvTask.StartDeviceId);
        if (deviceValidationResult.Code == ERR_CODE)
        {
            return deviceValidationResult;
        }

        var locationValidator = _objectFactory.GetOrCreate<LocationValidator>();
        var locationValidationResult = locationValidator.Validate(agvTask.StartLocationCode);
        if (locationValidationResult.Code == ERR_CODE)
        {
            return locationValidationResult;
        }

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},The verification of device and locations has been passed.\r\n");

        var startDevice = _deviceHolder.GetOnlineDevice(agvTask.StartDeviceId!);
        var startLocation = _locationManager.GetLocation(agvTask.StartLocationCode!);

        var podLotNum = agvTask.ClinkerCount ?? 0;
        var specGroup = await _workOrderTaskAdapter.GetWorkOrderInfo(agvTask.InternalLotNo!);
        if (_stdAgvConfig.IsCustomPanelCount() && specGroup.ContainsKey("PanelCount"))
        {
            podLotNum *= (int)specGroup["PanelCount"]!;
        }

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Calculation result,podLotNum:{podLotNum},specGroup:{specGroup["PanelCount"]}\r\n");

        var request = new MaterialDistributionNotifyRequest
        {
            ContainerName = agvTask.InternalLotNo!,
            SonContainerName = agvTask.ExternalLotNo,
            SpecGroup = specGroup["SpecGroup"]?.ToString() ?? "DR1",
            EquipmentCode = agvTask.SiloCode!,
            TaskType = GetOperationType(agvTask.InteractionSequence, agvTask.TransportationKind).ToString(),
            Qty = podLotNum
        };

        var notifyEapUrl = await _stdAgvConfig.GetMaterialDistributionNotifyUrl();

        // 向EAP发送通知
        // TODO: 考虑到EAP接口连接失败情况，可能需要相应地在定时监控服务下进行连接重试
        var kwRes = await _httpRequestInvoker.PostAsJsonAsync<MaterialDistributionNotifyRequest, MaterialDistributionNotifyResponse>(notifyEapUrl, request);

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},下熟料 接口MaterialDistributionNotify,返回的数据为{(kwRes != null ? JsonSerializer.Serialize(kwRes) : string.Empty)}"
        });

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},下熟料 接口MaterialDistributionNotify,发送结果:{kwRes?.Code},返回信息:{kwRes?.Message}.\r\n");

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
                    { "ShelfIndex",startLocation.Index}
                }
            });

            _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Device Invoke Service.\r\n");

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
                Message = $"AGV:{status.robotCode},已离开中转位。物料信息:料号，{agvTask.InternalLotNo};片数，{podLotNum}"
            }, null);

            _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Set Schedule Finished.\r\n");

            await _scheduleTaskManager.AddScheduleLog(new AddScheduleLogRequest
            {
                TraceId = startSchedule.Code,
                Message = $"车辆已到达，任务完成，分配AGV:{status.robotCode}"
            });

            _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Add Schedule Log Request.\r\n");

            //更新任务表信息
            agvTask.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
            agvTask.CompletedTime = DateTime.Now;
            await _transferPlanManager.TryUpdateTransferJob(agvTask);

            _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},Update Database agvTask.\r\n");
        }

        _logger.LogInformation($"StdAgvHandler - DrilledFromForkToOutSideUnPinHandler,task:{agvTask.Id},End Excute Handle==========================\r\n");

        return response;
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    /// <param name="interactionSequence"></param>
    /// <param name="transportationKind"></param>
    /// <returns></returns>
    private int GetOperationType(InteractionSequence? interactionSequence, TransportationKind? transportationKind)
    {
        // TODO: 提取并重用该方法
        return (interactionSequence, transportationKind) switch
        {
            (InteractionSequence.LoadOnly, TransportationKind.EmptySilo) => 5,
            (InteractionSequence.LoadOnly, TransportationKind.Raw) => 4,
            (InteractionSequence.UnloadOnly, TransportationKind.EmptySilo) => 2,
            (InteractionSequence.UnloadOnly, TransportationKind.Clinker) => 1,
            (InteractionSequence.UnloadOnly, TransportationKind.First) => 7,
            _ => 0
        };
    }
}
