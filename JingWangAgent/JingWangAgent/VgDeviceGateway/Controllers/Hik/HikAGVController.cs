using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Dynamic;
using System.Text.Json;
using VegaIot.External.Hik;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Common.Agv;
using VgDeviceGateway.Devices.Common.Agv.Hik;
using static VgDeviceGateway.Devices.Common.Agv.Hik.HikCarStatus;

namespace VgDeviceGateway.Controllers.HiK;

/// <summary>
/// 海康 接口回调
/// </summary>
[ApiController]
[Route("agv/agvCallbackService")]
public class HikAGVController : Controller
{
    private readonly IDeviceProvider _deviceProvider;
    private readonly IObjectFactory _objectFactory;
    private static readonly IOptions<MemoryCacheOptions> optionsAccessor = new MemoryCacheOptions();
    private static readonly IMemoryCache memoryCache = new MemoryCache(optionsAccessor.Value);
    private readonly ILogger<HikAGVController> _logger;
    public readonly HikHandler _hikHandler;
    public HikAGVController(IDeviceProvider deviceProvider,
        IObjectFactory objectFactory,
        ILogger<HikAGVController> logger)
    {
        _deviceProvider = deviceProvider;
        _objectFactory = objectFactory;
        _logger = logger;

        if (deviceProvider.Devices.Any()
            && deviceProvider.Devices[0].DeviceDescriptor.DeviceClazz.Contains("HikTransfer"))
        {
            _hikHandler = _objectFactory.GetOrCreate<HikHandler>(deviceProvider.Devices[0]);
        }
    }

    /// <summary>
    /// 车辆到位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("agvCallback")]
    public HikArrivedResponseEntity agvCallback(HikArrivedRequestEntity status)
    {
        _logger.LogDebug($"\r\n AGV执行的任务已完成{status.taskCode},{status.robotCode.ToStr()}，{status.currentPositionCode}=======================================\r\n");
        _logger.LogDebug($"AGV回调结果：{JsonSerializer.Serialize(status)}");

        var response = new HikArrivedResponseEntity();
        if (status.robotCode == null)
        {
            response.code = "1";
            response.message = "失败";
            return response;
        }

        var agvId = status.robotCode.ToStr();
        agvId = agvId.Replace("#", "");

        Vehicle device;
        try
        {
            device = _deviceProvider.GetDevice(agvId) as Vehicle;
        }
        catch (Exception ex)
        {
            //不存在此device
            response.code = "1";
            response.message = $"失败:{ex.Message}";
            return response;
        }
        //response.AGV_ID = status.robotCode;
        response.reqCode = status.taskCode;
        if (status.taskCode.Length > 0 && status.method.ToLower().Contains("end"))
        {
            memoryCache.Set(status.taskCode.ToStr(), status, TimeSpan.FromDays(1));//一天后过期
            response.code = "0";
            response.message = "成功";
        }
        else
        {
            response.code = "1";
            response.message = $"失败，taskCode:：{status.taskCode}，method：{status.method} ";
        }
        return response;
    }

    /// <summary>
    /// AGV进行下一步动作需要请示代理
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("battery")]
    public HikBatteryResponseEntity Battery(HikBatteryRequestEntity request)
    {
        var agvId = request.AGV_ID.ToStr();
        agvId = agvId.Replace("#", "");
        var response = new HikBatteryResponseEntity();
        Vehicle device;

        try
        {
            device = _deviceProvider.GetDevice(agvId) as Vehicle;
        }
        catch (Exception ex)
        {
            return response;
        }

        response.AGV_ID = request.AGV_ID;
        response.battery = request.battery;

        if (device != null)
        {
            if (device.MoveActionStatus == AgvMoveActionStatus.Doing || !device.AgvScanResult)
            {
                response.IsIDLE = false;
            }
            else
            {
                response.IsIDLE = true;
            }
        }
        return response;
    }

    /// <summary>
    /// get online agv,查询内部小车的任务缓存
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpGet("online", Name = "GetHikAgv")]
    public HikArrivedRequestEntity GetAgv(string taskId)
    {
        if (string.IsNullOrEmpty(taskId) || taskId == "0")
        {
            return new HikArrivedRequestEntity();
        }
        HikArrivedRequestEntity value;
        if (!memoryCache.TryGetValue(taskId, out value))
        {
            //"不存在该缓存或者已过期";
            _logger.LogDebug($"GetAgv_online未查询到数据：参数：{taskId}");
        }
        return value != null ? value : new HikArrivedRequestEntity();
    }

    /// <summary>
    /// 地上爬的AGV车辆到位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("HikAgvCallback")]
    public HikAgvCallBackRes HikAgvCallback(HikAgvCallBack status)
    {
        _logger.LogInformation($"\r\n AGV执行的任务已完成status.taskCode : {status.taskCode}, 任务状态 : {status.method}，currentPositionCode : {status.currentPositionCode}=======================================\r\n");
        _logger.LogInformation($"AGV回调 HikAgvCallback 结果：{JsonSerializer.Serialize(status)}");

        var response = new HikAgvCallBackRes();
        if (status.data == null || string.IsNullOrEmpty(status.data.ToString()))
        {
            response.resultCode = "1";
            response.resultMsg = "未收到有效数据";
            return response;
        }

        var materialData = JsonSerializer.Deserialize<MaterialData>(status.data.ToString()!);
        if (materialData == null)
        {
            response.resultCode = "1";
            response.resultMsg = "数据格式不对，无法反序列化";
            return response;
        }

        if (status.taskCode.Length > 0 && status.method == "end")
        {
            _ = _hikHandler.CallbackHikAgvArrived(status, materialData);
            response.resultCode = "0";
            response.resultMsg = "成功";
        }
        else
        {
            response.resultCode = "1";
            response.resultMsg = "失败";
        }
        return response;
    }

    /// <summary>
    /// 地上爬的AGV离开储位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("outbin")]
    public HikAgvCallBackRes outbin(HikAgvCallBack status)
    {
        _logger.LogInformation($"\r\n AGV离开库位status.taskCode : {status.taskCode}, 任务状态 : {status.method}，currentPositionCode : {status.currentPositionCode}=======================================\r\n");
        _logger.LogInformation($"AGV回调 outbin 结果：{JsonSerializer.Serialize(status)}");

        var response = new HikAgvCallBackRes();
        if (status.data == null || string.IsNullOrEmpty(status.data.ToString()))
        {
            response.resultCode = "1";
            response.resultMsg = "未收到有效数据";
            return response;
        }

        var materialData = JsonSerializer.Deserialize<MaterialData>(status.data.ToString()!);
        if (materialData == null)
        {
            response.resultCode = "1";
            response.resultMsg = "数据格式不对，无法反序列化";
            return response;
        }

        if (status.taskCode.Length > 0)
        //if (status.taskCode.Length > 0 && status.method == "end")
        {
            _ = _hikHandler.Outbin(status, materialData);
            response.resultCode = "0";
            response.resultMsg = "成功";
        }
        else
        {
            response.resultCode = "1";
            response.resultMsg = "失败";
        }

        return response;
    }

    /// <summary>
    /// get online agv,查询内部小车的任务缓存
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    [HttpGet("GetAgvTask", Name = "GetAgvTask")]
    public HikAgvCallBack GetAgvTask(string taskId)
    {
        if (string.IsNullOrEmpty(taskId) || taskId == "0")
        {
            return new HikAgvCallBack();
        }
        HikAgvCallBack value;
        if (!memoryCache.TryGetValue(taskId, out value))
        {
            //"不存在该缓存或者已过期";
            _logger.LogDebug($"GetAgv_online未查询到数据：参数：{taskId}");
        }
        return value != null ? value : new HikAgvCallBack();
    }
    [HttpGet("GetMockAgv", Name = "GetMockAgv")]
    public CarModel GetMockAgv()
    {
        var carModel = new CarModel();
        carModel.CarType = "";
        carModel.Battery = 99;
        carModel.IP = "192.168.0.0";
        carModel.Name = "MockAgv";
        carModel.CurrentStation = "Mock坐标"; 
        carModel.Status = "0";
        carModel.IseeConfigBattery = "99";
        carModel.CanDispatch =  true ;
        carModel.IsLowBattery = false;
        return carModel;
    }
}