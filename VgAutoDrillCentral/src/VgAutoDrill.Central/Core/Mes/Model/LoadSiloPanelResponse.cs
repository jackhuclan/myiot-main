using System.Text.Json;

namespace VgAutoDrill.Central.Core.Mes;

public class LoadSiloPanelResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
