using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure.Clickhouse;

namespace VgAutoDrill.Central.Core.Handler.Alarm;

internal class DeviceAlarmListener : BackgroundService
{
    private readonly DeviceAlarmListenerOptions _options;
    private readonly PeriodicTimer _timer;
    private readonly ILogger<DeviceAlarmListener> _logger;
    private readonly IClickHouseConnector _clickHouseConnector;
    private readonly IDeviceAlarmDispatcher _deviceAlarmDispatcher;
    private readonly ISysConfigManager _sysConfigManager;
    private AutoResetEvent _resetEvent = new AutoResetEvent(true);
    private List<DeviceAlarmReportRequest> _handledAlarmRequests = new();

    public DeviceAlarmListener(IOptions<DeviceAlarmListenerOptions> options,
        IClickHouseConnector clickHouseConnector,
        IDeviceAlarmDispatcher deviceAlarmDispatcher,
        ISysConfigManager sysConfigManager,
        ILoggerFactory loggerFactory)
    {
        _options = options.Value;
        _logger = loggerFactory.CreateLogger<DeviceAlarmListener>();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(_options.PrefetchInterval));
        _clickHouseConnector = clickHouseConnector;
        _deviceAlarmDispatcher = deviceAlarmDispatcher;
        _sysConfigManager = sysConfigManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (_timer)
        {
            while (_options.Enabled && !stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync())
            {
                if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN, Admin.Model.Enum.SysConfigCategoryEnum.None, false))
                {
                    _logger.LogWarning($"系统即将停机维护，暂停后台任务执行");
                    await Task.Delay(TimeSpan.FromSeconds(_options.PrefetchInterval));
                    continue;
                }

                await PopulateUnhandledAlarmRecord();
            }
        }
    }

    private async Task PopulateUnhandledAlarmRecord()
    {
        try
        {
            _resetEvent.WaitOne();
            _logger.LogDebug($"begin to do PopulateUnhandledAlarmRecord");

            try
            {
                using var command = _clickHouseConnector.Connection.CreateCommand();
                command.CommandText = string.Format(@"select t1.id, t1.device_id, t1.product_id, t1.event_id, t1.trace_id, t1.alarm_code, t1.alarm_level, t1.alarm_json, t2.handled as handled, t1.alarm_time
                                     from device_alarm_trace as t1
                                     LEFT JOIN (
 	                                    select DISTINCT device_id, product_id, event_id, trace_id, alarm_code, alarm_level, handled
 	                                    from device_alarm_trace 
 	                                    where handled > 0
 	                                    and alarm_time > '{0}'
                                     )t2  on t1.product_id = t2.product_id 
                                     and t1.device_id = t2.device_id 
                                     and t1.event_id = t2.event_id 
                                     and t1.trace_id = t2.trace_id 
                                     and t1.alarm_code = t2.alarm_code 
                                     where t1.alarm_level > 1
                                     and t1.alarm_time > '{0}'
                                     AND t2.handled = 0
                                     order by t1.id DESC 
                                     limit {1}",
                                 DateTime.Now.AddDays(-_options.PrefetchDays).ToString("yyyy-MM-dd"),
                                 _options.PrefetchCount);

                _logger.LogDebug($"DeviceAlarmHandler sql:{command.CommandText}");

                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var request = new DeviceAlarmReportRequest()
                    {
                        Id = reader.GetInt64("id"),
                        ProductId = reader.GetFieldValue<string>("product_id"),
                        DeviceId = reader.GetFieldValue<string>("device_id"),
                        EventId = reader.GetFieldValue<string>("event_id"),
                        TraceId = reader.GetFieldValue<string>("trace_id"),
                        AlarmCode = reader.GetFieldValue<string>("alarm_code"),
                        AlarmLevel = reader.GetFieldValue<AlarmLevel>("alarm_level"),
                        Handled = reader.GetFieldValue<int>("handled"),
                        AlarmTime = reader.GetFieldValue<DateTime>("alarm_time"),
                    };

                    if (request.Handled == 1)
                    {
                        _handledAlarmRequests.Add(request);
                        continue;
                    }

                    Func<DeviceAlarmReportRequest, bool> handledFilter = (x) => x.ProductId == request.ProductId
                        && x.DeviceId == request.DeviceId
                        && x.EventId == request.EventId
                        && x.TraceId == request.TraceId
                        && x.AlarmCode == request.AlarmCode
                        && request.Handled == 0
                        && x.Handled > 0;
                    if (_handledAlarmRequests.Count > 0 && _handledAlarmRequests.Any(handledFilter))
                    {
                        continue;
                    }

                    request.Handled = 2;//状态变为正在处理
                    var json = reader.GetFieldValue<string>("alarm_json");
                    request.Params = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>() { };
                    _handledAlarmRequests.Add(request);

                    var policies = _deviceAlarmDispatcher.SelectMatchedPolicies(request);
                    foreach (var policy in policies)
                    {
                        if (policy.ExcuteHandler == null) continue;
                        await policy.ExcuteHandler.HandleDeviceAlarmReportRequest(request);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeviceAlarmHandler error:{ex.Message}");
            }
        }
        finally
        {
            _resetEvent.Set();
            _logger.LogDebug($"End to do background PopulateUnhandledAlarmRecord");
        }
    }
}
