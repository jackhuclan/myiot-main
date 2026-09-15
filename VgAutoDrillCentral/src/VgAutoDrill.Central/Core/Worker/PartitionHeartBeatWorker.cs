using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mysql;

namespace VgAutoDrill.Central.Core.Worker;

/// <summary>
/// 分区心跳检测，检测每个分区的中转位是否有合适的转运任务可以下达
/// </summary>
internal class PartitionHeartBeatWorker : BackgroundService
{
    private readonly ILogger<PartitionHeartBeatWorker> _logger;
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly IPartitionManager _partitionManager;

    public PartitionHeartBeatWorker(IPartitionManager partitionManager,
        ILoggerFactory loggerFactory,
        CentralFlags centralFlags,
        IOptions<MysqlTaskSchedulerOptions> options)
    {
        _mysqlTaskSchedulerOptions = options.Value ?? new();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_mysqlTaskSchedulerOptions.DrillLocationHeartBeatSeconds));
        _logger = loggerFactory.CreateLogger<PartitionHeartBeatWorker>();
        _partitionManager = partitionManager;
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
                _logger.LogInformation($"begin to do PartitionHeartBeatWorker ExecuteAsync");

                foreach (var partition in _partitionManager.Partitions.Where(t => t.Status == 1))
                {
                    partition.SiloTransferStrategy.ReassignSiloTransferJob();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogInformation($"End to do PartitionHeartBeatWorker ExecuteAsync");
            }
        }
    }
}
