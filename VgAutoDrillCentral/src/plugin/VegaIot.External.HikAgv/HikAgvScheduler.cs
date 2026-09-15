using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VegaIot.External.HikAgv.Executor.ForkOutside;
using VegaIot.External.HikAgv.Executor.None;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Infrastructure;

namespace VegaIot.External.HikAgv;

public class HikAgvScheduler : BackgroundService
{
    private HikAgvSchedulerOptions _hikAgvSchedulerOptions;
    private readonly ILogger<HikAgvScheduler> _logger;
    private readonly ITransferPlanManager _planManager;
    private readonly IObjectFactory _objectFactory;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;

    public HikAgvScheduler(ITransferPlanManager planManager,
        IObjectFactory objectFactory,
        ILoggerFactory loggerFactory,
        IOptions<HikAgvSchedulerOptions> options)
    {
        _hikAgvSchedulerOptions = options.Value;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_hikAgvSchedulerOptions.CallInterval));
        _logger = loggerFactory.CreateLogger<HikAgvScheduler>();
        _planManager = planManager;
        _objectFactory = objectFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested
            && await _timer.WaitForNextTickAsync())
        {
            try
            {
                if (!CentralFlags.SystemPreloadCompleted)
                    continue;

                _autoResetEvent.WaitOne();
                _logger.LogInformation($"HikAgvScheduler:begin to do HikAgvScheduler ExecuteAsync,Time:{DateTime.Now}");

                var jobList = _planManager.TransferJobs.ToList().Where(x => x.ScheduledTaskStatus == ScheduledTaskStatus.Created);
                _logger.LogInformation($"HikAgvScheduler:jobList count{jobList.Count()}.{DateTime.Now}");

                foreach (var job in jobList)
                {
                    switch (job.TransferBehavior)
                    {
                        //上、下PIN <--->中转位(适用:崇达)
                        case SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_PIN://空料仓：中转位->上PIN
                        case SiloTransferBehavior.UNDRILLED_FROM_PIN_TO_FORK://生料：上PIN->中转位
                        case SiloTransferBehavior.DRILLED_FROM_FORK_TO_UNPIN://熟料:中转位->下PIN
                        case SiloTransferBehavior.EMPTY_BOX_FROM_UNPIN_TO_FORK://空料仓:下PIN->中转位
                        //上、下PIN <--->线边仓(适用:博敏)
                        case SiloTransferBehavior.UNDRILLED_FROM_PIN_TO_WIP://生料:上PIN->线边仓
                        case SiloTransferBehavior.EMPTY_BOX_FROM_WIP_TO_PIN://空料仓:线边仓->上PIN
                        case SiloTransferBehavior.EMPTY_BOX_FROM_UNPIN_TO_WIP://空料仓:下PIN->线边仓
                        case SiloTransferBehavior.DRILLED_FROM_WIP_TO_UNPIN://熟料:线边仓->下PIN
                        //中转位<--->线边仓(适用:博敏)
                        case SiloTransferBehavior.DRILLED_FROM_FORK_TO_WIP://熟料:中转位->线边仓
                        case SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_WIP://空料仓:中转位->线边仓
                        case SiloTransferBehavior.EMPTY_BOX_FROM_WIP_TO_FORK://空料仓:线边仓->中转位
                        case SiloTransferBehavior.UNDRILLED_FROM_FORK_TO_WIP://生料:中转位->线边仓
                        case SiloTransferBehavior.UNDRILLED_FROM_WIP_TO_FORK://生料:线边仓->中转位
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey)
                                && !string.IsNullOrEmpty(job.StartLocationCode)
                                && !string.IsNullOrEmpty(job.EndLocationCode))
                            {
                                var trackExecutor = _objectFactory.GetOrCreate<AutoTrackSiloExcutor>();
                                var ret = await trackExecutor.Execute(job);
                                _logger.LogInformation(string.Format("AutoTrackSiloExcutor: send job={0} to hik {1}", job.Id, ret ? "Success" : "Failed"));
                            }
                            else if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                 && string.IsNullOrEmpty(job.HikResponseKey)
                                 || string.IsNullOrEmpty(job.StartLocationCode)
                                 || string.IsNullOrEmpty(job.EndLocationCode))
                            {
                                //var trackExecutor = _objectFactory.GetOrCreate<ManualTrackSiloExcutor>();
                                //var ret = await trackExecutor.Execute(job);
                                //_logger.LogInformation(string.Format("ManualTrackSiloExcutor: send job={0} to hik {1}", job.Id, ret ? "Success" : "Failed"));
                            }

                            break;

                        //熟料:中转位->（外部）熟料区
                        case SiloTransferBehavior.DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var drilledForkOutsideExecutor = _objectFactory.GetOrCreate<DrilledFromForkToOutsideExecutor>();
                                var ret = await drilledForkOutsideExecutor.Execute(job);
                                _logger.LogInformation(string.Format("DrilledFromForkToOutsideExecutor: send job={0} to hik {1}", job.Id, ret ? "Success" : "Failed"));
                            }
                            break;

                        //首件:中转位->（外部）检验区
                        case SiloTransferBehavior.FIRST_DRILLED_FROM_FORK_TO_OUTSIDE_UNPIN:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var firstDrilledForkOutsideExecutor = _objectFactory.GetOrCreate<FirstDrilledFromForkToOutsideExecutor>();
                                var ret = await firstDrilledForkOutsideExecutor.Execute(job);
                                _logger.LogInformation(string.Format("FirstDrilledFromForkToOutsideExecutor: send job={0} to hik {1}", job.Id, ret ? "Success" : "Failed"));
                            }
                            break;

                        //空料仓：中转位->（外部）空料仓区
                        case SiloTransferBehavior.EMPTY_BOX_FROM_FORK_TO_OUTSIDE:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var emptyBoxForkOutsideExecutor = _objectFactory.GetOrCreate<EmptyBoxFromForkToOutsideExecutor>();
                                var ret = await emptyBoxForkOutsideExecutor.Execute(job);
                                _logger.LogInformation(string.Format("EmptyBoxFromForkToOutsideExecutor: send job={0} to hik {1}", job.Id, ret ? "Success" : "Failed"));
                            }
                            break;

                        //空料仓：（外部）空料仓区->中转位
                        case SiloTransferBehavior.EMPTY_BOX_FROM_OUTSIDE_TO_FORK:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var emptyBoxOutsideForkExecutor = _objectFactory.GetOrCreate<EmptyBoxFromOutsideToForkExecutor>();
                                var ret = await emptyBoxOutsideForkExecutor.Execute(job);
                                _logger.LogInformation(string.Format("EmptyBoxFromOutsideToForkExecutor: send job={0} to hik {1}", job.Id, ret ? "Success" : "Failed"));
                            }
                            break;

                        //熟料：（外部）熟料区->中转位
                        case SiloTransferBehavior.UNDRILLED_FROM_OUTSIDE_TO_FORK:
                            if (job.ScheduledTaskStatus == ScheduledTaskStatus.Created
                                && string.IsNullOrEmpty(job.HikResponseKey))
                            {
                                var unDrilledOutsideForkExecutor = _objectFactory.GetOrCreate<UndrilledFromOutsideToForkExecutor>();
                                var ret = await unDrilledOutsideForkExecutor.Execute(job);
                                _logger.LogInformation(string.Format("UndrilledFromOutsideToForkExecutor: send job={0} to hik {1}", job.Id, ret ? "Success" : "Failed"));
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do HikAgvScheduler ExecuteAsync");
            }
        }
    }
}
