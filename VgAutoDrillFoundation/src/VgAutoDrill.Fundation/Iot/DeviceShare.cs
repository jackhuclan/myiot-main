using Microsoft.Extensions.DependencyInjection;
using Modbus.Device;
using VgAutoDrill.Fundation.Alarm;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.CNC;
using VgAutoDrill.Fundation.Drill;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.State;
using VgAutoDrill.Fundation.Store;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VgAutoDrill.Fundation.Iot;

public class DeviceShare<TDevice> : IDeviceShare
    where TDevice : Device
{
    private readonly IServiceProvider _serviceProvider;
    public DeviceShare(IServiceProvider serviceProvider, TDevice device)
    {
        ShareData = new();
        _serviceProvider = serviceProvider;
        ApplicationServices = serviceProvider;
        InteractingDevice = device;
        WatchingProperties = device.WatchingProperties;
        DeviceDescriptor = device.DeviceDescriptor;
        PayloadPanels = device.PayloadPanels;
        PayloadCutterTrays = device.PayloadCutterTrays;
        Engine = device.Engine;
        Connector = device.Connector;
        HttpRequestInvoker = device.HttpRequestInvoker;
        CentralWebOptions = device.CentralWebOptions;
        DeviceProvider = device.DeviceProvider;
        MqttClientWrapper = device.MqttClientWrapper;
        EventContainer = device.EventContainer;
        StateContainer = device.StateContainer;
        PropertyContainer = device.PropertyContainer;
        AlarmContainer = device.AlarmContainer;
        ScheduleContainer = device.ScheduleContainer;
        DataExporter = device.DataExporter;
        DeviceStore = device.DeviceStore;
    }

    /// <inheritdoc/>
    public TDevice InteractingDevice { get; }

    /// <inheritdoc/>
    public WatchableProperties ShareData { get; }

    /// <inheritdoc/>
    public WatchableProperties WatchingProperties { get; }

    /// <inheritdoc/>
    public DeviceDescriptor DeviceDescriptor { get; }

    /// <inheritdoc/>
    public PanelList PayloadPanels { get; }
    /// <inheritdoc/>
    public CutterTrays PayloadCutterTrays { get; }

    /// <inheritdoc/>
    public IServiceProvider ApplicationServices { get; }
    public IDeviceEngine Engine { get; }
    public IDeviceConnector? Connector { get; }

    public ICNCCommandWrapper? CNCCommandWrapper
    {
        get
        {
            if (InteractingDevice.Connector is ModbusDeviceConnector modbusConnector)
            {
                return modbusConnector.CNCCommandWrapper;
            }

            return null;
        }
    }

    public IModbusIpMasterWrapper? ModbusIpMasterWrapper
    {
        get
        {
            if (InteractingDevice.Connector is ModbusDeviceConnector modbusConnector)
            {
                return modbusConnector.ModbusIpMasterWrapper;
            }

            return null;
        }
    }

    public IObjectFactory ObjectFactory { get { return _serviceProvider.GetRequiredService<IObjectFactory>(); } }

    public IHttpRequestInvoker HttpRequestInvoker { get; }

    public IMqttClientWrapper MqttClientWrapper { get; }

    public CentralWebOptions CentralWebOptions { get; }

    public IDeviceProvider DeviceProvider { get; }

    public IMessageChannel DataExporter { get; }

    public IEventHandlerContainer EventContainer { get; }

    public IStateHandlerContainer StateContainer { get; }

    public IPropertyHandlerContainer PropertyContainer { get; }

    public IAlarmHandlerContainer AlarmContainer { get; }
    public IScheduleHandlerContainer ScheduleContainer { get; }
    public IDeviceStore DeviceStore { get; }
    public IPeriodicTimerExecutorFactory PeriodicTimers { get { return _serviceProvider.GetRequiredService<IPeriodicTimerExecutorFactory>(); } }
    public IDrillFilePathLocator DrillFilePathLocator { get { return _serviceProvider.GetRequiredService<IDrillFilePathLocator>(); } }
    public IModbusMaster? ModbusIpMaster { get { return ModbusIpMasterWrapper?.ModbusIpMaster; } }
    public ICNCCommand? CNCCommand { get { return CNCCommandWrapper?.CNCCommand; } }

    /// <inheritdoc/>
    public async Task<DeviceServiceInvokeResponse> ResponseSuccess(string message = "", Dictionary<string, object?> data = default)
    {
        return await InteractingDevice.ResponseSuccess(message, data);
    }

    /// <inheritdoc/>
    public async Task<DeviceServiceInvokeResponse> ResponseFail(string message = "", Dictionary<string, object?> data = default)
    {
        return await InteractingDevice.ResponseFail(message, data);
    }

    /// <inheritdoc/>
    public async Task<DeviceServiceInvokeResponse> Response(string code, string message = "", Dictionary<string, object?> data = default)
    {
        return await InteractingDevice.Response(code, message, data);
    }
}
