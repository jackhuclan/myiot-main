using Microsoft.Extensions.Hosting;

namespace VgAutoDrill.Fundation.Iot;

public class EngineStarter : IHostedService
{
    private readonly IDeviceProvider _deviceProvider;

    public EngineStarter(IDeviceProvider deviceProvider)
    {
        _deviceProvider = deviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var device in _deviceProvider.Devices)
        {
            await device.FireEngine(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var engines = _deviceProvider.Devices.Select(x => x.Engine).Distinct();
        foreach (var engine in engines)
        {
            await engine.Shutdown(cancellationToken);
        }
    }
}
