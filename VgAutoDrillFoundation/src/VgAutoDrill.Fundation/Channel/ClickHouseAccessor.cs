using System.Text.Encodings.Web;
using System.Text.Json;
using ClickHouse.Client.ADO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Fundation.Channel;

/// <inheritdoc/>
internal class ClickHouseAccessor
{
    private readonly ClickHouseConnection _connection;
    private readonly ILogger<ClickHouseAccessor> _logger;
    private readonly ClickHouseOptions _clickHouseOptions;

    /// <summary>
    /// constuct ClickHouseAccessor
    /// </summary>
    /// <param name="options"></param>
    /// <param name="loggerFactory"></param>
    public ClickHouseAccessor(IOptions<ClickHouseOptions> options,
        ILoggerFactory loggerFactory)
    {
        _clickHouseOptions = options.Value;
        _logger = loggerFactory.CreateLogger<ClickHouseAccessor>();

        var builder = new ClickHouseConnectionStringBuilder(_clickHouseOptions.ConnectionString);
        builder.Compression = _clickHouseOptions.Compression;
        builder.UseSession = _clickHouseOptions.Session;
        builder.UseCustomDecimals = _clickHouseOptions.CustomDecimals;
        _connection = new ClickHouseConnection(builder.ConnectionString);

        _logger.LogDebug($"this.ClickHouseConnection :{_connection}");
        _logger.LogDebug($"ClickHouseOptions.Enabled:{_clickHouseOptions.Enabled}" +
            $",PropertiesReportEnabled:{_clickHouseOptions.PropertiesReportEnabled}" +
            $",StatusReportEnabled:{_clickHouseOptions.StatusReportEnabled}" +
            $",ServiceReportEnabled:{_clickHouseOptions.ServiceReportEnabled}" +
            $",EventReportEnabled:{_clickHouseOptions.EventReportEnabled}" +
            $",AlarmReportEnabled:{_clickHouseOptions.AlarmReportEnabled}");
    }

    /// <inheritdoc/>
    public Task DeviceEventReport(DeviceEventReportRequest request, DeviceEventReportResponse response)
    {
        return Task.Run(() =>
        {
            if (_clickHouseOptions.Enabled && _clickHouseOptions.EventReportEnabled)
            {
                var requestContent = JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                var responseContent = JsonSerializer.Serialize(response, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                try
                {
                    using var command = _connection.CreateCommand();
                    command.CommandText = $"insert into device_event_trace(device_id,product_id,event_id,event_name,request_json,response_json)"
                        + $" values('{request.DeviceId}','{request.ProductId}','{request.EventId}','{request.EventName}','{requestContent}','{responseContent}')";
                    _logger.LogDebug($"HandleDeviceEventReport sql:{command.CommandText}");

                    command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"HandleDeviceEventReport error:{ex.Message}");
                }
            }
        });
    }

    /// <inheritdoc/>
    public Task DevicePropertiesReport(DevicePropertiesReportRequest request, DevicePropertiesReportResponse response)
    {
        return Task.Run(() =>
        {
            if (_clickHouseOptions.Enabled && _clickHouseOptions.PropertiesReportEnabled)
            {
                try
                {
                    var propertyContent = JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    using var command = _connection.CreateCommand();
                    command.CommandText = $"insert into device_property_trace(device_id,product_id,properpty_json) values('{request.DeviceId}','{request.ProductId}','{propertyContent}')";
                    _logger.LogDebug($"DevicePropertiesReport sql:{command.CommandText}");
                    command.ExecuteScalar();

                }
                catch (Exception ex)
                {
                    _logger.LogError($"DevicePropertiesReport  {request.DeviceId} error:{ex.Message}");
                }
            }
        });
    }

    /// <inheritdoc/>
    public Task DeviceServiceReport(DeviceServiceInvokeRequest request, DeviceServiceInvokeResponse response)
    {
        return Task.Run(() =>
        {
            if (_clickHouseOptions.Enabled && _clickHouseOptions.ServiceReportEnabled)
            {
                var requestContent = JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                var responseContent = JsonSerializer.Serialize(response, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

                try
                {
                    using var command = _connection.CreateCommand();
                    command.CommandText = "insert into device_service_trace(device_id,product_id,service_id,event_id,event_name,target_product_id,target_device_id,request_json,response_json) " +
                        $"values('{request.DeviceId}','{request.ProductId}','{request.ServiceId}','{request.EventId}','{request.EventName}','{request.TargetProductId}','{request.TargetDeviceId}','{requestContent}','{responseContent}')";
                    _logger.LogDebug($"DeviceServiceInvokeRequest sql:{command.CommandText}");

                    command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"DeviceServiceReport error:{ex.Message}");
                }
            }
        });
    }

    /// <inheritdoc/>
    public Task DeviceStatusReport(DeviceStatusReportRequest request, DeviceStatusReportResponse response)
    {
        return Task.Run(() =>
        {
            if (_clickHouseOptions.Enabled && _clickHouseOptions.StatusReportEnabled)
            {
                try
                {
                    using var command = _connection.CreateCommand();
                    command.CommandText = "insert into device_status_trace(device_id,product_id,old_status,new_status)"
                            + $" values('{request.DeviceId}','{request.ProductId}','{request.OldStatus}','{request.NewStatus}')";
                    _logger.LogDebug($"HandleDeviceStatusReport sql:{command.CommandText}");

                    command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"DeviceStatusReport error:{ex.Message}");
                }
            }

        });
    }

    /// <inheritdoc/>
    public Task DeviceAlarmReport(DeviceAlarmReportRequest request, DeviceAlarmReportResponse response)
    {
        return Task.Run(() =>
        {
            if (_clickHouseOptions.Enabled && _clickHouseOptions.AlarmReportEnabled)
            {
                try
                {
                    var alarmContent = JsonSerializer.Serialize(request, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                    using var command = _connection.CreateCommand();
                    command.CommandText = $"insert into device_alarm_trace(id,device_id,product_id,event_id,trace_id,alarm_code,alarm_level,handled,alarm_json,alarm_time) " +
                                $"values({SnowflakeIdGenerator.nextId()},'{request.DeviceId}','{request.ProductId}','{request.EventId}','{request.TraceId}','{request.AlarmCode}',{(int)request.AlarmLevel},{request.Handled},'{alarmContent}','{request.AlarmTime.ToString("yyyy-MM-dd HH:mm:ss")}')";

                    _logger.LogDebug($"DeviceAlarmReport sql:{command.CommandText}");

                    command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"DeviceAlarmReport  {request.DeviceId} error:{ex.Message}");
                }
            }
        });
    }
}
