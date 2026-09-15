using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot.Extensions;

public static class CutterTrayExtensions
{
    public static bool IsNull(this CutterTray? panel)
    {
        return panel == null
            || panel.Status == CutterTrayStatus.Noop
            || panel.Status == CutterTrayStatus.NoTray
            || panel.Status == CutterTrayStatus.EmptyTray;
    }
}
