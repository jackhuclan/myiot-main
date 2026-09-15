using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Manager;

namespace VgAutoDrill.Central.Core.Domain;

public class Pin : DeviceProxy
{
    private readonly ILocationManager _locationManager;

    [ActivatorUtilitiesConstructor]
    public Pin(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
    }

    /// <summary>
    /// 所拥有的库位
    /// </summary>
    public List<Location> Locations => _locationManager.Locations
                                .Where(x => x.HostDevice?.DeviceId == this.DeviceId)
                                .ToList();
    public override string GetLocationCode(int position) => $"{DeviceId}{position.ToString().PadLeft(3, '0')}";
}
