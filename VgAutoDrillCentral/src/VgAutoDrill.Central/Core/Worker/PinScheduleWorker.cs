using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Schedule.Handler;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Worker;

internal class PinScheduleWorker : BackgroundService
{
    private readonly ILogger<PinScheduleWorker> _logger;
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly IObjectFactory _objectFactory;
    private readonly IPinScheduleHandler _pinScheduleHandler;

    public PinScheduleWorker(IObjectFactory objectFactory,
        ILoggerFactory loggerFactory,
        IPinScheduleHandler pinScheduleHandler,
        CentralFlags centralFlags,
        IOptions<MysqlTaskSchedulerOptions> options)
    {
        _mysqlTaskSchedulerOptions = options.Value ?? new();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_mysqlTaskSchedulerOptions.ConsumingPerSeconds));
        _logger = loggerFactory.CreateLogger<PinScheduleWorker>();
        _objectFactory = objectFactory;
        _pinScheduleHandler = pinScheduleHandler;
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
                _logger.LogDebug($"begin to do PinScheduleWorker ExecuteAsync");

                await _pinScheduleHandler.Handle();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do PinScheduleWorker ExecuteAsync");
            }
        }
    }
}
