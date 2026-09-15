namespace VgAutoDrill.Central.Core.Mes.Model;

public class DeviceRouteCodesPair
{
    public string DeviceId { get; set; } = string.Empty;
    public virtual List<string> RouteCodes { get; set; } = new List<string>();
}
