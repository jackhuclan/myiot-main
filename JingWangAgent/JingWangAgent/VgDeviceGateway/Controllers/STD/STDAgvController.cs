using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;
using VgDeviceGateway.Devices.Agv;

namespace VgDeviceGateway.Controllers.STD;

[ApiController]
[Route("agv/std")]
public class STDAgvController : ControllerBase
{
    private readonly IDeviceProvider _deviceProvider;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<STDAgvController> _logger;
    private static long _mockTaskId = 0;

    public STDAgvController(IDeviceProvider deviceProvider, ILogger<STDAgvController> logger, IMemoryCache memoryCache)
    {
        _deviceProvider = deviceProvider;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    [HttpGet("hello")]
    public string hello()
    {
        return "hello";
    }

    [HttpPost("arrived", Name = "Arrived")]
    public StdArrivedResponseEntity Arrived(StdArrivedRequestEntity request)
    {
        _logger.LogDebug($"AGV_Arrived：{JsonSerializer.Serialize(request)}");

        try
        {
            var device = _deviceProvider.GetDevice(request.VehicleName);
            if (device is Vehicle)
            {
                _logger.LogDebug($"AGV Arrived：保存缓存成功：{request.OrderId}");
                _memoryCache.Set(request.OrderId.ToStr(), request, TimeSpan.FromDays(1));//一天后过期
                                                                                         //agv.MoveActionStatus = AgvMoveActionStatus.ToDo;
                return new StdArrivedResponseEntity(0);
            }
            else
            {
                return new StdArrivedResponseEntity(201, $"AGV Arrived：维嘉AGV配置异常，该设备不是AGV:{request.VehicleName}");
            }
        }
        catch (Exception ex)
        {
            return new StdArrivedResponseEntity(201, $"{request.VehicleName} 异常: {ex.Message}");
        }
    }

    /// <summary>
    /// AGV进行下一步动作需要请示代理
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("askLeave")]
    public StdCanLeaveResponseEntity AskLeave(StdCanLeaveRequestEntity request)
    {
        _logger.LogDebug($"AskLeave_Info：{JsonSerializer.Serialize(request)}");
        DefaultAgv? agv;

        var agvId = request.VehicleName.Trim();

        try
        {
            var device = _deviceProvider.GetDevice(agvId);
            if (device is DefaultAgv)
            {
                agv = (DefaultAgv)device;
                _logger.LogInformation($"AskLeave,MoveActionStatus:{agv.MoveActionStatus},AgvScanResult:{agv.AgvScanResult},AgvCanLeave:{agv.AgvCanLeave}请求参数:{JsonSerializer.Serialize(request)}");

                if (agv.MoveActionStatus == AgvMoveActionStatus.Doing || !agv.AgvScanResult || agv.AgvCanLeave == false)
                {
                    return new StdCanLeaveResponseEntity(0, false);
                }

                agv.AgvCanLeaveCallBack = true;
                _logger.LogInformation($"{agvId}: AskLeave:AgvCanLeaveCallBack=True");
                return new StdCanLeaveResponseEntity(0, true);
            }
            else
            {
                return new StdCanLeaveResponseEntity(0, false);
            }
        }
        catch
        {
            return new StdCanLeaveResponseEntity(0, false);
        }
    }

    /// <summary>
    /// get online agv
    /// </summary>
    /// <param name="orderid"></param>
    /// <returns></returns>
    [HttpGet("online", Name = "GetStdAgv")]
    public StdArrivedRequestEntity? GetStdAgv(string orderid)
    {
        StdArrivedRequestEntity value;
        if (!_memoryCache.TryGetValue(orderid, out value))
        {
            //"不存在该缓存或者已过期";
            return null;
        }

        return value;
    }

    [HttpPost("mockMoveTask")]
    public StdMoveResponse MockMoveTask(StdMoveRequest request)
    {
        return new StdMoveResponse
        {
            Code = 0,
            ReqCode = request.ReqCode,
            Succ = true,
            Data = new StdMoveOrder { OrderId = ++_mockTaskId }
        };
    }
}