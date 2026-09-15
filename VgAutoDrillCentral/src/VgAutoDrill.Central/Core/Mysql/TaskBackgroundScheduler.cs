using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Schedule;

namespace VgAutoDrill.Central.Core.Mysql;

/// <summary>
/// 基于mysql 分发任务
/// </summary>
public class TaskBackgroundScheduler : BackgroundService
{
    private readonly ITaskScheduleStrategy _scheduleStrategy;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILogger<TaskBackgroundScheduler> _logger;
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private volatile bool _isBusy = false;

    public TaskBackgroundScheduler(ITaskScheduleStrategy scheduleStrategy,
        ILoggerFactory loggerFactory,
        IScheduleTaskManager scheduleTaskManager,
        IAlarmLogManager alarmLogManager,
        CentralFlags centralFlags,
        IOptions<MysqlTaskSchedulerOptions> options)
    {
        _scheduleStrategy = scheduleStrategy;
        _scheduleTaskManager = scheduleTaskManager;
        _mysqlTaskSchedulerOptions = options.Value ?? new();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_mysqlTaskSchedulerOptions.ConsumingPerSeconds));
        _logger = loggerFactory.CreateLogger<TaskBackgroundScheduler>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested
            && !_isBusy
            && await _timer.WaitForNextTickAsync())
        {
            try
            {
                _autoResetEvent.WaitOne();
                _isBusy = true;
                _logger.LogInformation($"begin to TaskBackgroundScheduler");

                if (!CentralFlags.SystemPreloadCompleted)
                    continue;

                //await _scheduleStrategy.RefreshDevicesStatus();
                //await _scheduleStrategy.RefreshTimeoutRedisKey();
                await _scheduleTaskManager.RefreshTimeoutSchedule();
                await _scheduleStrategy.HandleScheduleTask();
                await _scheduleStrategy.SetIdleAgvToRestPoint();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _isBusy = false;
                _logger.LogInformation($"End to do TaskBackgroundScheduler ExecuteAsync");
                _autoResetEvent.Set();
            }
        }
    }
}
