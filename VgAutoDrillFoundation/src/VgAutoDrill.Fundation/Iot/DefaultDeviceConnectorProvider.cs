using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.CNC;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Iot;

public class DefaultDeviceConnectorProvider : IDeviceConnectorProvider
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultDeviceConnectorProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDeviceConnector GetDeviceConnector(DeviceDescriptor deviceDescriptor)
    {
        switch (deviceDescriptor.ConnectorProtocol)
        {
            case DeviceProtocolKind.Modbus:
                var modbusIpMasterWrapper = ActivatorUtilities.CreateInstance(_serviceProvider, typeof(ModbusIpMasterWrapper)) as IModbusIpMasterWrapper;
                var cnc84CommandWrapper = ActivatorUtilities.CreateInstance(_serviceProvider, typeof(CNCCommandWrapper), deviceDescriptor) as ICNCCommandWrapper;

                ThrowHelper.ThrowArgumentNullException(modbusIpMasterWrapper);
                ThrowHelper.ThrowArgumentNullException(cnc84CommandWrapper);

                var connector = ActivatorUtilities.CreateInstance(_serviceProvider, typeof(ModbusDeviceConnector), deviceDescriptor, modbusIpMasterWrapper, cnc84CommandWrapper) as IDeviceConnector;

                ThrowHelper.ThrowArgumentNullException(connector);

                return connector!;
            case DeviceProtocolKind.MC:
                return new MCDeviceConnector();
            default:
                return new NullDeviceConnector();
        }
    }
}
