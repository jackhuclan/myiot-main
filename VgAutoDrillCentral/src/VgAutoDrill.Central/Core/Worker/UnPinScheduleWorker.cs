using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Schedule.Handler;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Worker;

internal class UnPinScheduleWorker : BackgroundService
{
    private readonly ILogger<UnPinScheduleWorker> _logger;
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly IObjectFactory _objectFactory;
    private readonly IUnpinScheduleHandler _unpinScheduleHandler;

    public UnPinScheduleWorker(IObjectFactory objectFactory,
        ILoggerFactory loggerFactory,
        IUnpinScheduleHandler unpinScheduleHandler,
        CentralFlags centralFlags,
        IOptions<MysqlTaskSchedulerOptions> options)
    {
        _mysqlTaskSchedulerOptions = options.Value ?? new();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_mysqlTaskSchedulerOptions.ConsumingPerSeconds));
        _logger = loggerFactory.CreateLogger<UnPinScheduleWorker>();
        _objectFactory = objectFactory;
        _unpinScheduleHandler = unpinScheduleHandler;
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
                _logger.LogDebug($"begin to do UnPinScheduleWorker ExecuteAsync");

                await _unpinScheduleHandler.Handle();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do UnPinScheduleWorker ExecuteAsync");
            }
        }
    }
}
