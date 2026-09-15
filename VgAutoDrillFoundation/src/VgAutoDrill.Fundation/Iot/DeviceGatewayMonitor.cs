using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.OpenAPI;

namespace VgAutoDrill.Fundation.Iot;

internal class DeviceGatewayMonitor : BackgroundService
{
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ILogger<DeviceGatewayMonitor> _logger;
    private readonly DeviceGatewayOptions _gatewayOptions;
    private readonly CentralWebOptions _centralWebOptions;
    private readonly PeriodicTimer _timer;

    public DeviceGatewayMonitor(IHttpRequestInvoker httpRequestInvoker,
        IOptions<DeviceGatewayOptions> options,
        IOptions<CentralWebOptions> webOptions,
        IDeviceProvider deviceProvider,
        ILoggerFactory loggerFactory)
    {
        _httpRequestInvoker = httpRequestInvoker;
        _gatewayOptions = options.Value;
        _gatewayOptions.FundationVersion = deviceProvider.Devices[0].DeviceDescriptor.FundationVersion.ToString() ?? "1.0.0.0";
        _gatewayOptions.AppVersion = deviceProvider.Devices[0].DeviceDescriptor.AgentVersion.ToString() ?? "1.0.0.0";
        _centralWebOptions = webOptions.Value;
        _timer = new PeriodicTimer(_gatewayOptions.NoticeCentralHeartbeat);
        _logger = loggerFactory.CreateLogger<DeviceGatewayMonitor>();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"agent logout with {_gatewayOptions}");
        if (_gatewayOptions.Enabled)
        {
            await _httpRequestInvoker.PostAsJsonAsync<DeviceGatewayOptions, object>(_centralWebOptions.Gateway + "/" + "logout", _gatewayOptions);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (_timer)
        {
            _logger.LogInformation($"FundationVersion={_gatewayOptions.FundationVersion}");
            _logger.LogInformation($"AgentVersion={_gatewayOptions.AppVersion}");

            while (_gatewayOptions.Enabled
                && !stoppingToken.IsCancellationRequested
                && await _timer.WaitForNextTickAsync())
            {
                _logger.LogInformation($"notice central agent login with {_gatewayOptions}");
                await _httpRequestInvoker.PostAsJsonAsync<DeviceGatewayOptions, object>(_centralWebOptions.Gateway + "/" + "login", _gatewayOptions);
            }
        }
    }
}
