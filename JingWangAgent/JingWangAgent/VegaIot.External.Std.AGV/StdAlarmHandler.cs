using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Std.Shelf;

public class StdAlarmHandler : SiloShelfAlarmHandler
{
    public StdAlarmHandler(IServiceProvider serviceProvider, StdShelf device) : base(serviceProvider, device)
    {
    }
}
