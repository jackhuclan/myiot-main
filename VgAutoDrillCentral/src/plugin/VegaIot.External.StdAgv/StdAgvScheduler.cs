using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VegaIot.External.StdAgv.Executor.ForkOutside;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.StdAgv;

public class StdAgvScheduler : BackgroundService
{
    private StdAgvSchedulerOptions _stdAgvSchedulerOptions;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ILogger<StdAgvScheduler> _logger;
    private readonly ITransferPlanManager _planManager;
    private readonly IObjectFactory _objectFactory;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly ISysConfigManager _sysConfigManager;
    public StdAgvScheduler(ITransferPlanManager planManager,
        IObjectFactory objectFactory,
        ILoggerFactory loggerFactory,
        IHttpRequestInvoker httpRequestInvoker,
        ISysConfigManager sysConfigManager,
        IOptions<StdAgvSchedulerOptions> options)
    {
        _httpRequestInvoker = httpRequestInvoker;
        _stdAgvSchedulerOptions = options.Value;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_stdAgvSchedulerOptions.CallInterval));
        _logger = loggerFactory.CreateLogger<StdAgvScheduler>();
        _planManager = planManager;
        _objectFactory = objectFactory;
        _sysConfigManager = sysConfigManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        VgAutoDrill.Central.Core.Domain.TransferJob logJob = new VgAutoDrill.Central.Core.Domain.TransferJob();

        while (!stoppingToken.IsCancellationRequested
            && await _timer.WaitForNextTickAsync())
        {
            try
            {
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
                {
                    _logger.LogWarning($"系统维护，暂停料仓转运的任务，请稍候.");
                    continue;
                }

                if (!CentralFlags.SystemPreloadCompleted)
                    continue;

                _autoResetEvent.WaitOne();
                _logger.LogInformation($"StdAgvScheduler:begin to do StdAgvScheduler ExecuteAsync,Time:{DateTime.Now}");


                foreach (var job in _planManager.TransferJobs.ToList().Where(x => x.ScheduledTaskStatus == ScheduledTaskStatus.Created && !x.PartitionCode.Equals("Manual")))
                {
                    logJob = job;

                    switch (job.TransferBehavior)
                    {
                        //熟料:中转位->（外部）熟料区
                        case SiloTransferBehavior.DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var drilledForkOutsideExecutor = _objectFactory.GetOrCreate<DrilledFromForkToOutsideExecutor>();
                                var ret6 = await drilledForkOutsideExecutor.Execute(job);
                                _logger.LogInformation(string.Format("DrilledFromForkToOutsideExecutor: send job={0} to Std {1}", job.Id, ret6 ? "Success" : "Failed"));
                            }
                            break;

                        //空料仓：中转位->（外部）外部线边仓
                        case SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_OUTSIDE:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var emptyBoxForkOutsideExecutor = _objectFactory.GetOrCreate<EmptyBoxFromForkToOutsideExecutor>();
                                var ret8 = await emptyBoxForkOutsideExecutor.Execute(job);
                                _logger.LogInformation(string.Format("EmptyBoxFromForkToOutsideExecutor: send job={0} to Std {1}", job.Id, ret8 ? "Success" : "Failed"));
                            }
                            break;

                        //空料仓：（外部）外部线边仓->中转位
                        case SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_FORK:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var emptyBoxOutsideForkExecutor = _objectFactory.GetOrCreate<EmptyBoxFromOutsideToForkExecutor>();
                                var ret9 = await emptyBoxOutsideForkExecutor.Execute(job);
                                _logger.LogInformation(string.Format("EmptyBoxFromOutsideToForkExecutor: send job={0} to Std {1}", job.Id, ret9 ? "Success" : "Failed"));
                            }
                            break;

                        //生料：（外部）外部线边仓->中转位
                        case SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_FORK:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var unDrilledOutsideForkExecutor = _objectFactory.GetOrCreate<UndrilledFromOutsideToForkExecutor>();
                                var ret10 = await unDrilledOutsideForkExecutor.Execute(job);
                                _logger.LogInformation(string.Format("UndrilledFromOutsideToForkExecutor: send job={0} to Std {1}", job.Id, ret10 ? "Success" : "Failed"));
                            }
                            break;

                        //生料：（外部）中转位->外部线边仓
                        case SiloTransferBehavior.UNDRILLED_FROM_FORK_TO_OUTSIDE:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var unDrilledOutsideForkExecutor = _objectFactory.GetOrCreate<UndrilledFromForkToOutsideExecutor>();
                                var ret11 = await unDrilledOutsideForkExecutor.Execute(job);
                                _logger.LogInformation(string.Format("UndrilledFromOutsideToForkExecutor: send job={0} to Std {1}", job.Id, ret11 ? "Success" : "Failed"));
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await _planManager.AddTransferJobLog(new AddTranserJobLogRequest
                {
                    TransferJobId = logJob!.Id,
                    Message = $"料仓转运任务异常：\r\n {ex.ToString()}"
                });
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do StdAgvScheduler ExecuteAsync");
            }
        }
    }
}
