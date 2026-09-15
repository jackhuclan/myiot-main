namespace VgAutoDrill.Central.Core.Flusher;

public interface IPropertiesFlusher
{
    Task OnPropertiesRefreshed(DeviceProxy deviceProxy);
}
