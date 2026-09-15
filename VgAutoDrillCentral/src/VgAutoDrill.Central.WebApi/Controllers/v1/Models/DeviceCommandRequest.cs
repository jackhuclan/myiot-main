namespace VgAutoDrill.Central.WebApi.Controllers.v1.Models;

public class DeviceCommandRequest
{
    public string DeviceId { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public Dictionary<string, object?> Params { get; set; } = new Dictionary<string, object?>();
}
