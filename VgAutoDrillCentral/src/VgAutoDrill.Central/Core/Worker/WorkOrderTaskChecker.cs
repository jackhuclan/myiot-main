using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mysql;
using VgAutoDrill.Central.Core.Reporter;

namespace VgAutoDrill.Central.Core.Worker;

/// <summary>
/// 检查没有任务的钻机，插入告警信息到t_alarm表
/// </summary>
internal class WorkOrderTaskChecker : BackgroundService
{
    private readonly ILogger<WorkOrderTaskChecker> _logger;
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly IWorkOrderTaskAdapter _workOrderTaskAdapter;
    private readonly IDeviceAlarmReporter _alarmReporter;

    public WorkOrderTaskChecker(IWorkOrderTaskAdapter workOrderTaskAdapter,
        IDeviceAlarmReporter alarmReporter,
        ILoggerFactory loggerFactory,
        CentralFlags centralFlags,
        IOptions<MysqlTaskSchedulerOptions> options)
    {
        _mysqlTaskSchedulerOptions = options.Value ?? new();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_mysqlTaskSchedulerOptions.ConsumingPerSeconds));
        _logger = loggerFactory.CreateLogger<WorkOrderTaskChecker>();
        _workOrderTaskAdapter = workOrderTaskAdapter;
        _alarmReporter = alarmReporter;
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
                _logger.LogDebug($"begin to do WorkOrderTaskChecker ExecuteAsync");

                /* todo: _workOrderTaskAdapter查询数据库，如果告警表中不存在该钻机的未处理的生产任务，则插入一条告警记录
                await _alarmReporter.Report(new DeviceAlarmReportRequest
                {
                    AlarmKind = AlarmKind.DrillMissingWorkOrderTask,
                    //补充余下信息
                });

                TODO,设置钻机属性，记录没有生产任务的开始时间点， 如果有了任务 再取消 无任务时间点；
                */
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do WorkOrderTaskChecker ExecuteAsync");
            }
        }
    }
}
