using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Polly;
using VegaIot.External.AgvEntity.Hik;
using VegaIot.External.HikAgv.Handler;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;
using static VegaIot.External.Agv.ErrorCodes;

namespace VegaIot.External.HikAgv;

/// <summary>
/// 海康 接口回调
/// </summary>
[ApiController]
[Route("agv/agvCallbackService")]
public class HikAGVController : Controller
{
    private readonly IDeviceManager _deviceHolder;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly ILogger<HikAGVController> _logger;
    private readonly IObjectFactory _objectFactory;
    private readonly HikAgvAgentOptions _hikAgvAgentOptions;

    public HikAGVController(IDeviceManager deviceHolder,
        IHttpRequestInvoker httpRequestInvoker,
        ITransferPlanManager transferPlanManager,
        IOptions<HikAgvAgentOptions> options,
        ILogger<HikAGVController> logger,
        IObjectFactory objectFactory)
    {
        _deviceHolder = deviceHolder;
        _httpRequestInvoker = httpRequestInvoker;
        _transferPlanManager = transferPlanManager;
        _logger = logger;
        _objectFactory = objectFactory;
        _hikAgvAgentOptions = options.Value;
    }

    /// <summary>
    /// 底盘车辆到位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("agvCallback")]
    public async Task<HikArrivedResponseEntity> agvCallbackAsync(HikArrivedRequestEntity status)
    {
        //_logger.LogInformation($"\r\nAGV执行的任务已完成{status.taskCode},{status.robotCode.ToStr()}=======================================\r\n");
        _logger.LogDebug($"{status?.taskCode} :AGV回调结果：{JsonSerializer.Serialize(status)}");
        _logger.LogInformation($"hikAgvAgentOptions.BaseUrlPrefix={_hikAgvAgentOptions.BaseUrlPrefix}");

        var response = new HikArrivedResponseEntity();

        _logger.LogInformation($"==============================开始底盘任务==========================================\r\n");

        if (string.IsNullOrEmpty(status?.robotCode))
        {
            _logger.LogWarning($"agvCallback is callded with null status.robotCode input.");
            return response;
        }

        var agvId = status.robotCode.ToStr();
        agvId = agvId.Replace("#", "");

        if (!_deviceHolder.TryGetLocalDevice(agvId, out DeviceProxy device) || device == null)
        //var device = _deviceHolder.GetOnlineDevice(agvId);
        //if (device == null)
        {
            _logger.LogError($"CCS(central contral system) deviceHolder Can't get device[{agvId}]");
            return response;
        }

        var url = device.Descriptor.HostAddress + "/" + _hikAgvAgentOptions.BaseUrlPrefix + "/agvCallback";
        _logger.LogInformation($"{status?.taskCode}:agvCallback_Request: \r\n {JsonSerializer.Serialize(status)}, \r\n  Url: {url}");

        //int maxRetries = 10; // 设置最大重试次数
        //int delayBetweenRetries = 6000; // 设置重试间隔（单位：毫秒）
        //for (int attempt = 1; attempt <= maxRetries; attempt++)
        //{
        //    _logger.LogInformation($"start send post agvAgent...");

        //    var result = await _httpRequestInvoker.PostAsJsonAsync<HikArrivedRequestEntity, HikArrivedResponseEntity>(url, status);
        //    if (result != null
        //        && !string.IsNullOrEmpty(result.reqCode))
        //    {
        //        _logger.LogInformation($"Send successfully.frequency of {attempt}");
        //        return result;
        //    }

        //    if (attempt < maxRetries)
        //    {
        //        _logger.LogInformation($"Retrying in {delayBetweenRetries} ms...frequency of {attempt}");
        //        await Task.Delay(delayBetweenRetries); // 等待一段时间再重试
        //    }
        //}
        //// 如果所有重试都失败，返回默认响应
        //return response;

        return await Policy.HandleInner<Exception>()
            .WaitAndRetryAsync(3, t => TimeSpan.FromSeconds(t), (outcome, i, ctx) =>
            {
                _logger.LogInformation($"agvCallbackAsync is retrying at {i} times...");
            })
            .ExecuteAsync(async () =>
            {
                var result = await _httpRequestInvoker.PostAsJsonAsync<HikArrivedRequestEntity, HikArrivedResponseEntity>(url, status!);
                _logger.LogInformation($"{status?.taskCode}:agvCallback_Response: {JsonSerializer.Serialize(result)}");
                _logger.LogInformation($"==============================结束底盘任务==========================================\r\n");
                return result ?? response;
            });
    }

    /// <summary>
    /// 潜伏小车车辆到位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("transAgvCallback")]
    public async Task<HikArrivedResponseEntity> transAgvCallbackAsync(HikArrivedRequestEntity status)
    {
        _logger.LogDebug($"AGV回调结果：{JsonSerializer.Serialize(status)}");

        var response = new HikArrivedResponseEntity();

        var agvTask = _transferPlanManager.TransferJobs.Where(t => !string.IsNullOrEmpty(status.taskCode) &&
                        t.HikResponseKey == status.taskCode).FirstOrDefault();
        if (agvTask == null
            || !status.taskCode.ToLower().Contains("vega")
            || status.method.IsNullOrEmpty())
        {
            _logger.LogInformation($"未找到中控下发的任务，任务编号:{status.taskCode}");
            _logger.LogWarning($"未找到中控下发的任务，任务编号:{status.taskCode}");
            return new HikArrivedResponseEntity
            {
                code = ERR_CODE,
                message = NOT_FIND_CENTRAL_TASK
            };
        }

        if (agvTask.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
        {
            _logger.LogWarning($"the task has been assigned and cannot be reassigned.");
            return new HikArrivedResponseEntity
            {
                code = ERR_CODE,
                message = ALREADY_SEND
            };
        }

        _logger.LogInformation($"==============================开始潜伏AGV任务==============================================\r\n");

        #region Mode:上、下PIN <---> 中转位(适用:崇达)

        //空料仓：中转位->上PIN
        if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_PIN)
        {
            var handler = _objectFactory.GetOrCreate<EmptyBoxFromForkToPinHandler>();
            return await handler.Handle(agvTask, status);
        }
        //生料：上PIN->中转位
        if (agvTask.TransferBehavior == SiloTransferBehavior.UNDRILLED_FROM_PIN_TO_FORK)
        {
            var handler = _objectFactory.GetOrCreate<UndrilledFromPinToForkHandler>();
            return await handler.Handle(agvTask, status);
        }
        //熟料:中转位->下PIN
        if (agvTask.TransferBehavior == SiloTransferBehavior.DRILLED_FROM_FORK_TO_UNPIN)
        {
            var handler = _objectFactory.GetOrCreate<DrilledFromForkToUnPinHandler>();
            return await handler.Handle(agvTask, status);
        }
        //空料仓:下PIN->中转位
        if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_UNPIN_TO_FORK)
        {
            var handler = _objectFactory.GetOrCreate<EmptyBoxFromUnpinToForkHandler>();
            return await handler.Handle(agvTask, status);
        }

        #endregion Mode:上、下PIN <---> 中转位(适用:崇达)

        #region Mode:中转位 <---> 外部(适用:江西景旺)

        //熟料:中转位->（外部）熟料区
        if (agvTask.TransferBehavior == SiloTransferBehavior.DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN)
        {
            var handler = _objectFactory.GetOrCreate<DrilledFromForkToOutSideUnPinHandler>();
            return await handler.Handle(agvTask, status);
        }
        //首件:中转位->（外部）检验区
        if (agvTask.TransferBehavior == SiloTransferBehavior.FIRST_DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN)
        {
            var handler = _objectFactory.GetOrCreate<DrilledFromForkToOutSideUnPinHandler>();
            return await handler.Handle(agvTask, status);
        }
        //空料仓：中转位->（外部）空料仓区
        if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_OUTSIDE)
        {
            var handler = _objectFactory.GetOrCreate<EmptyBoxFromForkToOutSideHandler>();
            return await handler.Handle(agvTask, status);
        }
        //空料仓：（外部）空料仓区->中转位
        if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_FORK)
        {
            var handler = _objectFactory.GetOrCreate<EmptyBoxFromOutSideToForkHandler>();
            return await handler.Handle(agvTask, status);
        }
        //熟料：（外部）熟料区->中转位
        if (agvTask.TransferBehavior == SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_FORK)
        {
            var handler = _objectFactory.GetOrCreate<UndrilledFromOutSideToForkHandler>();
            return await handler.Handle(agvTask, status);
        }

        #endregion Mode:中转位 <---> 外部(适用:江西景旺)

        #region Mode:上、下PIN <---> 线边仓(适用:博敏)

        //熟料:线边仓->下PIN
        if (agvTask.TransferBehavior == SiloTransferBehavior.DRILLED_FROM_WIP_TO_UNPIN)
        {
            var handler = _objectFactory.GetOrCreate<DrilledFromWipToUnPinHandler>();
            return await handler.Handle(agvTask, status);
        }
        //空料仓:下PIN->线边仓
        if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_UNPIN_TO_WIP)
        {
            var handler = _objectFactory.GetOrCreate<EmptyBoxFromUnPinToWipHandler>();
            return await handler.Handle(agvTask, status);
        }
        //空料仓:线边仓->上PIN
        if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_WIP_TO_PIN)
        {
            var handler = _objectFactory.GetOrCreate<EmptyBoxFromWipToPinHandler>();
            return await handler.Handle(agvTask, status);
        }
        //生料:上PIN->线边仓
        if (agvTask.TransferBehavior == SiloTransferBehavior.UNDRILLED_FROM_PIN_TO_WIP)
        {
            var handler = _objectFactory.GetOrCreate<UndrilledFromPinToWipHandler>();
            return await handler.Handle(agvTask, status);
        }

        #endregion Mode:上、下PIN <---> 线边仓(适用:博敏)

        #region Mode:中转位 <----> 线边仓(适用:博敏)

        //熟料：中转位->线边仓
        if (agvTask.TransferBehavior == SiloTransferBehavior.DRILLED_FROM_FORK_TO_WIP)
        {
            var handler = _objectFactory.GetOrCreate<DrilledFromForkToWipHandler>();
            return await handler.Handle(agvTask, status);
        }
        //空料仓：中转位->线边仓
        if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_WIP)
        {
            var handler = _objectFactory.GetOrCreate<EmptyBoxFromForkToWipHandler>();
            return await handler.Handle(agvTask, status);
        }
        //空料仓：线边仓->中转位
        if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_WIP_TO_FORK)
        {
            var handler = _objectFactory.GetOrCreate<EmptyBoxFromWipToForkHandler>();
            return await handler.Handle(agvTask, status);
        }
        //生料：中转位->线边仓
        if (agvTask.TransferBehavior == SiloTransferBehavior.UNDRILLED_FROM_FORK_TO_WIP)
        {
            var handler = _objectFactory.GetOrCreate<UndrilledFromForkToWipHandler>();
            return await handler.Handle(agvTask, status);
        }
        //生料：线边仓->中转位
        if (agvTask.TransferBehavior == SiloTransferBehavior.UNDRILLED_FROM_WIP_TO_FORK)
        {
            var handler = _objectFactory.GetOrCreate<UndrilledFromWipToForkHandler>();
            return await handler.Handle(agvTask, status);
        }

        #endregion Mode:中转位 <----> 线边仓(适用:博敏)

        _logger.LogInformation($"==============================结束潜伏AGV任务==============================================\r\n");
        return response;
    }

    /// <summary>
    /// AGV进行下一步动作需要请示代理
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("battery")]
    public async Task<HikBatteryResponseEntity> BatteryAsync(HikBatteryRequestEntity request)
    {
        var agvId = request.AGV_ID.ToStr();
        agvId = agvId.Replace("#", "");

        if (!_deviceHolder.TryGetLocalDevice(agvId, out DeviceProxy device) || device == null)
        //var device = _deviceHolder.GetOnlineDevice(agvId);
        //if (device == null)
        {
            _logger.LogError($"CCS(central contral system) deviceHolder Can't get device[{agvId}]");
            return new HikBatteryResponseEntity
            {
                AGV_ID = agvId,
            };
        }

        var url = device.Descriptor.HostAddress + "/" + _hikAgvAgentOptions.BaseUrlPrefix + "/battery";
        _logger.LogDebug(url);
        return await _httpRequestInvoker.PostAsJsonAsync<HikBatteryRequestEntity, HikBatteryResponseEntity>(url, request);
    }
}
