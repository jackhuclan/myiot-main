using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mysql;

namespace VgAutoDrill.Central.Core.Worker;

/// <summary>
/// 检查没有发起新调度的库位
/// </summary>
internal class LoationMissingScheduleChecker : BackgroundService
{
    private readonly ILogger<LoationMissingScheduleChecker> _logger;
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IPartitionManager _partitionManager;
    private readonly IAlarmLogManager _alarmLogManager;

    public LoationMissingScheduleChecker(ITransferPlanManager transferPlanManager,
        IPartitionManager partitionManager,
        IAlarmLogManager alarmLogManager,
        ILoggerFactory loggerFactory,
        CentralFlags centralFlags,
        IOptions<MysqlTaskSchedulerOptions> options)
    {
        _mysqlTaskSchedulerOptions = options.Value ?? new();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_mysqlTaskSchedulerOptions.ConsumingPerSeconds));
        _logger = loggerFactory.CreateLogger<LoationMissingScheduleChecker>();
        _transferPlanManager = transferPlanManager;
        _partitionManager = partitionManager;
        _alarmLogManager = alarmLogManager;
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
                _logger.LogDebug($"begin to do LoationMissingScheduleChecker ExecuteAsync");

                foreach (var partition in _partitionManager.Partitions.Where(t => t.Status == 1))
                {
                    await _transferPlanManager.TryFindAnyNotReadyFork(partition.PartCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do LoationMissingScheduleChecker ExecuteAsync");
            }
        }
    }
}
