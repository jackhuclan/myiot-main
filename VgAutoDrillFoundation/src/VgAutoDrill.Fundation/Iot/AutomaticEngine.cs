using Microsoft.Extensions.Logging;
using Quartz;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Jobs;
using VgAutoDrill.Fundation.Mqtt.Client;

namespace VgAutoDrill.Fundation.Iot;

public class AutomaticEngine : ManualEngine
{
    private readonly IMqttClientWrapper _mqttClientWrapper;
    private readonly IClientMqttListener _clientMqttListener;
    private readonly ILogger<AutomaticEngine> _logger;

    public AutomaticEngine(IDeviceConnectorProvider deviceConnectorProvider,
        IMqttClientWrapper mqttClientWrapper,
        ISchedulerFactory schedulerFactory,
        IClientMqttListener clientMqttListener,
        ILoggerFactory loggerFactory,
        DeviceDescriptor deviceDescriptor) :
        base(deviceConnectorProvider, schedulerFactory, loggerFactory, deviceDescriptor)
    {
        _mqttClientWrapper = mqttClientWrapper;
        _clientMqttListener = clientMqttListener;
        _logger = loggerFactory.CreateLogger<AutomaticEngine>();
    }

    public override async Task Fire(Device device, CancellationToken cancellationToken)
    {
        _mqttClientWrapper.OnConnected += () => _clientMqttListener.OnConnected(device);
        _mqttClientWrapper.OnDisconnected += () => _clientMqttListener.OnDisonnected(device);

        await ConnectDevice(device, cancellationToken);
        await StartDataCollectingJob(device, cancellationToken);
        await StartScheduleTaskExecutingJob(device, cancellationToken);
        _ = _mqttClientWrapper.ConnectMqtt(cancellationToken);
    }

    protected override Task ConnectDevice(Device device, CancellationToken cancellationToken)
    {
        DeviceConnector.OnConnected += (object? sender, DevcieConnectedEventArgs e) =>
        {
            _logger.LogInformation($"{e.DeviceDescriptor.DeviceId} DevcieConnected:{e.IsConnected}...");
        };

        return base.ConnectDevice(device, cancellationToken);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _mqttClientWrapper.Dispose();
    }

    private async Task StartScheduleTaskExecutingJob(Device device, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AutomaticEngine ScheduleTaskExecutingJob...");
        await ScheduleJobAsync<ScheduleTaskExecutingJob>(device, TimeSpan.FromSeconds(device.DeviceDescriptor.ScheduleTaskExecutingPerSeconds), $"{device.DeviceId}_Group", cancellationToken);
    }
}
