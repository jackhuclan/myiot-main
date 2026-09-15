using Microsoft.Extensions.Logging;
using VegaIot.External.AgvEntity.Hik;
using VegaIot.External.HikAgv.Validator;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.HikAgv.Handler;

/// <summary>
/// 空料仓从线边仓拉到上PIN
/// </summary>
public class EmptyBoxFromWipToPinHandler
{
    private readonly ILogger<EmptyBoxFromWipToPinHandler> _logger;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationManager _locationManager;
    private readonly IObjectFactory _objectFactory;

    public EmptyBoxFromWipToPinHandler(ILogger<EmptyBoxFromWipToPinHandler> logger,
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

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromWipToPinHandler,task:【{agvTask.Id}】,Start Excute Handle==========================\r\n");

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

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromWipToPinHandler,task:【{agvTask.Id}】,The verification of device and locations has been passed.\r\n");

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

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromWipToPinHandler,task:【{agvTask.Id}】,End Excute Handle==========================\r\n");

        return response;
    }

    private async Task HandleAgvTaskOutBin(TransferJob agvTask, HikArrivedRequestEntity status, DeviceProxy startDevice, Location startLocation)
    {
        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromWipToPinHandler,开始-执行第一段任务，料仓任务:【{agvTask.Id}】.\r\n");

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"海康已调度潜伏AGV:【{status.robotCode}】进行料仓转运···"
        });

        var outBinTaskHandle = _objectFactory.GetOrCreate<HandleAgvOutBinTask>();

        string message = "空料仓:线边仓->上PIN，离开线边仓,下发完成信息";
        string operationName = "EmptyBoxFromWipToPinHandler";

        await outBinTaskHandle.Handle(agvTask, status, startDevice, startLocation, message, operationName);

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"车辆已到达，离开【空仓】线边仓，结束-执行第一段任务."
        });

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromWipToPinHandler,结束-执行第一段任务，料仓任务:【{agvTask.Id}】.\r\n");
    }

    private async Task HandleAgvTaskEnd(TransferJob agvTask, HikArrivedRequestEntity status, DeviceProxy endDevice, Location endLocation)
    {
        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromWipToPinHandler,开始-执行第二段任务，料仓任务:【{agvTask.Id}】.\r\n");

        var endTaskHandle = _objectFactory.GetOrCreate<HandleAgvEndTask>();

        string operationName = "EmptyBoxFromWipToPinHandler";

        await endTaskHandle.Handle(agvTask, status, endDevice, endLocation, operationName);

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"车辆已到达，离开上pin机，结束-执行第二段任务."
        });

        await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
        {
            TransferJobId = agvTask.Id,
            Message = $"料仓转运任务结束."
        });

        _logger.LogInformation($"HikAgvHandler - EmptyBoxFromWipToPinHandler,结束-执行第二段任务，料仓任务:【{agvTask.Id}】.\r\n");
    }
}
