using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot;

public class CutterTrays : ObservableList<CutterTray, DeviceCutterTrayChangedResponse>
{
    public List<CutterTray> GetCutterTrayOfLayer(int layer) => this.Where(x => x.Z == layer).OrderBy(x => x.IndexOnLayer).ToList();
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
