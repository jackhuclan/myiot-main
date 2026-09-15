using Microsoft.Extensions.DependencyInjection;
using UnitTest.VgAutoDrill.Central.Mock;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Infrastructure.Clickhouse;

namespace UnitTest.VgAutoDrill.Central;

public class ClickHouseLoggerTest : CentralTestBase
{
    [Fact]
    public void LogWarningShouldWork()
    {
        var services = ConstructRequiredServiceCollection();
        services.AddSingleton<IClickHouseConnector, ClickHouseConnector>();
        services.AddSingleton<IClickHouseLogger, ClickHouseLogger>();
        services.AddSingleton(new MockOptions<ClickHouseConnectionOptions>().Mock(new ClickHouseConnectionOptions()
        {
            ConnectionString = "Compress=False;BufferSize=32768;SocketTimeout=10000;CheckCompressedHash=False;Compressor=lz4;Host=192.168.104.253;Port=8123;Database=vg_autodrill_db_ck;User=default;Password=",
            Compression = true,
            Session = false,
            CustomDecimals = true,
            Enabled = true,
        }).Object);

        var serviceProvider = services.BuildServiceProvider();
        var clickHouseLogger = serviceProvider.GetRequiredService<IClickHouseLogger>();

        clickHouseLogger.LogWarning("LogWarning{0}", "11");
        clickHouseLogger.LogError("LogError{0}", "22");
        clickHouseLogger.LogDebug("LogDebug{0}", "33");
        clickHouseLogger.LogInformation("LogInformation{0}", "44");
    }
}
