using VgAutoDrill.Fundation.Store;
using VgDeviceGateway.Devices.Common;

namespace VgDeviceGateway.Devices.Pin;

public class LocationPin : LocationData<Pin>
{
    public int HandlAskUpload { get; set; }
    public int HandleAskDownLoad { get; set; }

    public LocationPin(IServiceProvider serviceProvider, IDeviceStore deviceStore, Pin device)
        : base(serviceProvider, deviceStore, device)
    {
    }
}
