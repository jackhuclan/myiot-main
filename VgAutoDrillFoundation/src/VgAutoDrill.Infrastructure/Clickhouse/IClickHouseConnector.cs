using ClickHouse.Client.ADO;

namespace VgAutoDrill.Infrastructure.Clickhouse;

public interface IClickHouseConnector
{
    ClickHouseConnection Connection { get; }
    ClickHouseConnectionOptions ConnectionOptions { get; }
}
