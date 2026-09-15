// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Modbus.Device;
using VgAutoDrill.Fundation.CNC;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Utils;

namespace Vegalot.External.XianjinIot.Agv;

internal class XianjInIotDefaultAgv : Device
{
    private readonly IModbusIpMasterWrapper modbusIpMasterWrapper;
    public IModbusMaster modbusIpMaster;
    public readonly ILogger<XianjInIotDefaultAgv> logger;
    public Tuple<string, string, string> errorInfo;
    public volatile string errorsMsg = "";
    public static bool IsRuning = false;
    private readonly Uri plcUri;
    private byte slaveID = default;

    public XianjInIotDefaultAgv(
        DeviceDescriptor deviceDescriptor,
        IDeviceEngine deviceEngine,
        IServiceProvider serviceProvider,
        ILogger<XianjInIotDefaultAgv> logger)
        : base(deviceDescriptor, deviceEngine, serviceProvider)
    {
        this.logger = logger;
        errorInfo = new Tuple<string, string, string>("", "", "");
        modbusIpMasterWrapper = ((IModbusOperator)Engine.DeviceConnector).ModbusIpMasterWrapper;
        slaveID = (byte)DeviceDescriptor.Extra["SlaveId"].ToInt();
        plcUri = !string.IsNullOrEmpty(deviceDescriptor.Extra["ModbusTcpUri"].ToStr()) ? new Uri(deviceDescriptor.Extra["ModbusTcpUri"].ToStr()) : new Uri("");

        Connector.ConnectFunc = (deviceDescriptor) => Task.Run(() =>
        {
            return MachineConnect();
        });

        Connector.HeartBeatFunc = () => HeartBeat();

        Connector.IsConnected = IsConnected();
    }

    /// <summary>
    /// 心跳检测(500毫秒触发一次)
    /// </summary>
    /// <returns></returns>
    private async  Task HeartBeat()
    {
        //心跳检测
        if (Connector.IsConnected)
        {
            ///plc端 红心1、0闪烁
            modbusIpMaster.WriteSingleRegister(slaveID, 3050, 1);
            await Task.Delay(250);
            modbusIpMaster.WriteSingleRegister(slaveID, 3050, 0);
        }
        else
        {
            modbusIpMaster.Dispose();
            logger.LogInformation("PLC断线，重新连接。。。。。");
            await MachineConnect();//断线重连
        }        
    }
    /// <summary>
    /// 检测是否连接
    /// </summary>
    /// <returns></returns>
    private bool IsConnected()
    {
        if (modbusIpMaster == null)
        {
            return false;
        }
        var data =  modbusIpMaster.ReadHoldingRegisters(slaveID, 3052, 1);//3052 为固定的PLC心跳检测通讯地址
        return data?.Length > 0 && data[0] == 1;
    }

    private Task<bool> MachineConnect() => Task.Run(() =>
    {
        try
        {
            if (string.IsNullOrEmpty(DeviceDescriptor.Extra["ModbusTcpUri"].ToStr()))
            {
                return true;
            }
            if (modbusIpMaster == null)
            {
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   begin to PlcConnect!");
                modbusIpMaster = modbusIpMasterWrapper.CreateIp(plcUri.Host, plcUri.Port);
                logger.LogDebug($"{DateTime.Now.ToLongTimeString()}-{ProductId}-{DeviceName}-{DeviceId}   finish  PlcConnect!");
            }

            if (modbusIpMaster == null)
            {
                logger.LogDebug("PLC链接失败 modbusIpMaster == null");
                errorInfo = new Tuple<string, string, string>("AGV_MachineConnect_modbusIpMasterNull", "modbusIpMasterNull", "PLC链接失败");
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            modbusIpMaster.Dispose();
            logger.LogError(ex, ex.Message);
            errorInfo = new Tuple<string, string, string>("AGV_MachineConnect_Exception", "Exception", ex.Message);
            return false;
        }
    });
}
