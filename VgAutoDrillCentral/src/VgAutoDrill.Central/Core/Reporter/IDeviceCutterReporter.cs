using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter;

public interface IDeviceCutterReporter
{
    Task<DeviceCutterTrayChangedResponse> Report(DeviceCutterTrayChangedRequest? request);
}
