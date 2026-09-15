namespace VgAutoDrill.Fundation.Mqtt.Client;

public interface IMqttClientWrapper : IDisposable
{
    bool IsConnected { get; }
    Task ConnectMqtt(CancellationToken cancellationToken = default);

    event Func<Task> OnConnected;
    event Func<Task> OnDisconnected;
}
