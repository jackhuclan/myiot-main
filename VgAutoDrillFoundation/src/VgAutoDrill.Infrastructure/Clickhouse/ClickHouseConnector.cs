using ClickHouse.Client.ADO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VgAutoDrill.Infrastructure.Clickhouse;

public class ClickHouseConnector : IClickHouseConnector
{
    private readonly ClickHouseConnection _connection;
    private readonly ILogger<ClickHouseConnector> _logger;
    private readonly ClickHouseConnectionOptions _clickHouseOptions;

    public ClickHouseConnector(IOptions<ClickHouseConnectionOptions> options,
        ILoggerFactory loggerFactory)
    {
        _clickHouseOptions = options.Value;
        _logger = loggerFactory.CreateLogger<ClickHouseConnector>();

        var builder = new ClickHouseConnectionStringBuilder(_clickHouseOptions.ConnectionString);
        builder.Compression = _clickHouseOptions.Compression;
        builder.UseSession = _clickHouseOptions.Session;
        builder.UseCustomDecimals = _clickHouseOptions.CustomDecimals;
        _connection = new ClickHouseConnection(builder.ConnectionString);

        _logger.LogDebug($"this.ClickHouseConnection :{_connection}");
        _logger.LogDebug($"ClickHouseOptions.Enabled:{_clickHouseOptions.Enabled}");
    }

    public ClickHouseConnection Connection { get { return _connection; } }
    public ClickHouseConnectionOptions ConnectionOptions { get { return _clickHouseOptions; } }
}
