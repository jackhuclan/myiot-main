using Modbus.Device;

namespace VgAutoDrill.Fundation.CNC;

public interface IModbusIpMasterWrapper
{
    IModbusMaster? ModbusIpMaster { get; }
    IModbusMaster CreateIp(string ip, int port);
    IInovanceModbusIp CreateInovanceModbusIp();
}
