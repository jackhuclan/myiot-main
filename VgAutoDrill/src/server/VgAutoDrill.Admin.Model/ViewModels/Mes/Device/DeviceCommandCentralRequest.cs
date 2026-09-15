namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class DeviceCommandCentralRequest
    {
        public string DeviceId { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public Dictionary<string, object?> Params { get; set; } = new Dictionary<string, object?>();
    }
}
