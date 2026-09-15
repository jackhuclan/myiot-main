using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Iot;

public class DeviceProvider : IDeviceProvider
{
    private readonly IDictionary<string, DeviceDescriptor> deviceOptions;
    private readonly IDictionary<string, Device> devices;
    private readonly IServiceProvider provider;

    public DeviceProvider(IDictionary<string, DeviceDescriptor> deviceOptions, IServiceProvider provider)
    {
        this.deviceOptions = deviceOptions;
        this.provider = provider;
        devices = new Dictionary<string, Device>();
    }

    public IReadOnlyList<Device> Devices
    {
        get
        {
            foreach (var item in deviceOptions.Where(x => !devices.ContainsKey(x.Key)))
            {
                GetDevice(item.Key);
            }

            return devices.Values.ToList().AsReadOnly();
        }
    }

    public Device GetDevice(string? deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            ThrowHelper.ThrowArgumentNullException(nameof(deviceId));

        if (devices.ContainsKey(deviceId)) return devices[deviceId];

        DeviceDescriptor descriptor = deviceOptions[deviceId];

        var deviceClazz = typeof(Device).Assembly.GetTypes()
               .SingleOrDefault(x => x.FullName == descriptor.DeviceClazz);

        if (deviceClazz == null)
        {
            var basePath = AppContext.BaseDirectory;
            var mocksDllFile = Path.Combine(basePath, descriptor.DeviceDllFilePath);

            deviceClazz = Assembly.LoadFrom(mocksDllFile).GetTypes()
               .SingleOrDefault(x => x.FullName == descriptor.DeviceClazz);

            if (deviceClazz == null)
            {
                throw new ArgumentException($"Please configure correct DeviceClazz:{descriptor.DeviceClazz}");
            }
        }

        descriptor.FundationVersion = Assembly.GetExecutingAssembly().GetName().Version ?? Version.Parse("1.0.0.0");
        descriptor.AgentVersion = deviceClazz.Assembly.GetName().Version ?? Version.Parse("1.0.0.0");

        var engine = ActivatorUtilities.CreateInstance(provider, typeof(ManualEngine), descriptor) as IDeviceEngine;

        if (descriptor.AutoMode)
        {
            engine = ActivatorUtilities.CreateInstance(provider, typeof(AutomaticEngine), descriptor) as IDeviceEngine;
        }

        var device = ActivatorUtilities.CreateInstance(provider, deviceClazz, engine, descriptor) as Device;

        ThrowHelper.ThrowArgumentNullException(device, "Please configure correct DeviceClazz.");
        devices.Add(deviceId, device);
        return device;
    }

}
