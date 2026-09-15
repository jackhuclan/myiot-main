using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter;

public interface IDevicePanelReporter
{
    Task<DevicePanelChangedResponse> Report(DevicePanelChangedRequest? request);
}
