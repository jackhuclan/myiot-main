using System.Text.Json;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.Infrastructure.Clickhouse;

namespace VgAutoDrill.Central.Core.Handler.Alarm;

internal abstract class AbstractAlarmHandler : ICommonAlarmHandler
{
    private readonly IClickHouseConnector _clickHouseConnector;
    private readonly ILogger<AbstractAlarmHandler> _logger;

    public AbstractAlarmHandler(IClickHouseConnector clickHouseConnector,
        ILoggerFactory loggerFactory)
    {
        _clickHouseConnector = clickHouseConnector;
        _logger = loggerFactory.CreateLogger<AbstractAlarmHandler>();
    }

    ///<inheritdoc/>
    public virtual async Task HandleDeviceAlarmReportRequest(DeviceAlarmReportRequest alarmRequest)
    {
        await SaveHandledDeviceAlarmReportRequest(alarmRequest);
    }

    ///<inheritdoc/>
    public virtual Task SaveHandledDeviceAlarmReportRequest(DeviceAlarmReportRequest handledRequest)
    {
        return Task.Run(() =>
        {
            try
            {
                handledRequest.Handled = 1;
                var alarmContent = JsonSerializer.Serialize(handledRequest);
                using var command = _clickHouseConnector.Connection.CreateCommand();
                command.CommandText = $"insert into device_alarm_trace(id,device_id,product_id,event_id,trace_id,alarm_code,alarm_level,handled,alarm_json,alarm_time) " +
                $"values({SnowflakeIdGenerator.nextId()},'{handledRequest.DeviceId}','{handledRequest.ProductId}','{handledRequest.EventId}','{handledRequest.TraceId}','{handledRequest.AlarmCode}',{(int)handledRequest.AlarmLevel},{handledRequest.Handled},'{alarmContent}','{handledRequest.AlarmTime.ToString("yyyy-MM-dd HH:mm:ss")}')";

                _logger.LogDebug($"DeviceAlarmReport sql:{command.CommandText}");
                command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeviceAlarmReport  {handledRequest.DeviceId} error:{ex.Message}");
            }
        });
    }
}
