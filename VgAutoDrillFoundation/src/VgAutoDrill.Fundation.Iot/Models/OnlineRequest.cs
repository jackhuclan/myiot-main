using System.Text.Json;
using VgAutoDrill.Fundation.Iot.Configuration;

namespace VgAutoDrill.Fundation.Iot.Models;

public class OnlineRequest
{
    public DeviceDescriptor Descriptor { get; set; } = new DeviceDescriptor();
    public bool Connected { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}
