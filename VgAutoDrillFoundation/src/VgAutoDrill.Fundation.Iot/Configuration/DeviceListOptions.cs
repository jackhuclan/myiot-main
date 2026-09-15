namespace VgAutoDrill.Fundation.Iot.Configuration;

public class DeviceListOptions
{
    public const string Options = "DeviceListOptions";
    public List<DeviceDescriptor> Devices { get; set; } = new();
}
