using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SqlSugar.Extensions;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.WebApi.Controllers.v1;

/// <summary>
/// StartSchedule() //AGV开始工作
/// UpdateSchedule() 可用于上板过程情况，当中报告第几轴的完成情况
/// FailSchedule() //异常中止
/// CompleteSchedule //调度完成
///</summary>
[ApiController]
[Route("v1/central/schedule")]
public class ScheduleController
{
    private readonly ILogger<ScheduleController> _logger;
    private readonly IDeviceManager _deviceManager;
    private readonly IDistributedCache _distributedCache;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ISiloAdapter _siloAdapter;
    private readonly IDeviceServiceInvocationLogger _deviceServiceInvocationLogger;
    private readonly ILocationManager _locationManager;

    public ScheduleController(IDeviceManager deviceHolder,
        IDistributedCache distributedCache,
        IScheduleTaskManager taskScheduler,
        IScheduleService scheduleService,
        IScheduleLogService scheduleLogService,
        ISiloAdapter siloAdapter,
        IScheduleTaskAdapter scheduleTaskAdapter,
        IDeviceServiceInvocationLogger deviceServiceInvocationLogger,
        ILocationManager locationManager,
        ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ScheduleController>();
        _deviceManager = deviceHolder;
        _distributedCache = distributedCache;
        _scheduleTaskManager = taskScheduler;
        _siloAdapter = siloAdapter;
        _deviceServiceInvocationLogger = deviceServiceInvocationLogger;
        _locationManager = locationManager;
    }

    //[HttpPost("clearRedisKeys", Name = "ClearRedisKeys")]
    //public async Task ClearRedisKeys(string productId, string deviceId)
    //{
    //    var keyWords = $"*{productId}.{deviceId}.event*".ToLower();
    //    if (!string.IsNullOrEmpty(keyWords))
    //    {
    //        await _distributedCache.RemoveAsync(keyWords);
    //    }
    //}

    //[HttpPost("findKeys", Name = "FindKeys")]
    //public List<string> FindKeys(string productId, string deviceId)
    //{
    //    var keyWords = $"{productId}.{deviceId}.event".ToLower();
    //    if (!string.IsNullOrEmpty(keyWords))
    //    {
    //        return _redisClient.FindKeys(keyWords);
    //    }

    //    return new List<string>();
    //}

    [HttpPost("complete", Name = "CompleteSchedule")]
    public async Task<CompleteScheduleTaskResponse> CompleteSchedule(CompleteScheduleTaskRequest completeScheduleTaskRequest)
    {
        if (completeScheduleTaskRequest == null
            || string.IsNullOrWhiteSpace(completeScheduleTaskRequest.TraceId))
        {
            _logger.LogError($"completeScheduleTaskRequest参数错误{JsonSerializer.Serialize(completeScheduleTaskRequest)}");
            throw new ArgumentNullException(nameof(completeScheduleTaskRequest));
        }

        var agv = _deviceManager.GetOnlineDevice(completeScheduleTaskRequest.DeviceId) as Agv;
        if (agv == null)
        {
            return new CompleteScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
            };
        }

        var responseMsg = await _scheduleTaskManager.SetScheduleAsFinished(completeScheduleTaskRequest, agv);
        if (string.IsNullOrEmpty(responseMsg))
        {
            return new CompleteScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = ""
            };
        }
        else
        {
            return new CompleteScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"Complete Schedule Failed. Reason:{responseMsg}"
            };
        }
    }

    /// <summary>
    /// 写入调度明细
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [HttpPost("processReport", Name = "ProcessReportSchedule")]
    public async Task<AddScheduleLogResponse> ProcessReportSchedule(AddScheduleLogRequest request)
    {
        _logger.LogDebug($"ProcessReportSchedule,device:{request.DeviceId},message:{request.Message}");
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return await _scheduleTaskManager.AddScheduleLog(request);
    }

    [HttpPost("fail", Name = "FailSchedule")]
    public async Task<FailScheduleTaskResponse> FailSchedule(FailScheduleTaskRequest failScheduleTaskRequest)
    {
        _logger.LogWarning($"FailSchedule,request:{JsonSerializer.Serialize(failScheduleTaskRequest)}");
        if (failScheduleTaskRequest == null)
        {
            throw new ArgumentNullException(nameof(failScheduleTaskRequest));
        }

        var agv = _deviceManager.GetOnlineDevice(failScheduleTaskRequest.DeviceId) as Agv;
        if (agv == null)
        {
            return new FailScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
            };
        }

        await _scheduleTaskManager.SetScheduleAsException(failScheduleTaskRequest, agv);

        return new FailScheduleTaskResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = ""
        };
    }

    [HttpPost("start", Name = "StartSchedule")]
    public async Task<StartScheduleTaskResponse> StartSchedule(StartScheduleTaskRequest startScheduleTaskRequest)
    {
        _logger.LogWarning($"StartSchedule,request:{JsonSerializer.Serialize(startScheduleTaskRequest)}");

        var agv = _deviceManager.GetOnlineDevice(startScheduleTaskRequest.DeviceId) as Agv;
        if (agv == null)
        {
            return new StartScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
            };
        }

        var response = await _scheduleTaskManager.SetScheduleAsWorking(startScheduleTaskRequest, agv);

        if (string.IsNullOrEmpty(response))
        {
            return new StartScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = ""
            };
        }
        else
        {
            return new StartScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = response
            };
        }
    }

    /// <summary>
    /// 重置redis 叫料锁的key
    /// 并通知钻机，取消调度控制的限定，以便发起下一次的上下料申请
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="deviceId"></param>
    /// <param name="routingKey"></param>
    /// <returns></returns>
    [HttpGet("resetScheduleLocker", Name = "ResetScheduleLocker")]
    public async Task<string> ResetScheduleLocker(string productId, string deviceId, string routingKey)
    {
        _logger.LogWarning($"ResetScheduleLocker,productId:{productId},deviceId:{deviceId}, now started.");

        var deviceProxy = _deviceManager.GetOnlineDevice(deviceId);
        if (deviceProxy == null)
        {
            return ErrorCodes.Sys.WRONG_DEVICE_MESSAGE;
        }

        await _distributedCache.RemoveAsync(routingKey);

        var serviceRequest = new DeviceServiceInvokeRequest
        {
            ProductId = deviceProxy.Descriptor.ProductId,
            DeviceId = deviceProxy.Descriptor.DeviceId,
            ClientId = deviceProxy.ClientId,
            ServiceId = Topics.Services.COMPLETE_SCHEDULE_SERVICE_ID,
            TargetProductId = deviceProxy.Descriptor.ProductId,
            TargetDeviceId = deviceProxy.Descriptor.DeviceId,
            TargetClientId = deviceProxy.ClientId,
            ScheduledStatus = ScheduledTaskStatus.Canceled
        };

        var response = await deviceProxy.InvokeService(serviceRequest);
        if (response.Code == ErrorCodes.Sys.SUCCESS)
        {
            _logger.LogWarning($"ResetScheduleLocker,productId:{productId},deviceId:{deviceId},now succeed.");
            return string.Empty;
        }
        else
        {
            _logger.LogWarning($"ResetScheduleLocker,productId:{productId},deviceId:{deviceId},now failed.");
            var inputRequest = JsonSerializer.Serialize(serviceRequest);
            _ = _deviceServiceInvocationLogger.OnInvocationFailure(new DeviceServiceInvocationArgs
            {
                RequestTopic = serviceRequest.RequestTopic,
                ResponseTopic = serviceRequest.ReplyTopic,
                Payload = inputRequest,
                Reason = response.Code,
                ServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce,
            });
            return $"ResetScheduleLocker,productId:{productId},deviceId:{deviceId},now failed.";
        }
    }

    [HttpGet("resetAgvCallLimit", Name = "ResetAgvCallLimit")]
    public async Task<string> ResetAgvCallLimit(string productId, string deviceId)
    {
        _logger.LogWarning($"ResetAgvCallLimit,productId:{productId},deviceId:{deviceId}, now started.");

        var deviceProxy = _deviceManager.GetOnlineDevice(deviceId);
        if (deviceProxy == null)
        {
            return ErrorCodes.Sys.WRONG_DEVICE_MESSAGE;
        }

        var serviceRequest = new DeviceServiceInvokeRequest
        {
            ProductId = deviceProxy.Descriptor.ProductId,
            DeviceId = deviceProxy.Descriptor.DeviceId,
            ClientId = deviceProxy.ClientId,
            ServiceId = Topics.Services.COMPLETE_SCHEDULE_SERVICE_ID,
            TargetProductId = deviceProxy.Descriptor.ProductId,
            TargetDeviceId = deviceProxy.Descriptor.DeviceId,
            TargetClientId = deviceProxy.ClientId,
            ScheduledStatus = ScheduledTaskStatus.Canceled
        };

        var response = await deviceProxy.InvokeService(serviceRequest);
        if (response.Code == ErrorCodes.Sys.SUCCESS)
        {
            _logger.LogWarning($"ResetAgvCallLimit,productId:{productId},deviceId:{deviceId},now succeed.");
            return string.Empty;
        }
        else
        {
            var inputRequest = JsonSerializer.Serialize(serviceRequest);
            _ = _deviceServiceInvocationLogger.OnInvocationFailure(new DeviceServiceInvocationArgs
            {
                RequestTopic = serviceRequest.RequestTopic,
                ResponseTopic = serviceRequest.ReplyTopic,
                Payload = inputRequest,
                Reason = response.Code,
                ServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce,
            });
            _logger.LogWarning($"ResetAgvCallLimit,productId:{productId},deviceId:{deviceId},now failed.");
            return $"ResetAgvCallLimit,productId:{productId},deviceId:{deviceId},now failed.";
        }
    }

    [HttpGet("setScheduleUrgent", Name = "SetScheduleUrgent")]
    public async Task<string> SetScheduleUrgent(long id)
    {
        return await _scheduleTaskManager.SetScheduleUrgent(id);
    }

    [HttpPost("cancelSchedule", Name = "CancelSchedule")]
    public async Task<CancelScheduleTaskResponse> CancelSchedule(CancelScheduleTaskRequest request)
    {
        if (request == null
            || (string.IsNullOrEmpty(request.TraceId)
                && string.IsNullOrEmpty(request.LocationCode)))
        {
            return new CancelScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = "错误参数",
            };
        }

        var traceId = request.TraceId;
        if (string.IsNullOrEmpty(traceId))
        {
            if (!string.IsNullOrEmpty(request.LocationCode)
                && _scheduleTaskManager.TryGetTodoOrDoingScheduleByLocationCode(request.LocationCode, out var scheduleTask))
            {
                traceId = scheduleTask.Code;
            }
        }

        if (string.IsNullOrEmpty(traceId))
        {
            return new CancelScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = "",
            };
        }

        string cancelReason;
        if (request.Params.ContainsKey("CancelReason") && !string.IsNullOrEmpty(request.Params["CancelReason"].ToStr()))
        {
            cancelReason = request.Params["CancelReason"].ToStr();
        }
        else
        {
            cancelReason = "未提供取消原因，接口呼叫";
        }

        var isForced = false;
        if (request.Params.ContainsKey("IsForced") && request.Params["IsForced"].ToBool())
        {
            isForced = true;
        }

        var responseMsg = await _scheduleTaskManager.CancelSingleSchedule(traceId, isForced, cancelReason);
        if (string.IsNullOrEmpty(responseMsg))
        {
            return new CancelScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.SUCCESS,
                Message = ""
            };
        }
        else
        {
            return new CancelScheduleTaskResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = responseMsg,
            };
        }
    }

    [HttpGet("reBindSilo", Name = "ReBindSilo")]
    public async Task<string> ReBindSilo(string siloCode, string deviceId, string rackCode = "")
    {
        _logger.LogWarning($"ReBindSilo,siloCode:{siloCode},deviceId:{deviceId},rackCode:{rackCode}, now started.");

        var deviceProxy = _deviceManager.GetOnlineDevice(deviceId);
        if (deviceProxy == null)
        {
            return ErrorCodes.Sys.WRONG_DEVICE_MESSAGE;
        }
        var panels = await _siloAdapter.GetSiloPanels(siloCode);
        var serviceRequest = new DeviceServiceInvokeRequest
        {
            ProductId = deviceProxy.Descriptor.ProductId,
            DeviceId = deviceProxy.Descriptor.DeviceId,
            ClientId = deviceProxy.ClientId,
            ServiceId = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
            ServiceName = "SetSiloCommand",
            TargetProductId = deviceProxy.Descriptor.ProductId,
            TargetDeviceId = deviceProxy.Descriptor.DeviceId,
            TargetClientId = deviceProxy.ClientId,
            PayloadPanels = PanelList.FromList(panels),
        };

        if (!string.IsNullOrEmpty(rackCode))
        {
            serviceRequest.LocationCode = rackCode;
        }

        var response = await deviceProxy.InvokeService(serviceRequest);
        if (response.Code == ErrorCodes.Sys.SUCCESS)
        {
            _logger.LogInformation($"ReBindSilo,siloCode:{siloCode},deviceId:{deviceId},now succeed.");
            return string.Empty;
        }
        else
        {
            _logger.LogWarning($"ReBindSilo,siloCode:{siloCode},deviceId:{deviceId},now failed.");
            return $"ReBindSilo,siloCode:{siloCode},deviceId:{deviceId},now failed.";
        }
    }

    [HttpPost("allotsPanelData", Name = "AllotsPanelData")]
    public async Task<string> AllotsPanelData(AllotsPanelDataRequest request)
    {
        if (string.IsNullOrEmpty(request.DeviceCode))
        {
            return "未识别有效的DeviceCode！";
        }
        StringBuilder layerStr = new StringBuilder();
        if (request.Layers != null && request.Layers.Count > 0)
        {
            foreach (var layer in request.Layers)
            {
                layerStr.Append($"{layer.ObjToString()}|");
            }
        }
        _logger.LogWarning($"AllotsPanelData,deviceId:{request.DeviceCode},layer:{layerStr.ToString()},rackCode:{request.RackCode} now started.");

        var deviceProxy = _deviceManager.GetOnlineDevice(request.DeviceCode);
        if (deviceProxy == null)
        {
            return ErrorCodes.Sys.WRONG_DEVICE_MESSAGE;
        }

        var panels = new List<Fundation.Iot.Models.Panel>();
        string serviceName = "SetSiloCommand";
        if (deviceProxy.Descriptor.ProductId.ToLower().Contains("drill"))
        {
            if (request.Layers == null || request.Layers.Count == 0)
            {
                return "未识别有效的Layers！";
            }
            panels = await _siloAdapter.GetSiloPanelsByDrillDevice(request.DeviceCode, request.Layers);
            serviceName = "SetDrillCommand";
        }
        else if (deviceProxy.Descriptor.ProductId.ToLower().Contains("agv"))
        {
            panels = await _siloAdapter.GetSiloPanelsByDevice(request.DeviceCode);
            serviceName = "SetAgvCommand";

            if (panels.Count > 0)
            {
                await deviceProxy.SetPanels(PanelList.FromList(panels));
            }
        }
        else
        {
            if (string.IsNullOrEmpty(request.RackCode))
            {
                return "未识别有效的RackCode！";
            }
            panels = await _siloAdapter.GetSiloPanelsByDevice(request.RackCode);

            if (panels.Count > 0)
            {
                await deviceProxy.SetPanels(PanelList.FromList(panels));
            }
        }

        if (panels.Count == 0)
        {
            return "未找到板料数据！";
        }

        var serviceRequest = new DeviceServiceInvokeRequest
        {
            ProductId = deviceProxy.Descriptor.ProductId,
            DeviceId = deviceProxy.Descriptor.DeviceId,
            ClientId = deviceProxy.ClientId,
            ServiceId = Topics.Services.REMOTE_COMMAND_SERVICE_ID,
            ServiceName = serviceName,
            TargetProductId = deviceProxy.Descriptor.ProductId,
            TargetDeviceId = deviceProxy.Descriptor.DeviceId,
            TargetClientId = deviceProxy.ClientId,
            PayloadPanels = PanelList.FromList(panels),
        };

        if (serviceName == "SetSiloCommand")
        {
            serviceRequest.LocationCode = request.RackCode;
            serviceRequest.Params = new Dictionary<string, object?> { { "DeviceCode", request.RackCode } };
        }

        //var locationCode = string.IsNullOrEmpty(request.RackCode) ? request.DeviceCode : request.RackCode;
        //if (_locationManager.TryGetLocation(locationCode, out var location) && location != null)
        //{
        //    location.SetPanels(PanelList.FromList(panels));
        //}

        var response = await deviceProxy.InvokeService(serviceRequest);
        if (response.Code == ErrorCodes.Sys.SUCCESS)
        {
            _logger.LogInformation($"AllotsPanelData,deviceId:{request.DeviceCode},layer:{layerStr.ToString()},now succeed.");
            return string.Empty;
        }
        else
        {
            _logger.LogWarning($"AllotsPanelData,deviceId:{request.DeviceCode},layer:{layerStr.ToString()},now failed.");
            return $"AllotsPanelData,deviceId:{request.DeviceCode},layer:{layerStr.ToString()},now failed.";
        }
    }
}
