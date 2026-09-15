using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using VegaIot.External.AgvEntity.AJW;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Controllers.AJW;

/// <summary>
/// 艾吉威 接口回调
/// </summary>
[ApiController]
[Route("v1/agent/ajw")]
public class AjwAGVController : Controller
{
    private readonly IDeviceProvider _deviceProvider;
    private static readonly IOptions<MemoryCacheOptions> optionsAccessor = new MemoryCacheOptions();
    private static readonly IMemoryCache memoryCache = new MemoryCache(optionsAccessor.Value);
    private readonly ILogger<AjwAGVController> _logger;

    public AjwAGVController(IDeviceProvider deviceProvider, ILoggerFactory loggerFactory)
    {
        _deviceProvider = deviceProvider;
        _logger = loggerFactory.CreateLogger<AjwAGVController>();
    }

    /// <summary>
    /// 车辆到位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("arrived")]
    public AJWArrivedResponseEntity Arrived(AJWArrivedRequestEntity status)
    {
        var response = new AJWArrivedResponseEntity();
        _logger.LogDebug($"艾吉威到位回调:{JsonSerializer.Serialize(status)}");
        if (status.AGV_ID == null)
        {
            _logger.LogDebug($"设备端Arrived:参数没有AGV_ID：{status.AGV_ID}");
            response.Result = false;
            return response;
        }
        var agvId = status.AGV_ID.ToStr();
        agvId = agvId.Replace("#", "");
        response.AGV_ID = status.AGV_ID;
        Vehicle device;
        try
        {
            device = _deviceProvider.GetDevice(agvId) as Vehicle;
            if (device == null)
            {
                _logger.LogDebug($"设备端Arrived:根据ID没找到AGV{agvId}");
                response.Result = false;
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"设备端Arrived:没找到AGV：{ex.Message}");
            response.Result = false;
            return response;
        }
        try
        {
            if (status.IsSuccess)
            {
                _logger.LogDebug($"设备端Arrived:保存缓存成功：{status.TaskId}");
                memoryCache.Set(status.TaskId.ToStr(), status, TimeSpan.FromDays(1));//一天后过期
            }
            else
            {
                _logger.LogDebug($"设备端Arrived返回失败:{status.TaskId}_{JsonSerializer.Serialize(status)}");
                //int failcount = 0;
                //if (!memoryCache.TryGetValue("ScanFailCount" + status.TaskId.ToStr(), out failcount))
                //{
                //    memoryCache.Set("ScanFailCount" + status.TaskId.ToStr(), 1, TimeSpan.FromDays(1));
                //    _logger.LogDebug($"1ScanFailCount:{status.TaskId.ToStr()}:{failcount}");
                //}
                //else
                //{
                //    memoryCache.Set("ScanFailCount" + status.TaskId.ToStr(), failcount + 1, TimeSpan.FromDays(1));
                //    _logger.LogDebug($"2ScanFailCount:{status.TaskId.ToStr()}:{failcount + 1}");
                //}
                //if (device != null)
                //{
                //    if (failcount > 5)
                //    {
                //        _logger.LogDebug($"3ScanFailCount:{status.TaskId.ToStr()}:{failcount + 1}");
                //        //device.AgvScanResult = false;
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"艾吉威Arrived：{ex.Message}");
            response.Result = false;
            return response;
        }
        response.Result = true;
        return response;
    }

    [HttpPost("battery")]
    public AJWBatteryResponseEntity Battery(AJWBatteryRequestEntity request)
    {
        var agvId = request.AGV_ID.ToStr();
        agvId = agvId.Replace("#", "");
        var response = new AJWBatteryResponseEntity();
        Vehicle device;
        response.AGV_ID = request.AGV_ID;
        response.battery = request.battery;
        response.IsIDLE = true;
        try
        {
            device = _deviceProvider.GetDevice(agvId) as Vehicle;
        }
        catch (Exception ex)
        {
            _logger.LogError("Battery接口:" + ex.ToString());
            response.IsIDLE = false;
            return response;
        }

        if (device != null)
        {
            if (device.MoveActionStatus == AgvMoveActionStatus.Doing || !device.AgvScanResult)
            {
                _logger.LogDebug($"Battery:MoveActionStatus:{device.MoveActionStatus}_AgvScanResult5次失败:{device.AgvScanResult}");
                response.IsIDLE = false;
            }
        }

        return response;
    }

    /// <summary>
    /// get online agv
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpGet("online", Name = "GetAgv")]
    public AJWArrivedRequestEntity GetAgv(string taskId)
    {
        if (string.IsNullOrEmpty(taskId) || taskId == "0")
        {
            return new AJWArrivedRequestEntity();
        }
        AJWArrivedRequestEntity value;
        if (!memoryCache.TryGetValue(taskId, out value))
        {
            //"不存在该缓存或者已过期";
        }
        return value != null ? value : new AJWArrivedRequestEntity();
    }
}