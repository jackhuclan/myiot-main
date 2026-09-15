using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.CNC;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Iot;

public class ModbusDeviceConnector : IDeviceConnector, IModbusOperator
{
    private readonly DeviceDescriptor deviceDescriptor;
    private readonly ILogger<ModbusDeviceConnector> logger;
    private volatile bool _isConnected = false;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

    public bool IsConnected
    {
        get => _isConnected;
        set
        {
            _isConnected = value;
            OnConnected?.Invoke(this, new DevcieConnectedEventArgs(deviceDescriptor, _isConnected));
        }
    }

    public ICNCCommandWrapper CNCCommandWrapper { get; private set; }
    public IModbusIpMasterWrapper ModbusIpMasterWrapper { get; private set; }
    public Func<DeviceDescriptor, Task<bool>> ConnectFunc { get; set; } = (d) => Task.FromResult(false);
    public Func<Task> HeartBeatFunc { get; set; } = () => Task.CompletedTask;

    public event EventHandler<DevcieConnectedEventArgs>? OnConnected;

    public ModbusDeviceConnector(DeviceDescriptor deviceDescriptor,
        ICNCCommandWrapper cNC84CommandWrapper,
        IModbusIpMasterWrapper modbusIpMasterWrapper,
        ILoggerFactory loggerFactory)
    {
        this.deviceDescriptor = deviceDescriptor;
        this.CNCCommandWrapper = cNC84CommandWrapper;
        this.ModbusIpMasterWrapper = modbusIpMasterWrapper;
        this.logger = loggerFactory.CreateLogger<ModbusDeviceConnector>();
    }

    public async Task Connect(CancellationToken cancellationToken = default)
    {
        try
        {
            _autoResetEvent.WaitOne();
            _isConnected = await ConnectFunc.Invoke(this.deviceDescriptor);
            logger.LogInformation($"{this.deviceDescriptor.DeviceId} device IsConnected={IsConnected}");
        }
        finally
        {
            _autoResetEvent.Set();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (OnConnected is null)
        {
            return;
        }

        foreach (var @delegate in OnConnected.GetInvocationList())
        {
            Delegate.Remove(OnConnected, @delegate);
        }

        this.OnConnected = null;
    }
}
