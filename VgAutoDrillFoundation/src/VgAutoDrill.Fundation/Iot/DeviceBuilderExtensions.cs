using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Iot;

public static class DeviceBuilderExtensions
{
    public static DeviceProvider BuildDeviceProvider(this IDeviceDescriptorCollection descriptors, IServiceProvider provider)
    {
        if (descriptors is null)
        {
            throw new ArgumentNullException(nameof(descriptors));
        }
        if (provider is null)
        {
            throw new ArgumentNullException(nameof(provider));
        }

        return new DeviceProvider(descriptors, provider);
    }
}
