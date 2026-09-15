namespace VgAutoDrill.Central.Core.Flusher;

public interface ICutterFlusher
{
    Task OnCutterRefreshed(DeviceProxy deviceProxy);
}
