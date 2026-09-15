using System.Text.Json;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class DeviceCommandResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, object?> Params { get; set; } = new Dictionary<string, object?>();
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
