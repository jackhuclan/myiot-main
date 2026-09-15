using Modbus.Device;
using VgAutoDrill.Fundation.Alarm;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.CNC;
using VgAutoDrill.Fundation.Event;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Mqtt.Client;
using VgAutoDrill.Fundation.Property;
using VgAutoDrill.Fundation.State;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VgAutoDrill.Fundation.Iot;

public interface IDeviceShare
{
    /// <summary>
    /// 属于Handler,Policy自己的监控属性
    /// </summary>
    WatchableProperties ShareData { get; }
    /// <summary>
    /// Agent公用的服务
    /// </summary>
    IServiceProvider ApplicationServices { get; }
    /// <summary>
    /// 定义设备要监控的属性
    /// </summary>
    WatchableProperties WatchingProperties { get; }
    /// <summary>
    /// 设备描述符
    /// </summary>
    DeviceDescriptor DeviceDescriptor { get; }
    /// <summary>
    /// 设备上装载的板料信息
    /// </summary>
    PanelList PayloadPanels { get; }
    /// <summary>
    /// 设备上装载的刀盘信息
    /// </summary>
    CutterTrays PayloadCutterTrays { get; }
    /// <summary>
    /// 设备驱动引擎<see cref="ManualEngine"/> <seealso cref="AutomaticEngine"/>
    /// </summary>
    IDeviceEngine Engine { get; }
    /// <summary>
    /// 设备与底层硬件的连接器
    /// </summary>
    IDeviceConnector? Connector { get; }
    ICNCCommandWrapper? CNCCommandWrapper { get; }
    IModbusIpMasterWrapper? ModbusIpMasterWrapper { get; }
    IHttpRequestInvoker HttpRequestInvoker { get; }
    IMqttClientWrapper MqttClientWrapper { get; }
    CentralWebOptions CentralWebOptions { get; }
    IDeviceProvider DeviceProvider { get; }
    IMessageChannel DataExporter { get; }
    IEventHandlerContainer EventContainer { get; }
    IStateHandlerContainer StateContainer { get; }
    IPropertyHandlerContainer PropertyContainer { get; }
    IAlarmHandlerContainer AlarmContainer { get; }
    IScheduleHandlerContainer ScheduleContainer { get; }
    IPeriodicTimerExecutorFactory PeriodicTimers { get; }
    IObjectFactory ObjectFactory { get; }
    IModbusMaster? ModbusIpMaster { get; }
    ICNCCommand? CNCCommand { get; }

    /// <summary>
    /// 返回成功给调用者
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> ResponseSuccess(string message = "", Dictionary<string, object?> data = default);

    /// <summary>
    /// 返回失败给调用者
    /// </summary>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> ResponseFail(string message = "", Dictionary<string, object?> data = default);

    /// <summary>
    /// 返回response给调用者
    /// </summary>
    /// <param name="code"></param>
    /// <param name="message"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<DeviceServiceInvokeResponse> Response(string code, string message = "", Dictionary<string, object?> data = default);
}
