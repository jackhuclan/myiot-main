using VgAutoDrill.Fundation.CNC;

namespace VgAutoDrill.Fundation.Iot;

public interface IModbusOperator
{
    ICNCCommandWrapper CNCCommandWrapper { get; }
    IModbusIpMasterWrapper ModbusIpMasterWrapper { get; }
}
