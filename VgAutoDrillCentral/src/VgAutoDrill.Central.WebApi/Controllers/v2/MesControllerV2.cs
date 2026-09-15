using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup.Res;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.WebApi.Controllers.v2;

[Route("v2/central/mes")]
[ApiController]
public class MesControllerV2
{
    private readonly IScheduleTaskAdapter _scheduleTaskAdapter;
    private readonly IItemAdapter _itemAdapter;
    private readonly IPanelGeneratorAdapter _panelGeneratorAdapter;
    private readonly ILogger<MesControllerV2> _logger;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly ILocationManager _locationManager;
    private readonly ISiloAdapter _siloAdapter;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IDeviceManager _deviceManager;
    private readonly ICutterGroupAdapter _cutterGroupAdapter;
    public MesControllerV2(IScheduleTaskAdapter scheduleTaskAdapter,
                           IItemAdapter itemAdapter,
                           ILogger<MesControllerV2> logger,
                           ICutterGroupAdapter cutterGroupAdapter,
                           IPanelGeneratorAdapter panelGeneratorAdapter,
                           IWorkOrderTaskAdapter workOrderTaskAdapter,
                           ILocationManager locationManager,
                           ISiloAdapter siloAdapter,
                           IDeviceManager deviceManager,
                           ISysConfigManager sysConfigManager)
    {
        _scheduleTaskAdapter = scheduleTaskAdapter;
        _itemAdapter = itemAdapter;
        _panelGeneratorAdapter = panelGeneratorAdapter;
        _logger = logger;
        _cutterGroupAdapter = cutterGroupAdapter;
        _workOrderTaskAdapter = workOrderTaskAdapter;
        _locationManager = locationManager;
        _siloAdapter = siloAdapter;
        _sysConfigManager = sysConfigManager;
        _deviceManager = deviceManager;
    }

    /// <summary>
    /// 查询调度记录的状态
    /// 如果是取消、完成、异常中止时，设备端按对应的业务规则处理；
    /// 如果是其他状态时，保持等待；
    /// 不存在此记录，设备端按取消处理
    /// </summary>
    /// <param name="traceId"></param>
    /// <returns></returns>
    [HttpGet("QuerySchedule")]
    public async Task<QueryScheduleResponse> QuerySchedule(string traceId)
    {
        var schedule = await _scheduleTaskAdapter.FindScheduleByTraceId(traceId);
        if (schedule != null)
        {
            return new QueryScheduleResponse
            {
                Code = schedule.Code,
                ScheduledTaskStatus = schedule.ScheduledTaskStatus,
                StatusName = schedule.ScheduledTaskStatus.ToString(),
                TaskId = schedule.TaskId,
                ItemCode = schedule.ItemCode,
                RequireDeviceId = schedule.AllocatedAgv
            };
        }
        else
        {
            return new QueryScheduleResponse
            {
                ScheduledTaskStatus = ScheduledTaskStatus.Canceled,
                StatusName = ScheduledTaskStatus.Canceled.ToString(),
            };
        }
    }

    /// <summary>
    /// 获取物料代码
    /// </summary>
    /// <param name="request.QueryOrderBy">
    /// Original = 0,
    /// OrderByCreateTimeASC = 1,
    /// OrderByCreateTimeDesc = 2,
    /// OrderByCodeASC = 3,
    /// OrderByCodeDesc = 4
    /// </param>
    /// <returns></returns>
    [HttpPost("QueryItemCode")]
    public async Task<List<QueryItemCodeResponse>> QueryItemCode(QueryItemCodeRequest request)
    {
        return await _itemAdapter.GetProductCodes(request);
    }

    [HttpGet("QuerySiloCode")]
    public async Task<List<string>> QuerySiloCode()
    {
        var panelList = await _siloAdapter.GetSilos();
        return panelList.Select(t => t.Code!).OrderBy(x => x).ToList();
    }
    [HttpPost("QuerySilos")]
    public async Task<List<string>> QuerySilos(QuerySiloCodeRequest request)
    {
        var panelList = await _siloAdapter.GetSilos();
        if (!string.IsNullOrEmpty(request.Code))
        {
            return panelList
                .Where(x => x.Code.ToLower().Contains(request.Code.ToLower()))
                .Select(t => t.Code!)
                .OrderBy(x => x)
                .ToList(); ;
        }

        return panelList.Select(t => t.Code!).OrderBy(x => x).ToList();
    }
    [HttpGet("GetLocations")]
    public List<QueryLocationResponse> GetLocations(string deviceId)
    {
        var locationList = _locationManager.GetLocations(deviceId);

        _logger.LogInformation($"MesControllerV2:库位信息{JsonSerializer.Serialize(locationList)}");

        return _locationManager.GetLocations(deviceId)
            .Select(x => new QueryLocationResponse
            {
                Code = x.Code,
                PositionCode = x.PositionCode,
                DeviceId = x.DeviceId,
                SiloCode = x.SiloCode,
                FeedAGVInnerPoint = x.FeedAGVInnerPoint,
                FeedAGVOutputPoint = x.FeedAGVOutputPoint,
                FeedAGVRestPoint = x.FeedAGVRestPoint,
                TransAGVInnerPoint = x.TransAGVInnerPoint,
                TransAGVOutputPoint = x.TransAGVOutputPoint,
                TransAGVRestPoint = x.TransAGVRestPoint
            })
            .OrderBy(x => x.Code)
            .ToList();
    }

    /// <summary>
    /// 生成板料编号
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("GetNextPanelNumber")]
    public async Task<List<Panel>> GetNextPanelNumber(GetNextPanelRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        _logger.LogDebug($"GetNextPanelRequest request:{JsonSerializer.Serialize(request)}");

        if (request.PanelLength <= 0 || request.PanelWidth <= 0)
        {
            var item = await _itemAdapter.GetItemDataAsync(request.ItemCode);
            if (item != null)
            {
                if (item.PanelLength > 0 && request.PanelLength <= 0)
                {
                    request.PanelLength = item.PanelLength;
                }

                if (item.PanelWidth > 0 && request.PanelWidth <= 0)
                {
                    request.PanelWidth = item.PanelWidth;
                }
            }
        }

        return await _panelGeneratorAdapter.GetNextPanelNumber(request);
    }

    /// <summary>
    /// 钻机生成板料编号
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("GetDrillNextPanelNumber")]
    public async Task<List<Panel>> GetDrillNextPanelNumber(GetDrillNextPanelRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        _logger.LogDebug($"GetDrillNextPanelRequest request:{JsonSerializer.Serialize(request)}");

        var item = await _itemAdapter.GetItemDataAsync(request.ItemCode);
        if (item != null)
        {
            if (item.PanelLength > 0)
            {
                request.PanelLength = item.PanelLength;
            }

            if (item.PanelCount > 0 && request.Pcs == 0)
            {
                request.Pcs = item.PanelCount.ToInt();
            }
        }

        return await _panelGeneratorAdapter.GetDrillNextPanelNumber(request);
    }

    /// <summary>
    /// 根据设备ID和物料号获取任务信息
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    [HttpGet("GetTaskInfo")]
    public async Task<Dictionary<string, object?>> GetTaskInfo(string deviceId, string itemCode)
    {
        return await _workOrderTaskAdapter.GetTaskInfo(deviceId, itemCode);
    }

    [HttpGet("GetWorkOrderInfo")]
    public async Task<Dictionary<string, object?>> GetWorkOrderInfo(string itemCode)
    {
        return await _workOrderTaskAdapter.GetWorkOrderInfo(itemCode);
    }

    /// <summary>
    /// 自动上料钻机，获取钻带文件路径、配刀组计划ID
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="beforeDrillPath"></param>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    [HttpGet("GetDrillPath")]
    public async Task<Dictionary<string, object?>> GetDrillPath(string deviceId, string beforeDrillPath, string itemCode)
    {
        if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(beforeDrillPath) || string.IsNullOrEmpty(itemCode))
        {
            _logger.LogInformation($"GetDrillPath return empty! Reason deviceId {deviceId}, beforeDrillPath {beforeDrillPath}, itemCode {itemCode} ");
            return new Dictionary<string, object?> { };
        }
        _logger.LogInformation($"GetDrillPath deviceId {deviceId}, beforeDrillPath {beforeDrillPath}, itemCode {itemCode} ");

        var machineSize = string.Empty;

        if (_deviceManager.TryGetOnlineDevice<Drill>(deviceId, out var drill)
            && drill != null)
        {
            machineSize = drill.MachineSize;
        }

        return await _workOrderTaskAdapter.GetDrillPath(deviceId, beforeDrillPath, itemCode, machineSize);
    }

    /// <summary>
    /// 手动上料钻机，获取钻带文件路径、配刀组计划ID
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="beforeDrillPath"></param>
    /// <returns></returns>

    [HttpGet("GetDrillPathFromManulDrill")]
    public async Task<Dictionary<string, object?>> GetDrillPathFromManulDrill(string deviceId, string beforeDrillPath)
    {
        if (string.IsNullOrEmpty(deviceId) || string.IsNullOrEmpty(beforeDrillPath))
        {
            _logger.LogInformation($"GetDrillPathFromManulDrill return empty! Reason deviceId {deviceId}, beforeDrillPath {beforeDrillPath}");
            return new Dictionary<string, object?> { };
        }
        _logger.LogInformation($"GetDrillPathFromManulDrill deviceId {deviceId}, beforeDrillPath {beforeDrillPath} ");

        var machineSize = string.Empty;

        if (_deviceManager.TryGetOnlineDevice<Drill>(deviceId, out var drill)
            && drill != null)
        {
            machineSize = drill.MachineSize;
        }

        return await _workOrderTaskAdapter.GetDrillPath(deviceId, beforeDrillPath, string.Empty, machineSize);
    }

    /// <summary>
    /// 加载料仓板料信息
    /// 说明：各个代理调用此方法，需要额外传入LocationCode、DeviceId
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("LoadSiloPanelsInfo")]
    public async Task<LoadSiloPanelResponse> LoadSiloPanelsInfo(LoadSiloPanelRequest request)
    {
        ThrowHelper.ThrowArgumentNullException(request);
        _logger.LogDebug($"GetDrillNextPanelRequest request:{JsonSerializer.Serialize(request)}");

        var siloUsedData = _locationManager.Locations.Where(x => x.SiloCode == request.SiloCode
            && (x.DeviceId != request.DeviceId
            || (x.DeviceId == request.DeviceId && x.Code != request.LocationCode))).ToList();
        if (siloUsedData.Count >= 1)
        {
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.IS_CHECK_SILO_USED))
            {
                return new LoadSiloPanelResponse
                {
                    Code = ErrorCodes.Sys.FAIL,
                    Message = $"保存失败！料仓编号：{request.SiloCode},已被{string.Join(",", siloUsedData.Select(x => x.Code))}设备使用。"
                };
            }
        }

        var requests = new GetNextPanelRequest
        {
            BatchCode = request.BatchCode,
            BeginLayer = request.BeginLayer,
            Count = request.Count,
            ItemCode = request.ItemCode,
            LotId = request.LotId,
            PanelWidth = request.PanelWidth,
            PinOffset = request.PinOffset,
            Position = request.Position,
            ProductStatus = request.ProductStatus,
            SiloCode = request.SiloCode,
            LocationCode = request.LocationCode,
        };

        return await _panelGeneratorAdapter.LoadSiloPanelsInfo(requests);
    }

    [HttpPost("ProduceItemData")]
    public async Task<bool> ProduceItemData(Item item)
    {
        return await _itemAdapter.ProduceItemData(item);
    }

    /// <summary>
    /// 更新配刀组状态
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("UpdateCutterGroupStatus")]
    public async Task<ResponseDto<bool>> UpdateCutterGroupStatus(UpdateCutterGroupStatusReq req)
    {
        _logger.LogDebug($"UpdateCutterGroupStatus request:{JsonSerializer.Serialize(req)}");
        return await _cutterGroupAdapter.UpdateGroupStatus(req);
    }

    /// <summary>
    /// 获取配刀文件
    /// </summary>
    /// <param name="deviceNo"></param>
    /// <returns></returns>
    [HttpGet("GetAtpFile")]
    public async Task<ResponseDto<CutterGroupAptFileRes>> GetAtpFile(string deviceNo)
    {
        return await _cutterGroupAdapter.GetAtpFile(deviceNo);
    }
}
