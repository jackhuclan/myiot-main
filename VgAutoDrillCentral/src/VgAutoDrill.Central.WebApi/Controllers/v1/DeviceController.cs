using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominRequest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Reporter;
using VgAutoDrill.Central.Core.Reporter.Event;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.WebApi.Controllers.v1;

/// <summary>
///
/// </summary>
[ApiController]
[Route("v1/central/device")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly IDeviceManager _deviceManager;
    private readonly IDevicePanelService _devicePanelService;
    private readonly IExternalWorkOrderService _externalWorkOrderService;
    private readonly IItemAtpFileService _itemAtpFileService;
    private readonly IDeviceService _deviceService;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IDistributedCache _distributedCache;
    private readonly IDeviceEventReporter _deviceEventHandler;
    private readonly IDeviceStatusReporter _deviceStatusHandler;
    private readonly IDeviceAlarmReporter _alarmReporter;
    private readonly IDevicePanelReporter _devicePanelHandler;
    private readonly IDeviceCutterReporter _deviceCutterHandler;
    private readonly IDevicePropertyReporter _devicePropertyHandler;
    private readonly IDeviceServiceReporter _deviceServiceHandler;
    private readonly IItemAdapter _itemAdapter;
    private readonly ILocationManager _locationManager;

    public DeviceController(IDeviceManager deviceHolder,
        IDevicePanelService devicePanelService,
        IItemAtpFileService itemAtpFileService,
        IDeviceService deviceService,
        IScheduleTaskManager scheduleTaskManager,
        IDistributedCache distributedCache,
        IExternalWorkOrderService externalWorkOrderService,
        IDeviceEventReporter deviceEventHandler,
        IDeviceStatusReporter deviceStatusHandler,
        IDeviceAlarmReporter alarmReporter,
        IDevicePanelReporter devicePanelHandler,
        IDeviceCutterReporter deviceCutterHandler,
        IDevicePropertyReporter devicePropertyHandler,
        IDeviceServiceReporter deviceServiceHandler,
        IItemAdapter itemAdapter,
        ILocationManager locationManager,
        ILoggerFactory loggerFactory)
    {
        _deviceManager = deviceHolder;
        _logger = loggerFactory.CreateLogger<DeviceController>();
        _devicePanelService = devicePanelService;
        _itemAtpFileService = itemAtpFileService;
        _deviceService = deviceService;
        _scheduleTaskManager = scheduleTaskManager;
        _distributedCache = distributedCache;
        _deviceEventHandler = deviceEventHandler;
        _deviceStatusHandler = deviceStatusHandler;
        _alarmReporter = alarmReporter;
        _externalWorkOrderService = externalWorkOrderService;
        _devicePanelHandler = devicePanelHandler;
        _deviceCutterHandler = deviceCutterHandler;
        _devicePropertyHandler = devicePropertyHandler;
        _deviceServiceHandler = deviceServiceHandler;
        _itemAdapter = itemAdapter;
        _locationManager = locationManager;
    }

    /// <summary>
    /// 根据itemCode返回钻带文件参数地址
    /// </summary>
    /// <param name="itemCode">料号</param>
    /// <returns></returns>
    [HttpGet("drill/file", Name = "DrillFilePath")]
    public async Task<string?> DrillFilePath(string itemCode)
    {
        if (string.IsNullOrWhiteSpace(itemCode)) return string.Empty;
        var item = await _itemAdapter.GetItemDataAsync(itemCode);
        return item.DrillFilePath;
    }

    [HttpGet]
    [Route("GetDrillRecipes")]
    public async Task<Dictionary<string, object?>> GetDrillRecipes(string deviceId, string lot)
    {
        var result = new Dictionary<string, object?>();
        var req = new GetDrillRecipesReq()
        {
            DeviceId = deviceId,
            Lot = lot
        };
        var data = await _externalWorkOrderService.GetDrillRecipes(req);
        if (data != null && data?.Data != null)
        {
            result = new Dictionary<string, object?>()
            {
                { "Lot",data.Data.Lot },
                { "DeviceId",data.Data.DeviceId },
                { "ProcessCode",data.Data.ProcessCode },
                { "DiaFilePath",data.Data.DiaFilePath },
                { "ProgramFilePath",data.Data.ProgramFilePath },
                { "FtpHost",data.Data.FtpHost },
                { "FtpPort",data.Data.FtpPort },
                { "FtpUsername",data.Data.FtpUsername },
                { "FtpPassword",data.Data.FtpPassword },
                { "Success",data.Data.Success },
                { "Content",data.Data.Content }
            };
        }
        _logger.LogInformation($"自动加载钻带文件，请求参数：{JsonSerializer.Serialize(req)},返回结果：{JsonSerializer.Serialize(result)}");
        return result;
    }

    /// <summary>
    /// 读取设备参数,
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("config", Name = "DeviceConfig")]
    public async Task<DeviceConfigResponse> DeviceConfig(DeviceConfigRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);

        switch (request.ConfigId)
        {
            case Configs.Basic.PAYLOAD_PANELS:
                return await LoadPanelsFromDb(request);

            case Configs.Basic.DEVICE_DESCRIPTOR:
                return await LoadDeviceDescriptor(request);

            case Configs.Drill.DRL_PATH:
            case Configs.Drill.DIA_PATH:
            case Configs.Drill.ATP_PATH:
                return await GetFilePath(request);

            default:
                _logger.LogError($"{ErrorCodes.Sys.WRONG_CONFIG_ID_MESSAGE}：{request.ConfigId}");
                return new DeviceConfigResponse
                {
                    Code = ErrorCodes.Sys.WRONG_CONFIG_ID,
                    Message = $"{ErrorCodes.Sys.WRONG_CONFIG_ID_MESSAGE}：{request.ConfigId}",
                };
        }
    }

    [HttpPost("properties/report", Name = "DevicePropertiesReport")]
    public async Task<DevicePropertiesReportResponse> DevicePropertiesReport(DevicePropertiesReportRequest request)
    {
        return await _devicePropertyHandler.Report(request);
    }

    /// <summary>
    /// 读取设备的属性
    /// todo,检测钻机是否可用时， 改为调用钻机的DevicePropertiesRead的服务方法，获取钻机的属性值某个属性，如：IsAvailable，由AGV和钻机
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("properties/read", Name = "DevicePropertiesRead")]
    public async Task<DeviceServiceInvokeResponse> DevicePropertiesRead(DeviceServiceInvokeRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        DeviceServiceInvokeResponse response;
        if (request.ServiceId != Topics.Services.PROPERTIES_READ_SERVICE_ID)
        {
            response = new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.WRONG_SERVICE_CODE,
                Message = $"{ErrorCodes.Sys.WRONG_SERVICE_MESSAGE}-{request.ServiceId}"
            };
        }
        else
        {
            var device = _deviceManager.GetOnlineDevice(request.DeviceId);
            if (device == null)
            {
                response = new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                    Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
                };
            }
            else if (device.Status == DeviceStatus.Offline)
            {
                response = new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.OFFLINE_CODE,
                    Message = ErrorCodes.Sys.OFFLINE_MESSAGE
                };
            }
            else
            {
                request.ClientId = device.ClientId;
                SetTargetToRequest(request, device);

                response = await device.InvokeService(request);
            }
        }

        return response;
    }

    /// <summary>
    /// 写入设备的属性
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("properties/write", Name = "DevicePropertiesWrite")]
    public async Task<DeviceServiceInvokeResponse> DevicePropertiesWrite(DeviceServiceInvokeRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ServiceId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.ProductId);
        ThrowHelper.ThrowArgumentNullOrWhiteSpaceException(request.DeviceId);

        var response = new DeviceServiceInvokeResponse();

        if (request.ServiceId != Topics.Services.PROPERTIES_WRITE_SERVICE_ID)
        {
            response = new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.WRONG_SERVICE_CODE,
                Message = $"{ErrorCodes.Sys.WRONG_SERVICE_MESSAGE}-{request.ServiceId}"
            };
        }
        else
        {
            var device = _deviceManager.GetOnlineDevice(request.DeviceId);

            if (device == null)
            {
                response = new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                    Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
                };
            }
            else if (device.Status == DeviceStatus.Offline)
            {
                response = new DeviceServiceInvokeResponse
                {
                    Code = ErrorCodes.Sys.OFFLINE_CODE,
                    Message = ErrorCodes.Sys.OFFLINE_MESSAGE
                };
            }
            else
            {
                SetTargetToRequest(request, device);

                response = await device.InvokeService(request);
            }
        }

        return response;
    }

    /// <summary>
    /// 执行业务准备动作
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("service/prepare", Name = "DeviceServicePrepare")]
    public async Task<DeviceServiceInvokeResponse> DeviceServicePrepare(DeviceServiceInvokeRequest request)
    {
        return await _deviceServiceHandler.Report(request);
    }

    /// <summary>
    /// 执行业务操作
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("service/invoke", Name = "DeviceServiceInvoke")]
    public async Task<DeviceServiceInvokeResponse> DeviceServiceInvoke(DeviceServiceInvokeRequest request)
    {
        return await _deviceServiceHandler.Report(request);
    }

    /// <summary>
    /// 执行业务完成后操作
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("service/complete", Name = "DeviceServiceComplete")]
    public async Task<DeviceServiceInvokeResponse> DeviceServiceComplete(DeviceServiceInvokeRequest request)
    {
        return await _deviceServiceHandler.Report(request);
    }

    /// <summary>
    /// 设备事件报告
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("event/report", Name = "DeviceEventReport")]
    public async Task<DeviceEventReportResponse> DeviceEventReport(DeviceEventReportRequest request)
    {
        return await _deviceEventHandler.Report(request);
    }

    /// <summary>
    /// 设备状态报告
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("status/report", Name = "DeviceStatusReport")]
    public async Task<DeviceStatusReportResponse> DeviceStatusReport(DeviceStatusReportRequest request)
    {
        return await _deviceStatusHandler.Report(request);
    }

    /// <summary>
    /// 设备告警报告
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("alarm/report", Name = "DeviceAlarmReport")]
    public async Task<DeviceAlarmReportResponse> DeviceAlarmReport(DeviceAlarmReportRequest request)
    {
        return await _alarmReporter.Report(request);
    }

    /// <summary>
    /// 设备状态报告
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("panel/report", Name = "DevicePanelReport")]
    public async Task<DevicePanelChangedResponse> DevicePanelReport(DevicePanelChangedRequest request)
    {
        return await _devicePanelHandler.Report(request);
    }

    /// <summary>
    /// 设备状态报告
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("cutter/report", Name = "DeviceCutterReport")]
    public async Task<DeviceCutterTrayChangedResponse> DeviceCutterReport(DeviceCutterTrayChangedRequest request)
    {
        return await _deviceCutterHandler.Report(request);
    }

    /// <summary>
    /// 获取在线设备
    /// </summary>
    /// <returns></returns>
    [HttpGet("online", Name = "GetOnlineDevice")]
    [ResponseCache(Duration = 10)]
    public List<DeviceProxy> GetOnlineDevice(string? deviceId)
    {
        var device = _deviceManager.GetOnlineDevice(deviceId);
        if (device == null)
        {
            return new List<DeviceProxy>();
        }
        else
        {
            return new List<DeviceProxy> { device };
        }
    }

    /// <summary>
    /// 获取在线设备列表
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetOnlineDeviceList", Name = "GetOnlineDeviceList")]
    [ResponseCache(Duration = 10)]
    public object GetOnlineDeviceList()
    {
        var activeStatusList = new List<ScheduledTaskStatus?>
        {
            ScheduledTaskStatus.Created,
            ScheduledTaskStatus.Allocated,
            ScheduledTaskStatus.Running,
            ScheduledTaskStatus.PartCompleted,
        };

        var activeSchedules = _scheduleTaskManager.Tasks.Where(x => activeStatusList.Contains(x.ScheduledTaskStatus));

        return _deviceManager.Devices.Select(x => new
        {
            x.Descriptor.ProductId,
            x.DeviceId,
            x.RouteCodes,
            x.Descriptor,
            x.DeviceStandbyTime,
            x.Properties,
            PayloadPanels = _locationManager.GetDevicePanels(x.DeviceId),
            x.PayloadCutterTrays,
            TargetDevice = DeviceKindExtensions.IsAGV(x.Descriptor.DeviceKind) ? x.Properties["TargetDevice"].ToStr() : string.Empty,
            NotActive = !activeSchedules.Any(s => !string.IsNullOrEmpty(s.CallerDeviceId) && s.CallerDeviceId.ToLower() == x.DeviceId.ToLower()),
            Status = x.Status.ToString(),
            DeviceStatus = x.Status,
        }).ToList();
    }

    /// <summary>
    /// 生成板料编号
    /// </summary>
    /// <param name="count">几个板料编号</param>
    /// <returns></returns>
    [HttpGet("GetNextPanelNumber")]
    public List<string> GetNextPanelNumber(int count = 1)
    {
        var result = new List<string>();

        //上次存取的key
        var dateKey = _distributedCache.GetString("GetPanelCode");
        int nextNumber = 1;
        var numberKey = _distributedCache.GetString("GetPanelNextNumber");
        int.TryParse(numberKey, out nextNumber);

        var todayKey = DateTime.Today.ToString("yyyyMMdd");
        if (dateKey != todayKey)
        {
            nextNumber = 1;
        }
        for (var i = 0; i < count; i++)
        {
            result.Add($"{todayKey}{(i + nextNumber).ToString().PadLeft(5, '0')}");
        }

        nextNumber += count;

        _distributedCache.SetString("GetPanelCode", todayKey);
        _distributedCache.SetString("GetPanelNextNumber", nextNumber.ToString());

        return result;
    }

    [HttpPost("CheckStatus", Name = "CheckStatus")]
    public async Task<DeviceServiceInvokeResponse> CheckStatus(DeviceServiceInvokeRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.TargetDeviceId))
        {
            throw new ArgumentNullException(nameof(request));
        }

        request.ServiceId = Topics.Services.PROPERTIES_READ_SERVICE_ID;

        var deviceProxy = _deviceManager.GetOnlineDevice(request.TargetDeviceId);
        if (deviceProxy == null)
        {
            _logger.LogWarning($"CheckStatus TargetDeviceId is not online:{request.TargetDeviceId}");
            return new DeviceServiceInvokeResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"CheckStatus TargetDeviceId is not online:{request.TargetDeviceId}"
            };
        }

        var response = await deviceProxy.InvokeService(request);

        if (response == null || response.Code != ErrorCodes.Sys.SUCCESS)
        {
            _logger.LogWarning($"CheckStatus TargetDeviceId cannot be connected:{request.TargetDeviceId}");
        }

        _logger.LogInformation($"CheckStatus TargetDeviceId can be connected:{request.TargetDeviceId}!!");
        return response;
    }

    /// <summary>
    /// 设置Target相关属性
    /// </summary>
    /// <param name="request"></param>
    /// <param name="device"></param>
    private static void SetTargetToRequest(DeviceServiceInvokeRequest request, DeviceProxy device)
    {
        request.TargetProductId = device.Descriptor.ProductId;
        request.TargetDeviceId = device.Descriptor.DeviceId;
        request.TargetClientId = device.ClientId;
    }

    /// <summary>
    /// 获取文件路径
    /// TODO, 目前三个文件路径，全部都是从ATP 文件表中获取
    /// 对自动程度没有那么高的设备，可能没有ATP文件，需要拓展为从各自表中单独获取
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<DeviceConfigResponse> GetFilePath(DeviceConfigRequest request)
    {
        var itemCode = request.Params.ContainsKey("ItemCode") ? request.Params["ItemCode"].ToStr() : string.Empty;
        var response = new DeviceConfigResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty,
        };
        if (string.IsNullOrEmpty(itemCode))
        {
            response.Code = ErrorCodes.Sys.FAIL;
            response.Message = "物料代码提供，参数为空：request.Params[\"ItemCode\"]";
        }

        var query = await _itemAtpFileService.GetList(new GetItemAtpFileListReq { ItemCode = itemCode, PageSize = 1 });
        var atpSetting = query.Data.List.FirstOrDefault();
        if (atpSetting != null)
        {
            switch (request.ConfigId)
            {
                case Configs.Drill.DRL_PATH:
                    _logger.LogInformation($"调用成功，返回:{Configs.Drill.DRL_PATH}{atpSetting.ItemDrillFilePath}");
                    response.Data = atpSetting.ItemDrillFilePath;
                    break;

                case Configs.Drill.DIA_PATH:
                    _logger.LogInformation($"调用成功，返回:{Configs.Drill.DIA_PATH}{atpSetting.DiaFilePath}");
                    response.Data = atpSetting.DiaFilePath;
                    break;

                case Configs.Drill.ATP_PATH:
                    _logger.LogInformation($"调用成功，返回:{Configs.Drill.ATP_PATH}{atpSetting.ATPFilePath}");
                    response.Data = atpSetting.ATPFilePath;
                    break;

                default:
                    _logger.LogWarning($"参数错误，request.ConfigId：{request.ConfigId}");
                    response.Code = ErrorCodes.Sys.FAIL;
                    response.Message = $"参数错误，request.ConfigId：{request.ConfigId}";
                    return response;
            }
        }
        else
        {
            _logger.LogWarning($"找不到物料的排刀文件等信息， 物料代码：{itemCode}");
            response.Code = ErrorCodes.Sys.FAIL;
            response.Message = $"找不到物料的排刀文件等信息， 物料代码：{itemCode}";
            return response;
        }

        return response;
    }

    /// <summary>
    /// load PayloadPanels from db
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<DeviceConfigResponse> LoadPanelsFromDb(DeviceConfigRequest request)
    {
        var payloadPanels = new List<Panel>();
        var panels = await _devicePanelService.GetList(new GetDevicePanelListReq { DeviceCode = request.DeviceId, PageSize = int.MaxValue });
        foreach (var panel in panels.Data.List)
        {
            payloadPanels.Add(new Panel
            {
                PanelCode = panel.PanelCode,
                SiloCode = panel.SiloCode,
                ItemCode = panel.ItemCode,
                LotId = panel.LotId,
                BatchCode = panel.BatchCode,
                Layer = panel.Layer,
                Position = panel.Position,
                ProductStatus = panel.ProductStatus,
            });
        }
        _logger.LogInformation($"成功初始化设备PayloadPanels， 数量：{payloadPanels.Count}");

        return new DeviceConfigResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty,
            Data = payloadPanels
        };
    }

    /// <summary>
    /// 从数据库中，加载设备参数
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<DeviceConfigResponse> LoadDeviceDescriptor(DeviceConfigRequest request)
    {
        var response = new DeviceConfigResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty,
        };

        var deviceQuery = await _deviceService.GetEquipmentList(new GetDeviceListReq { Code = request.DeviceId, PageSize = 1 });
        if (deviceQuery != null
            && deviceQuery.Data.List.Count > 0
            && !string.IsNullOrEmpty(deviceQuery.Data.List[0].Parameters))
        {
            var parameter = deviceQuery.Data.List[0].Parameters;

#pragma warning disable CS8604 // 引用类型参数可能为 null。
            var deviceDescriptorExtra = JsonSerializer.Deserialize<DeviceDescriptor>(parameter, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
#pragma warning restore CS8604 // 引用类型参数可能为 null。

            if (deviceDescriptorExtra == null)
            {
                _logger.LogWarning($"设备参数格式有问题，未能初始化设备参数， 设备ID:{request.DeviceId}");
                response.Code = ErrorCodes.Sys.FAIL;
                response.Message = $"设备参数格式有问题，未能初始化设备参数， 设备ID:{request.DeviceId}";
            }
            else
            {
                //_logger.LogInformation($"成功初始化设备参数， 设备ID:{request.DeviceId}");
                response.Data = deviceDescriptorExtra;
            }
        }
        else
        {
            _logger.LogWarning($"找不到设备或者设备参数未定义， 设备ID:{request.DeviceId}");
            response.Code = ErrorCodes.Sys.FAIL;
            response.Message = $"设备参数格式有问题，未能初始化设备参数， 设备ID:{request.DeviceId}";
        }

        return response;
    }
}
