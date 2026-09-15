using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VegaIot.External.StdAgv.Manual.Executor;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.StdAgv;

public class StdManualAgvScheduler : BackgroundService
{
    private StdAgvSchedulerOptions _stdAgvSchedulerOptions;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ILogger<StdManualAgvScheduler> _logger;
    private readonly ITransferPlanManager _planManager;
    private readonly IObjectFactory _objectFactory;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    public StdManualAgvScheduler(ITransferPlanManager planManager,
        IObjectFactory objectFactory,
        ILoggerFactory loggerFactory,
        IHttpRequestInvoker httpRequestInvoker,
        ISysConfigManager sysConfigManager,
        IOptions<StdAgvSchedulerOptions> options)
    {
        _httpRequestInvoker = httpRequestInvoker;
        _stdAgvSchedulerOptions = options.Value;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_stdAgvSchedulerOptions.CallInterval));
        _logger = loggerFactory.CreateLogger<StdManualAgvScheduler>();
        _planManager = planManager;
        _objectFactory = objectFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested
            && await _timer.WaitForNextTickAsync())
        {
            long taskId = 0;
            try
            {
                _autoResetEvent.WaitOne();
                _logger.LogInformation($"StdManualAgvScheduler:begin to  ExecuteAsync,Time:{DateTime.Now}");

                foreach (var job in _planManager.TransferJobs.ToList().Where(x => x.ScheduledTaskStatus == ScheduledTaskStatus.Created && x.PartitionCode.Equals("Manual")))
                {
                    taskId = job.Id;
                    switch (job.TransferBehavior)
                    {
                        //熟料:中转位->（外部）熟料区
                        case SiloTransferBehavior.DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var drilledForkOutsideExecutor = _objectFactory.GetOrCreate<ManualDrilledFromForkToOutsideExecutor>();
                                var ret6 = await drilledForkOutsideExecutor.Execute(job);
                                _logger.LogInformation(string.Format("ManualDrilledFromForkToOutsideExecutor: send job={0} to Std {1}", job.Id, ret6 ? "Success" : "Failed"));
                            }
                            break;

                        //空料仓：中转位->（外部）空料仓区
                        case SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_OUTSIDE:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var emptyBoxForkOutsideExecutor = _objectFactory.GetOrCreate<ManualEmptyBoxFromForkToOutsideExecutor>();
                                var ret8 = await emptyBoxForkOutsideExecutor.Execute(job);
                                _logger.LogInformation(string.Format("ManualEmptyBoxFromForkToOutsideExecutor: send job={0} to Std {1}", job.Id, ret8 ? "Success" : "Failed"));
                            }
                            break;

                        //空料仓：（外部）空料仓区->中转位
                        case SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_FORK:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var emptyBoxOutsideForkExecutor = _objectFactory.GetOrCreate<ManualEmptyBoxFromOutsideToForkExecutor>();
                                var ret9 = await emptyBoxOutsideForkExecutor.Execute(job);
                                _logger.LogInformation(string.Format("ManualEmptyBoxFromOutsideToForkExecutor: send job={0} to Std {1}", job.Id, ret9 ? "Success" : "Failed"));
                            }
                            break;

                        //生料：（外部）生料区->中转位
                        case SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_FORK:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var unDrilledOutsideForkExecutor = _objectFactory.GetOrCreate<ManualUndrilledFromOutsideToForkExecutor>();
                                var ret10 = await unDrilledOutsideForkExecutor.Execute(job);
                                _logger.LogInformation(string.Format("ManualUndrilledFromOutsideToForkExecutor: send job={0} to Std {1}", job.Id, ret10 ? "Success" : "Failed"));
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _planManager.AddTransferJobLog(new()
                {
                    TransferJobId = taskId,
                    Message = $@"斯坦德下发任务失败，异常信息:{ex.ToString()}"
                });
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do StdManualAgvScheduler ExecuteAsync, Time:{DateTime.Now}");
            }
        }
    }
}
