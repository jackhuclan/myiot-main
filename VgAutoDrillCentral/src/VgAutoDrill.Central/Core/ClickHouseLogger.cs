using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Infrastructure.Clickhouse;

namespace VgAutoDrill.Central.Core;

public class ClickHouseLogger : IClickHouseLogger
{
    private readonly IClickHouseConnector _clickHouseConnector;
    private readonly ISysConfigManager _sysConfigManager;
    private readonly ILogger<ClickHouseLogger> _logger;

    public ClickHouseLogger(IClickHouseConnector clickHouseConnector,
        ILoggerFactory loggerFactory,
        ISysConfigManager sysConfigManager)
    {
        _clickHouseConnector = clickHouseConnector;
        _sysConfigManager = sysConfigManager;
        _logger = loggerFactory.CreateLogger<ClickHouseLogger>();
    }

    public void LogWarning(string? message, params object?[] args)
    {
        Log(LogLevel.Warning, message, args);
    }

    public void LogError(string? message, params object?[] args)
    {
        Log(LogLevel.Error, message, args);
    }

    public void LogDebug(string? message, params object?[] args)
    {
        Log(LogLevel.Debug, message, args);
    }

    public void LogInformation(string? message, params object?[] args)
    {
        Log(LogLevel.Information, message, args);
    }

    private void Log(LogLevel logLevel, string? message, params object?[] args)
    {
        Task.Run(() =>
        {
            try
            {
                using var command = _clickHouseConnector.Connection.CreateCommand();
                command.CommandText = $"insert into vg_autodrill_db_ck.schedule_logs(sequenceID,level,message,logged)"
                + $" values('{DateTime.Now.Ticks}','{logLevel.ToString()}','{string.Format(message, args)}','{DateTime.Now}')";
                _logger.LogDebug($"ClickHouseLogger sql:{command.CommandText}");

                command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ClickHouseLogger error:{ex.Message}");
            }
        });
    }
}
