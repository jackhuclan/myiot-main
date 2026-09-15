using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;

using VegaIot.External.AgvEntity;
using VegaIot.External.AgvEntity.STD;
using VegaIot.External.StdAgv.Handler;

using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

using static VegaIot.External.Agv.ErrorCodes;

using Policy = Polly.Policy;
using ScheduledTaskStatus = VgAutoDrill.Fundation.Iot.Schedule.ScheduledTaskStatus;

namespace VegaIot.External.StdAgv.Controllers;

/// <summary>
/// 海康 接口回调
/// </summary>
[ApiController]
[Route("v1/std")]
public class StdAGVController : Controller
{
    private readonly IDeviceManager _deviceHolder;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly ILogger<StdAGVController> _logger;
    private readonly IObjectFactory _objectFactory;
    private readonly StdAgvAgentOptions _stdAgvAgentOptions;
    private readonly StdAgvConfig _StdAgvConfig;
    private readonly IManualCallAgvTaskService _manualCallAgvLogService;
    private readonly IDistributedCache _distributedCache;

    public StdAGVController(IDeviceManager deviceHolder,
        IHttpRequestInvoker httpRequestInvoker,
        ITransferPlanManager transferPlanManager,
        IOptions<StdAgvAgentOptions> options,
        ILogger<StdAGVController> logger,
        ISysConfigManager sysConfigManager,
        IOptions<StdAgvSchedulerOptions> schedulerOptions,
        StdAgvConfig stdAgvConfig,
        IObjectFactory objectFactory,
        IManualCallAgvTaskService manualCallAgvTaskService,
        IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _manualCallAgvLogService = manualCallAgvTaskService;
        _deviceHolder = deviceHolder;
        _httpRequestInvoker = httpRequestInvoker;
        _transferPlanManager = transferPlanManager;
        _logger = logger;
        _objectFactory = objectFactory;
        _stdAgvAgentOptions = options.Value;
        _StdAgvConfig = stdAgvConfig;
    }

    #region -- 自动 --

    /// <summary>
    /// 大车底盘车辆到位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("arrived")]
    public async Task<StdArrivedResponseEntity> agvCallbackAsync(StdArrivedRequestEntity status)
    {
        _logger.LogInformation($"{status?.VehicleName} :AGV回调结果：{status.ToJson()}");

        var response = new StdArrivedResponseEntity(201, "接口异常!");
        _logger.LogInformation($"==============================开始底盘任务==========================================\r\n");

        if (string.IsNullOrWhiteSpace(status?.VehicleName))
        {
            _logger.LogError($"STD agvCallbackAsync  参数 : VehicleName 不正确! ");
            return new StdArrivedResponseEntity(201, "VehicleName 不正确");
        }

        var agvId = status.VehicleName.ToStr();
        agvId = agvId.Replace("#", "");

        if (!_deviceHolder.TryGetLocalDevice(agvId, out DeviceProxy device) || device == null)
        {
            _logger.LogError($"STD agvCallbackAsync  deviceHolder Can't get device[{agvId}]");
            return new StdArrivedResponseEntity(201, "deviceHolder Can't get device agvId ");
        }

        var url = device.Descriptor.HostAddress + "/" + _stdAgvAgentOptions.BaseUrlPrefix + "/arrived";
        _logger.LogInformation($"任务ID：{status?.OrderId},任务号：{status?.OrderNo}:agvCallback_Request: \r\n {status.ToJson()}, \r\n  Url: {url}");

        return await Policy.HandleInner<Exception>()
            .WaitAndRetryAsync(3, t => TimeSpan.FromSeconds(t), (outcome, i, ctx) =>
            {
                _logger.LogError($"agvCallbackAsync is retrying at {i} times...\r\n ErrorMessage:{outcome.ToString()}");
            })
            .ExecuteAsync(async () =>
            {
                var result = await _httpRequestInvoker.PostAsJsonAsync<StdArrivedRequestEntity, StdArrivedResponseEntity>(url, status!);
                _logger.LogInformation($"{status?.OrderNo}:agvCallback_Response: {result.ToJson()}");
                _logger.LogInformation($"==============================结束底盘任务==========================================\r\n");
                return result ?? response;
            });
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("askLeave")]
    public async Task<StdCanLeaveResponseEntity> AskLeave(StdCanLeaveRequestEntity agv)
    {
        _logger.LogInformation($"{agv?.VehicleName} : askLeave 接收参数：{agv.ToJson()}");

        var response = new StdCanLeaveResponseEntity(0, false);

        _logger.LogInformation($"==============================开始底盘任务==========================================\r\n");

        if (string.IsNullOrWhiteSpace(agv?.VehicleName))
        {
            _logger.LogError($"aAskLeave 接收的参数 VehicleName 不正确!");
            return response;
        }

        var agvId = agv.VehicleName.Trim().ToStr();
        agvId = agvId.Replace("#", "");

        if (!_deviceHolder.TryGetLocalDevice(agvId, out DeviceProxy device) || device == null)
        //var device = _deviceHolder.GetOnlineDevice(agvId);
        //if (device == null)
        {
            _logger.LogError($"CCS(central contral system) deviceHolder Can't get device[{agvId}]");
            return response;
        }

        var url = device.Descriptor.HostAddress + "/" + _stdAgvAgentOptions.BaseUrlPrefix + "/askLeave";
        _logger.LogInformation($"{agv?.VehicleName}:agvCallback_Request: \r\n {agv.ToJson()}, \r\n  Url: {url}");

        return await Policy.HandleInner<Exception>()
            .WaitAndRetryAsync(3, t => TimeSpan.FromSeconds(t), (outcome, i, ctx) =>
            {
                _logger.LogInformation($"agvCallbackAsync is retrying at {i} times...");
            })
            .ExecuteAsync(async () =>
            {
                var result = await _httpRequestInvoker.PostAsJsonAsync<StdCanLeaveRequestEntity, StdCanLeaveResponseEntity>(url, agv!);
                _logger.LogInformation($"{agv?.VehicleName}:agvCallback_Response: {result.ToJson()}");
                _logger.LogInformation($"==============================结束底盘任务==========================================\r\n");
                return result ?? response;
            });
    }

    /// <summary>
    /// 从料仓取料后的回调接口
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    [HttpPost("TaskCompleteCallBack")]
    [AllowAnonymous]
    public async Task<StdArrivedResponseEntityV2> TaskCompleteCallBack(STDMaterialsRequestEntity reqData)
    {
        return await AgvCallbackAsync(reqData, true);
    }

    /// <summary>
    /// 潜伏小车车辆到位
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    private async Task<StdArrivedResponseEntityV2> AgvCallbackAsync(STDMaterialsRequestEntity status, Boolean isLoading)
    {
        _logger.LogDebug($"AgvCallbackAsync：回调结果: {status.ToJson()}");

        VgAutoDrill.Central.Core.Domain.TransferJob agvTask = new VgAutoDrill.Central.Core.Domain.TransferJob();
        var response = new StdArrivedResponseEntityV2();

        try
        {
            agvTask = _transferPlanManager.TransferJobs.Where(t => !string.IsNullOrEmpty(status.taskId) &&
                      t.HikResponseKey == status.taskId).FirstOrDefault()!;

            if (agvTask == null)
            {
                _logger.LogWarning($"未找到中控下发的任务，任务编号:{status.taskId}");

                return new StdArrivedResponseEntityV2
                {
                    Code = ERR_CODE,
                    Message = NOT_FIND_CENTRAL_TASK
                };
            }

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = agvTask.Id,
                Message = $"收到小车回调信息: {status.ToJson()} \r\n TransJob 信息: ScheduledTaskStatus: {agvTask.ScheduledTaskStatus},TransferBehavior :{agvTask.TransferBehavior} "
            });

            var methodName = string.Empty;

            if (agvTask.InteractionSequence == InteractionSequence.UnloadOnly && isLoading)
            {
                methodName = "outbin";
            }
            else
            {
                methodName = "end";
            }

            if (agvTask.ScheduledTaskStatus == ScheduledTaskStatus.Completed)
            {
                _logger.LogWarning($"the task has been assigned and cannot be reassigned.");
                return new StdArrivedResponseEntityV2
                {
                    Code = SUCCESS,
                    Message = ALREADY_SEND
                };
            }

            _logger.LogInformation($"==============================开始潜伏AGV任务==============================================\r\n");

            #region Mode:中转位 <---> 外部

            //熟料:中转位->（外部）熟料区
            if (agvTask.TransferBehavior == SiloTransferBehavior.DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN)
            {
                var handler = _objectFactory.GetOrCreate<DrilledFromForkToOutSideUnPinHandler>();
                return await handler.Handle(agvTask, methodName, status.vehicleName);
            }
            //首件:中转位->（外部）检验区
            if (agvTask.TransferBehavior == SiloTransferBehavior.FIRST_DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN)
            {
                var handler = _objectFactory.GetOrCreate<DrilledFromForkToOutSideUnPinHandler>();
                return await handler.Handle(agvTask, methodName, status.vehicleName);
            }
            //空料仓：中转位->（外部）空料仓区
            if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_OUTSIDE)
            {
                var handler = _objectFactory.GetOrCreate<EmptyBoxFromForkToOutSideHandler>();
                return await handler.Handle(agvTask, methodName, status.vehicleName);
            }
            //空料仓：（外部）空料仓区->中转位
            if (agvTask.TransferBehavior == SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_FORK)
            {
                var handler = _objectFactory.GetOrCreate<EmptyBoxFromOutSideToForkHandler>();
                return await handler.Handle(agvTask, methodName, status.vehicleName, status.trayNum);
            }
            //生料：（外部）生料区->中转位
            if (agvTask.TransferBehavior == SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_FORK)
            {
                var handler = _objectFactory.GetOrCreate<UndrilledFromOutSideToForkHandler>();
                return await handler.Handle(agvTask, methodName, status.vehicleName, status.materialList, status.trayNum);
            }

            //生料：（外部）中转位->生料区
            if (agvTask.TransferBehavior == SiloTransferBehavior.UNDRILLED_FROM_FORK_TO_OUTSIDE)
            {
                var handler = _objectFactory.GetOrCreate<UndrilledFromForkToOutSideHandler>();
                return await handler.Handle(agvTask, methodName, status.vehicleName);
            }

            #endregion Mode:中转位 <---> 外部

            _logger.LogInformation($"==============================结束潜伏AGV任务==============================================\r\n");
        }
        catch (Exception ex)
        {
            _logger.LogError($"料仓任务: ID: {agvTask} 回调处理异常：{ex.ToJson()}");
            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = agvTask.Id,
                Message = $"收到小车回调后处理异常: 内容：\r\n {ex.Message} "
            });
        }

        return new StdArrivedResponseEntityV2
        {
            Code = ERR_CODE,
            Message = $"处理异常！"
        };
    }

    /// <summary>
    /// AGV进行下一步动作需要请示代理
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("battery")]
    public async Task<StdBatteryResponseEntity> BatteryAsync(StdBatteryRequestEntity request)
    {
        var agvId = request.AGV_ID.ToStr();
        agvId = agvId.Replace("#", "");

        if (!_deviceHolder.TryGetLocalDevice(agvId, out DeviceProxy device) || device == null)
        //var device = _deviceHolder.GetOnlineDevice(agvId);
        //if (device == null)
        {
            _logger.LogError($"CCS(central contral system) deviceHolder Can't get device[{agvId}]");
            return new StdBatteryResponseEntity
            {
                AGV_ID = agvId,
            };
        }

        var url = device.Descriptor.HostAddress + "/" + _stdAgvAgentOptions.BaseUrlPrefix + "/battery";
        _logger.LogDebug(url);
        return await _httpRequestInvoker.PostAsJsonAsync<StdBatteryRequestEntity, StdBatteryResponseEntity>(url, request);
    }

    #endregion -- 自动 --

    #region -- 手动 --

    [HttpPost("ManualTaskCallBack")]
    [AllowAnonymous]
    public async Task<STDResponse?> ManualTaskCallBack(ManualCallBackRequest request)
    {
        long taskId = 0;

        try
        {
            _logger.LogDebug($"ManualTaskCallBack：回调结果: {request.ToJson()}");

            if (_transferPlanManager.TryGetAgvTask(request.taskId, out var result))
            {
                taskId = result.Id;
                await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
                {
                    TransferJobId = result.Id,
                    Message = $"收到小车回调信息: {request.ToJson()} "
                });

                var _LocationCode = string.IsNullOrWhiteSpace(result.StartLocationCode) ? result.EndLocationCode : result.StartLocationCode;

                var manualCallAgvLog = await _manualCallAgvLogService.QueryByLocationCode(_LocationCode);

                switch (result.TransferBehavior)
                {
                    // 下熟料
                    case SiloTransferBehavior.DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN:
                        manualCallAgvLog.AgvOperateType = AgvOperateType.UnloadClinker;
                        manualCallAgvLog.AgvOperateName = $@"下料";
                        manualCallAgvLog.ClinkerMaterialNum = result.ClinkerCount;
                        manualCallAgvLog.PodCode = "";
                        break;
                    // 上生料
                    case SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_FORK:
                        manualCallAgvLog.AgvOperateType = AgvOperateType.UploadRawMaterial;
                        manualCallAgvLog.AgvOperateName = $@"上料";
                        manualCallAgvLog.IsBind = true;
                        manualCallAgvLog.PodCode = request.trayNum;
                        manualCallAgvLog.ItemCode = request.materialList?.FirstOrDefault()?.lot;
                        break;
                    // 空料仓从中转位到线边仓
                    case SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_OUTSIDE:
                        manualCallAgvLog.AgvOperateType = AgvOperateType.UnloadEmptyFork;
                        manualCallAgvLog.AgvOperateName = $@"退空盘";
                        manualCallAgvLog.IsBind = false;
                        manualCallAgvLog.PodCode = "";
                        manualCallAgvLog.ItemCode = "";
                        break;
                    // 空料仓从线边仓到中转位
                    case SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_FORK:
                        manualCallAgvLog.AgvOperateType = AgvOperateType.UploadEmptyFork;
                        manualCallAgvLog.AgvOperateName = $@"叫空盘";
                        manualCallAgvLog.IsBind = false;
                        manualCallAgvLog.ItemCode = "";
                        manualCallAgvLog.PodCode = request.trayNum;
                        break;
                }

                await _distributedCache.SetStringAsync(_LocationCode, "AGV搬运结束");
                manualCallAgvLog.TaskStatus = ManualCallAgvTaskStatus.Completed;
                await _manualCallAgvLogService.Update(manualCallAgvLog);

                await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
                {
                    TransferJobId = result.Id,
                    Message = $"更新手动任务信息完成! "
                });

                result.ScheduledTaskStatus = ScheduledTaskStatus.Completed;
                await _transferPlanManager.TryUpdateTransferJob(result);

                await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
                {
                    TransferJobId = result.Id,
                    Message = $"更新料仓任务信息完成!"
                });

                return new() { code = 0 };
            }
        }
        catch (Exception ex)
        {

            await _transferPlanManager.AddTransferJobLog(new AddTranserJobLogRequest
            {
                TransferJobId = taskId,
                Message = $"回调处理异常:\r\n 内容: {ex.ToJson()} "
            });

            return new() { code = 1, message = ex.ToStr() };
        }

        return new() { code = 1 };
    }

    #region -- 从Redis中获取库位状态 --
    /// <summary>
    /// 获取任务执行状态
    /// </summary>
    /// <param name="locationCode">库位编码</param>
    /// <returns></returns>
    [HttpGet("GetTaskState")]
    [AllowAnonymous]
    public async Task<STDResponse?> GetTaskState(String locationCode)
    {
        var val = await _distributedCache.GetStringAsync(locationCode);
        if (!String.IsNullOrEmpty(val))
        {
            return new()
            {
                code = 0,
                message = val
            };
        }
        return new() { code = 1, message = "无法找到该库位的运行状态" };
    }

    /// <summary>
    /// 设置任务执行状态
    /// </summary>
    /// <param name="locationCode">库位编码</param>
    /// <param name="message">任务信息</param>
    /// <returns></returns>
    [HttpGet("SetTaskState")]
    [AllowAnonymous]
    public async Task<STDResponse?> SetTaskState(String locationCode, String message)
    {
        await _distributedCache.SetStringAsync(locationCode, message);

        return new()
        {
            code = 0,
            message = $@"{locationCode}库位任务运行状态设置成功"
        };

    }
    #endregion

    #endregion -- 手动 --

    #region -- 绑定/解绑 托盘与物料 --
    /// <summary>
    /// 托盘与物料绑定
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("BoxBindFold")]
    [AllowAnonymous]
    public async Task<STDResponse?> BoxBindFold(BindSiloBoxEntity request)
    {
        try
        {
            request.reqCode = $"Vega_{DateTime.Now.Ticks.ToString()}";
            request.clientCode = "VEGA001";
            request.reqTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var _BoxBindFoldUrl = await _StdAgvConfig.GetBoxBindFoldUrl();
            var res = await _httpRequestInvoker?.PostAsJsonAsync<BindSiloBoxEntity?, STDResponse>(_BoxBindFoldUrl, request);
            return res;
        }
        catch (Exception ex)
        {
            return new()
            {
                code = 1,
                message = ex.Message
            };
        }
    }
    #endregion
}
