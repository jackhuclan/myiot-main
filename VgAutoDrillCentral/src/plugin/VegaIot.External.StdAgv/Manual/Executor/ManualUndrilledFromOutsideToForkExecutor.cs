using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using VegaIot.External.Agv.Executor;
using VegaIot.External.AgvEntity.STD;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.StdAgv.Manual.Executor;

/// <summary>
/// 手动钻机任务，上生料，区域到点
/// </summary>
internal class ManualUndrilledFromOutsideToForkExecutor : ITransferSiloExecutor
{
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly IManualCallAgvTaskService _manualCallAgvLogService;
    private readonly IDistributedCache _distributedCache;

    public ManualUndrilledFromOutsideToForkExecutor(IServiceProvider serviceProvider,
        IObjectFactory objectFactory,
        IDistributedCache distributedCache)
    {
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();
        _manualCallAgvLogService = serviceProvider.GetRequiredService<IManualCallAgvTaskService>();
        _distributedCache = distributedCache;
    }

    public async Task<bool> Execute(TransferJob job)
    {
        var manualCallAgvLog = await _manualCallAgvLogService.QueryByLocationCode(job.EndLocationCode);
        var url = await _stdAgvConfig.GetGenAgvSchedulingTaskUrl();
        var manualArea = await _stdAgvConfig.GetManualRawAreaCode();
        var path = new[] { job.InternalLotNo, job.EndLocationCode };
        // 向斯坦德小车下发任务
        var requestStd = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Code, job.ClinkerCount, "", job.InternalLotNo, StdAgvTaskCode.F11.ToString(), job.SiloCode);
        await _transferPlanManager.AddTransferJobLog(new()
        {
            TransferJobId = job.Id,
            Message = $@"斯坦德下发任务: url: {url},请求参数：{requestStd.ToJson()}"
        });
        var response = await _httpRequestInvoker?.PostAsJsonAsync<STDGenAgvSchedulingTaskRequestEntity?, STDResponse>(url, requestStd);

        job.HkResponse = response.ToJson();
        if (response != null && response.code == 0)
        {
            manualCallAgvLog.TaskStatus = ManualCallAgvTaskStatus.Distribute;
            manualCallAgvLog.TaskStatusDescription = $@"斯坦德下发任务成功，任务编号为{job.Id},执行F11上生料，从{manualArea}搬运承载编号{job.InternalLotNo}的生料{job.SiloCode}料仓到{job.EndLocationCode}，斯坦德返回信息{job.HkResponse}";
            await _manualCallAgvLogService.Update(manualCallAgvLog);

            await _transferPlanManager.AddTransferJobLog(new()
            {
                TransferJobId = job.Id,
                Message = $@"斯坦德下发任务成功，任务编号为{job.Id},执行F11上生料，从{manualArea}搬运承载编号{job.InternalLotNo}的生料{job.SiloCode}料仓到{job.EndLocationCode}，斯坦德返回信息{job.HkResponse}"
            });

            await _distributedCache.SetStringAsync(job.EndLocationCode, "AGV搬运中");
            manualCallAgvLog.TaskStatus = ManualCallAgvTaskStatus.CallBack;
            job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
            await _transferPlanManager.TryUpdateTransferJob(job);
            return true;
        }
        else
        {
            job.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            manualCallAgvLog.TaskStatus = ManualCallAgvTaskStatus.Exception;
            manualCallAgvLog.TaskStatusDescription = $@"斯坦德下发任务失败，任务编号为{job.Id},执行F11上生料，从{manualArea}搬运承载编号{job.InternalLotNo}的生料{job.SiloCode}料仓到{job.EndLocationCode}，斯坦德返回信息{job.HkResponse}";
            await _manualCallAgvLogService.Update(manualCallAgvLog);

            await _distributedCache.SetStringAsync(job.EndLocationCode, $"斯坦德任务下发失败:{response?.message.ToJson()}");
            await _transferPlanManager.AddTransferJobLog(new()
            {
                TransferJobId = job.Id,
                Message = $@"斯坦德下发任务失败，任务编号为{job.Id},执行F11上生料，从{manualArea}搬运承载编号{job.InternalLotNo}的生料{job.SiloCode}料仓到{job.EndLocationCode}，斯坦德返回信息{job.HkResponse}"
            });
            await _transferPlanManager.TryUpdateTransferJob(job);
            return false;
        }
    }
}
