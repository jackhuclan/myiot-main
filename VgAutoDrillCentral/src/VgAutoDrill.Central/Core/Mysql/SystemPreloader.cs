using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;

namespace VgAutoDrill.Central.Core.Mysql;

/// <summary>
/// 定时取数据库中相关配置
/// </summary>
public class SystemPreloader : BackgroundService
{
    private readonly ISysConfigManager _sysConfigManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly ILocationManager _locationManager;
    private readonly IPartitionManager _partitionManager;
    private readonly ITransferPlanManager _transferPlanManager;
    private readonly IDeviceManager _deviceManager;
    private readonly IDrillManager _drillManager;
    private readonly IAlarmLogManager _alarmLogManager;
    private readonly ILogger<SystemPreloader> _logger;
    private readonly MysqlTaskSchedulerOptions _mysqlTaskSchedulerOptions;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private volatile bool _isBusy = false;

    public SystemPreloader(ILoggerFactory loggerFactory,
        ISysConfigManager sysConfigManager,
        ITransferJobAdapter transportationAdapter,
        IScheduleTaskManager scheduleTaskManager,
        ILocationManager locationManager,
        IPartitionManager partitionManager,
        ITransferPlanManager transferPlanManager,
        IDeviceManager deviceManager,
        IDrillManager drillManager,
        IAlarmLogManager alarmLogManager,
        CentralFlags centralFlags,
        IOptions<MysqlTaskSchedulerOptions> options)
    {
        _sysConfigManager = sysConfigManager;
        _scheduleTaskManager = scheduleTaskManager;
        _locationManager = locationManager;
        _partitionManager = partitionManager;
        _transferPlanManager = transferPlanManager;
        _deviceManager = deviceManager;
        _drillManager = drillManager;
        _alarmLogManager = alarmLogManager;
        _mysqlTaskSchedulerOptions = options.Value ?? new();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_mysqlTaskSchedulerOptions.SystemRefreshInterval));
        _logger = loggerFactory.CreateLogger<SystemPreloader>();
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
                _logger.LogInformation($"begin to SystemPreloader");

                var beforeInitializeSysConfigs = DateTime.Now;
                //读取配置，不需要全部初始化，仅需刷新当前系统模块中使用到的参数
                await _sysConfigManager.InitializeSysConfigs(false);

                _logger.LogInformation($"SystemPreloader finish to _sysConfigManager.InitializeSysConfigs(), used {DateTime.Now.Subtract(beforeInitializeSysConfigs).TotalMilliseconds} TotalMilliseconds.");

                await _partitionManager.Refresh();
                _logger.LogInformation($"_partitionManager.Refresh() load data finished");

                await _locationManager.Refresh();
                _logger.LogInformation($"_locationManager.Refresh() load data finished");

                await _scheduleTaskManager.Evict();
                if (!CentralFlags.SystemPreloadCompleted)
                {
                    await _scheduleTaskManager.Refresh();
                }

                await _deviceManager.RefreshAgvRouteCodes();
                await _deviceManager.RefreshDrillRouteCodes();
                await _drillManager.Refresh();
                await _transferPlanManager.Refresh();
                await _alarmLogManager.Refresh();
                _logger.LogInformation($"SystemPreloader load data finished");
                CentralFlags.SystemPreloadCompleted = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _isBusy = false;
                _autoResetEvent.Set();
                _logger.LogInformation($"End to do SystemPreloader");
            }
        }
    }
}
