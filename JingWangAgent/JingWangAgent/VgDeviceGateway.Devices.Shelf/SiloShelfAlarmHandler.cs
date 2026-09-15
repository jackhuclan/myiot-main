using VgAutoDrill.Fundation.Alarm;

namespace VgDeviceGateway.Devices.Shelf;

public class SiloShelfAlarmHandler : AbstractAlarmHandler<SiloShelf>
{
    public SiloShelfAlarmHandler(IServiceProvider serviceProvider, SiloShelf device)
        : base(serviceProvider, device)
    {
    }

    public override void AddWatchingAlarms()
    {
    }
}
