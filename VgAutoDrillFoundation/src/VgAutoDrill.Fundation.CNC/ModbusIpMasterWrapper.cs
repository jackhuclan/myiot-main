using System.Net.Sockets;
using Modbus.Device;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.CNC;

public class ModbusIpMasterWrapper : IModbusIpMasterWrapper
{
    public IModbusMaster? ModbusIpMaster { get; private set; }

    public IInovanceModbusIp CreateInovanceModbusIp()
    {
        return new InovanceModbusIp();
    }

    public IModbusMaster CreateIp(string ip, int port)
    {
        try
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
            ModbusIpMaster = Modbus.Device.ModbusIpMaster.CreateIp(tcpClient);
            return ModbusIpMaster;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }

    }
}
