using VgDeviceGateway.Devices.Shelf;

namespace VegaIot.External.Hik;

public class HikAlarmHandler : SiloShelfAlarmHandler
{
    public HikAlarmHandler(IServiceProvider serviceProvider, HikTransfer device) : base(serviceProvider, device)
    {
    }
}
