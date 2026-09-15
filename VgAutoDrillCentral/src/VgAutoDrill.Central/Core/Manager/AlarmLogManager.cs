using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Central.Core.Mes.Adapter;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Manager;

public class AlarmLogManager : BackgroundService, IAlarmLogManager
{
    private readonly ILogger<AlarmLogManager> _logger;
    private readonly AlarmLogPersistOptions _persistOptions;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    private readonly PeriodicTimer _timer;
    private readonly IAlarmAdapter _alarmAdapter;
    private List<AlarmLog> _alarms = new List<AlarmLog>();
    private volatile bool _isBusy = false;

    public AlarmLogManager(IServiceProvider serviceProvider)
    {
        _persistOptions = serviceProvider.GetRequiredService<IOptions<AlarmLogPersistOptions>>().Value;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_persistOptions.FlushTimeout));
        _alarmAdapter = serviceProvider.GetRequiredService<IAlarmAdapter>();

        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<AlarmLogManager>();
    }

    public async Task Refresh()
    {
        var unhandledLogs = await _alarmAdapter.LoadUnhandledAlarmLogs();

        if (unhandledLogs.Any())
        {
            var missedAlarms = unhandledLogs.Where(x => !_alarms.Any(y => y.SyncId == x.SyncId)).ToList();
            missedAlarms.ForEach(x => x.IsSynced = true);

            _alarms.AddRange(missedAlarms);
        }
    }

    public async Task AddLogAsync(AlarmLog log)
    {
        if (!CentralFlags.SystemPreloadCompleted)
        {
            _logger.LogWarning($"系统已启用预加载模式，但是尚未加载完成，请等待.");
            return;
        }

        var old = _alarms.FirstOrDefault(a => !a.IsHandled && log.AlarmCode.ToLower() == a.AlarmCode.ToLower());
        if (old != null && old.AlarmName != log.AlarmName)
        {
            await _alarmAdapter.TryHandle(old.SyncId);
            old.IsHandled = true;
        }

        if (_alarms.Any(a => !a.IsHandled && log.AlarmCode.ToLower() == a.AlarmCode.ToLower()))
        {
            return;
        }

        _alarms.Add(log);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested
            && await _timer.WaitForNextTickAsync())
        {
            try
            {
                _autoResetEvent.WaitOne();
                _isBusy = true;

                await SyncFromDb();

                _logger.LogDebug($"begin to do AlarmLogManager ExecuteAsync");
                var batchList = _alarms.Where(x => !x.IsSynced).OrderBy(x => x.SyncId).Take(_persistOptions.BatchSize).ToList();
                if (batchList.Any())
                {
                    await _alarmAdapter.PersisitAlarmLog(batchList);
                    batchList.ForEach(x => x.IsSynced = true);
                }

                var handledAlarms = _alarms.Where(x => x.IsHandled && x.HandleTime < DateTime.Now.AddMinutes(-1 * 60)).ToList();
                _alarms.RemoveAll(x => handledAlarms.Contains(x));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            finally
            {
                _isBusy = false;
                _autoResetEvent.Set();
                _logger.LogDebug($"End to do AlarmLogManager ExecuteAsync");
            }
        }
    }

    private async Task SyncFromDb()
    {
        foreach (var alarm in _alarms.Where(x => !x.IsHandled))
        {
            var db = await _alarmAdapter.GetAlarm(alarm.SyncId);
            if (db != null && db.IsHandled)
            {
                alarm.IsHandled = true;
                alarm.HandleTime = db.HandleTime;
            }
        }
    }

    public async Task<bool> TryResetAlarm(string code)
    {
        var alarm = _alarms.FirstOrDefault(x => !x.IsHandled && x.AlarmCode == code);
        if (alarm != null)
        {
            return await _alarmAdapter.TryHandle(alarm.SyncId);
        }

        return false;
    }
    public async Task<bool> ResetAlarm(string code)
    {
        var alarmCodes = _alarms.Where(x => !x.IsHandled && x.AlarmCode == code).Select(x => x.AlarmCode).ToList();
        if (alarmCodes != null && alarmCodes.Any())
        {
            return await _alarmAdapter.SetHanled(alarmCodes);
        }

        return false;
    }
}
