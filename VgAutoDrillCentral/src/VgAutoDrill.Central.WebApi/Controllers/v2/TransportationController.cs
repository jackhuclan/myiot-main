using System.Text.Json;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

using SqlSugar;

using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.WebApi.Controllers.v2;

[Route("v2/central/transportation")]
[ApiController]
public class TransportationController
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ILogger<TransportationController> _logger;
    private readonly ITransferPlanManager _transitPlanManager;
    private readonly IScheduleTaskAdapter _ScheduleTaskAdapter;
    private readonly IRackService _RackService;
    private readonly IDistributedCache _distributedCache;
    public TransportationController(ISysConfigManager sysConfigManager,
        ITransferPlanManager transitPlanManager,
        IScheduleTaskAdapter scheduleTaskAdapter,
        IRackService rackService,
        ILogger<TransportationController> logger,
        IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _transitPlanManager = transitPlanManager;
        _ScheduleTaskAdapter = scheduleTaskAdapter;
        _RackService = rackService;
        _sysConfigManager = sysConfigManager;
        _logger = logger;
    }

    /// <summary>
    /// 获取料仓转运任务
    /// </summary>
    /// <returns></returns>
    [HttpPost("getTransportations")]
    public List<TransferJob> GetTransportations(QueryTransportationRequest request)
    {
        var response = _transitPlanManager.TransferJobs.ToList();
        _logger.LogDebug($"getTransportations, return {JsonSerializer.Serialize(response)}");

        return response;
    }

    /// <summary>
    /// 获取待执行的料仓转运任务
    /// </summary>
    /// <returns></returns>
    [HttpGet("getTodoTransportations")]
    public async Task<List<TransferJob>> GetTodoTransportations(string warehouseCode = "")
    {
        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"中转区正在维护，暂停料仓转运的任务，请稍候.");
            return new List<TransferJob>();
        }

        var result = new List<TransferJob>();
        result.AddRange(_transitPlanManager.TransferJobs.Where(x => x.ScheduledTaskStatus == ScheduledTaskStatus.Created));

        //var transferJob = await _transitPlanManager.TryGenerateTransferJob();
        //if (transferJob != null)
        //{
        //    result.Add(transferJob);
        //    _logger.LogDebug($"GetTodoTransportations, return {JsonSerializer.Serialize(transferJob)}");
        //}
        //else
        //{
        //    _logger.LogDebug($"GetTodoTransportations, 未能生成转运任务");
        //}

        return result;
    }

    [HttpPost("fail", Name = "FailTransportation")]
    public async Task<ServiceResponse> FailTransportation(ServiceRequest request)
    {
        _logger.LogWarning($"fail,request:{JsonSerializer.Serialize(request)}");
        ThrowHelper.ThrowArgumentNullException(request);

        return await _transitPlanManager.UpdateStatus(request, ScheduledTaskStatus.Failed);
    }
    [HttpPost("begin", Name = "BeginTransportation")]
    public async Task<ServiceResponse> BeginTransportation(ServiceRequest request)
    {
        _logger.LogWarning($"begin,request:{JsonSerializer.Serialize(request)}");
        ThrowHelper.ThrowArgumentNullException(request);

        if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
        {
            _logger.LogWarning($"中转区正在维护，暂停料仓转运的任务，请稍候.");
            return new ServiceResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"中转区正在维护，暂停料仓转运的任务，请稍候.",
            };
        }

        return await _transitPlanManager.UpdateStatus(request, ScheduledTaskStatus.Running);
    }

    [HttpPost("complete", Name = "CompleteTransportation")]
    public async Task<ServiceResponse> CompleteTransportation(ServiceRequest request)
    {
        _logger.LogWarning($"complete,request:{JsonSerializer.Serialize(request)}");
        ThrowHelper.ThrowArgumentNullException(request);

        return await _transitPlanManager.UpdateStatus(request, ScheduledTaskStatus.Completed);
    }

    [HttpPost("cancel", Name = "CancelTransportation")]
    public async Task<ServiceResponse> CancelTransportation(ServiceRequest request)
    {
        _logger.LogWarning($"cancel,request:{JsonSerializer.Serialize(request)}");
        ThrowHelper.ThrowArgumentNullException(request);

        return await _transitPlanManager.UpdateStatus(request, ScheduledTaskStatus.Canceled);
    }

    [HttpPost("addTransferJobLog", Name = "AddTransferJobLog")]
    public async Task<AddTranserJobResponse> AddTransferJobLog(AddTranserJobLogRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        _logger.LogDebug($"AddTransferJobLog,TransferJobId:{request.TransferJobId},device:{request.DeviceId},message:{request.Message}");

        return await _transitPlanManager.AddTransferJobLog(request);
    }

    #region -- 手动接口 --
    /// <summary>
    /// 生成手动钻机任务
    /// </summary>
    /// <remarks>
    /// 下熟料：
    ///     目标点：  不能为空
    ///     物料编码：不能为空
    ///     料仓编码：不能为空
    ///     熟料数量：不能为0
    ///     
    /// 上生料:
    ///     目标点：  不能为空
    ///     物料编码：不能为空
    ///     
    /// 空料仓从中转位到线边仓:
    ///     目标点：  不能为空
    ///     料仓编码：不能为空
    ///
    /// 空料仓从线边仓到中转位:
    ///     目标点：不能为空
    /// 
    /// </remarks>
    /// <param name="materialCode">物料编码</param>
    /// <param name="siloCode">料仓编码</param>
    /// <param name="destinationPoint">目标点</param>
    /// <param name="podLotNum">熟料数量</param>
    /// <returns></returns>
    [HttpGet("CreateManualTransferJob")]
    [AllowAnonymous]
    public async Task<ResponseDto<Boolean>> CreateManualTransferJob(String? materialCode, String? siloCode, String destinationPoint, Int32 podLotNum = 0)
    {
        return await Task.Run(async () =>
        {
            string message = "手动转运托盘任务";
            try
            {
                var flag = false;
                if (String.IsNullOrEmpty(destinationPoint))
                    return new ResponseDto<bool>() { Code = ResponseCode.Fail };

                var jobName = string.Empty;
                var id = DateTime.Now.Ticks;
                var job = new TransferJob()
                {
                    ScheduledTaskStatus = ScheduledTaskStatus.Created,
                    CreateTime = DateTime.Now,
                    ForkCode = destinationPoint,
                    IsUrgent = 0,
                };
                var _isSiloCode = !String.IsNullOrEmpty(siloCode);
                var _isMaterialCode = !String.IsNullOrEmpty(materialCode);
                var _isClinkerCount = podLotNum != 0;

                job.PartitionCode = "Manual";
                // 下熟料的规则
                if (_isClinkerCount && _isMaterialCode && _isSiloCode)
                {
                    job.Code = $@"VEGA_{id}";
                    job.TransferBehavior = SiloTransferBehavior.DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN;
                    job.ClinkerCount = podLotNum;
                    job.StartLocationCode = destinationPoint;
                    job.MasterLocationCode = destinationPoint;
                    job.SiloCode = siloCode;
                    job.InternalLotNo = materialCode;
                    job.TransportationKind = TransportationKind.Clinker;
                    job.TransferDesc = $@"手动钻机下熟料任务:从{destinationPoint}点转运{podLotNum}块物料编码为{materialCode}的{siloCode}熟料仓到线边仓";
                    job.InteractionSequence = InteractionSequence.UnloadOnly;
                    // _scheduleRequestInfo.InteractionSequence = Admin.Model.CentralModels.InteractionSequence.UnloadOnly;
                    flag = true;
                    message = "手动下熟料任务";
                }
                // 上生料规则
                if (_isMaterialCode && !_isSiloCode)
                {
                    job.Code = $@"VEGA_{id}";
                    job.TransferBehavior = SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_FORK;
                    job.EndLocationCode = destinationPoint;
                    job.InternalLotNo = materialCode;
                    job.MasterLocationCode = destinationPoint;
                    job.TransportationKind = TransportationKind.Raw;
                    job.TransferDesc = $@"手动钻机上生料任务:从线边仓转运物料编码为{materialCode}的生料到{destinationPoint}点";
                    job.InteractionSequence = InteractionSequence.LoadOnly;
                    //  _scheduleRequestInfo.InteractionSequence = Admin.Model.CentralModels.InteractionSequence.LoadOnly;
                    message = "手动上生料任务";
                    flag = true;
                }
                // 空料仓从中转位到线边仓规则
                if (_isSiloCode && !_isMaterialCode)
                {
                    job.Code = $@"VEGA_{id}";
                    job.TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_OUTSIDE;
                    job.StartLocationCode = destinationPoint;
                    job.SiloCode = siloCode;
                    job.MasterLocationCode = destinationPoint;
                    job.TransportationKind = TransportationKind.EmptySilo;
                    job.TransferDesc = $@"手动钻机从中转位到线边仓:从{destinationPoint}转运{siloCode}空料仓到线边仓";
                    job.InteractionSequence = InteractionSequence.UnloadOnly;
                    //   _scheduleRequestInfo.InteractionSequence = Admin.Model.CentralModels.InteractionSequence.UnloadOnly;
                    flag = true;
                    message = "手动下空仓任务";
                }
                // 空料仓从线边仓到中转位规则
                if (!_isSiloCode && !_isMaterialCode)
                {
                    job.Code = $@"VEGA_{id}";
                    job.TransferBehavior = SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_FORK;
                    job.EndLocationCode = destinationPoint;
                    job.MasterLocationCode = destinationPoint;
                    job.TransportationKind = TransportationKind.EmptySilo;
                    job.TransferDesc = $@"手动钻机从线边仓到中转位:从线边仓转运空料仓到{destinationPoint}";
                    job.InteractionSequence = InteractionSequence.LoadOnly;
                    //  _scheduleRequestInfo.InteractionSequence = Admin.Model.CentralModels.InteractionSequence.LoadOnly;
                    flag = true;
                    message = "手动上空仓任务";
                }

                flag = await _transitPlanManager.TryAddManualTransferJob(job);
                if (flag)
                {
                    await _distributedCache.SetStringAsync(destinationPoint, $"{message}下发成功！");
                }
                else
                {
                    await _distributedCache.SetStringAsync(destinationPoint, $"{message}下发失败！");
                }

                return new ResponseDto<bool>() { Code = flag ? ResponseCode.Success : ResponseCode.Fail };
            }
            catch (Exception ex)
            {
                await _distributedCache.SetStringAsync(destinationPoint, $"{message}下发异常！");
                return new ResponseDto<bool>() { Code = ResponseCode.GlobalExption, Message = ex.Message };
            }
        });
    }
    #endregion
}
