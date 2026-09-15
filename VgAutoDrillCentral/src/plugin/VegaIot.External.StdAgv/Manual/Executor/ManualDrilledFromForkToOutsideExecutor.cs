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
/// 手动钻机下熟料（中转位到线边仓)
/// </summary>
internal class ManualDrilledFromForkToOutsideExecutor : ITransferSiloExecutor
{
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly StdAgvConfig _stdAgvConfig;
    private readonly IManualCallAgvTaskService _manualCallAgvLogService;
    private readonly IDistributedCache _distributedCache;

    public ManualDrilledFromForkToOutsideExecutor(IServiceProvider serviceProvider,
        IObjectFactory objectFactory,
        IDistributedCache distributedCache)
    {
        _transferPlanManager = serviceProvider.GetRequiredService<ITransferPlanManager>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
        _manualCallAgvLogService = serviceProvider.GetRequiredService<IManualCallAgvTaskService>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();
        _distributedCache = distributedCache;
    }

    public async Task<Boolean> Execute(TransferJob job)
    {
        var boxBindFoldUrl = await _stdAgvConfig.GetBindPodAndMatUrl();

        var manualCallAgvLog = await _manualCallAgvLogService.QueryByLocationCode(job.StartLocationCode);

        var url = await _stdAgvConfig.GetGenAgvSchedulingTaskUrl();
        var manualArea = await _stdAgvConfig.GetManualClinkerAreaCode();
        var path = new[] { job.StartLocationCode, manualArea };
        // 向斯坦德小车下发任务
        var requestStd = GenAgvSchedulingTaskRequestBuilder.Build(path, job.Code, job.ClinkerCount, "", job.InternalLotNo, StdAgvTaskCode.F14.ToString(), job.SiloCode);
        await _transferPlanManager.AddTransferJobLog(new()
        {
            TransferJobId = job.Id,
            Message = $@"斯坦德下发任: url: {url},请求参数：{requestStd.ToJson()}"
        });
        var response = await _httpRequestInvoker?.PostAsJsonAsync<STDGenAgvSchedulingTaskRequestEntity?, STDResponse>(url, requestStd);
        job.HkResponse = response.ToJson();
        if (response != null && response.code == 0)
        {
            manualCallAgvLog.TaskStatus = VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv.ManualCallAgvTaskStatus.Distribute;
            manualCallAgvLog.TaskStatusDescription = $@"斯坦德下发任务成功，任务编号为{job.Id},执行F14下熟料任务，从{job.StartLocationCode}搬运{job.ClinkerCount}数量的{job.InternalLotNo}熟料到{manualArea}，斯坦德返回信息{job.HkResponse}";
            await _manualCallAgvLogService.Update(manualCallAgvLog);

            await _transferPlanManager.AddTransferJobLog(new()
            {
                TransferJobId = job.Id,
                Message = $@"斯坦德下发任务成功，任务编号为{job.Id},执行F14下熟料任务，从{job.StartLocationCode}搬运{job.ClinkerCount}数量的{job.InternalLotNo}熟料到{manualArea},斯坦德返回信息{job.HkResponse}"
            });

            await _distributedCache.SetStringAsync(job.StartLocationCode, "AGV搬运中");
            job.ScheduledTaskStatus = ScheduledTaskStatus.Running;
            await _transferPlanManager.TryUpdateTransferJob(job);
            return true;
        }
        else
        {
            job.ScheduledTaskStatus = ScheduledTaskStatus.Failed;
            manualCallAgvLog.TaskStatus = ManualCallAgvTaskStatus.Exception;
            manualCallAgvLog.TaskStatusDescription = $@"斯坦德下发任务失败，任务编号为{job.Id},执行F14下熟料任务，从{job.StartLocationCode}搬运{job.ClinkerCount}数量的{job.InternalLotNo}熟料到{manualArea},斯坦德返回信息{job.HkResponse}";
            await _manualCallAgvLogService.Update(manualCallAgvLog);

            await _transferPlanManager.AddTransferJobLog(new()
            {
                TransferJobId = job.Id,
                Message = $@"斯坦德下发任务失败，任务编号为{job.Id},执行F14下熟料任务，从{job.StartLocationCode}搬运{job.ClinkerCount}数量的{job.InternalLotNo}熟料到{manualArea},斯坦德返回信息{job.HkResponse}"
            });
            await _distributedCache.SetStringAsync(job.StartLocationCode, $"斯坦德任务下发失败:{response.message.ToJson()}");
            await _transferPlanManager.TryUpdateTransferJob(job);
            return false;
        }
    }
}
