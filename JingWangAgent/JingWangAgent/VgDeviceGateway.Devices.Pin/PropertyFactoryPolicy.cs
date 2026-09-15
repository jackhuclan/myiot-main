using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;

namespace VgDeviceGateway.Devices.Pin;

public class PropertyFactoryPolicy : DeviceShare<Pin>
{
    private readonly ILogger<PropertyFactoryPolicy> logger;

    public PropertyFactoryPolicy(ILogger<PropertyFactoryPolicy> logger, IServiceProvider serviceProvider, Pin device) : base(serviceProvider, device)
    {
        this.logger = logger;
        CreatePinPropertyHandler(device);
    }

    public void CreatePinPropertyHandler(Pin device)
    {
        //TODO 根据类型创建
        // var pinType = device.DeviceDescriptor.Extra["PinKind"].ToString();
        PropertyContainer.AddHandler<PinPropertyHandler, Pin>(device);
    }
}
