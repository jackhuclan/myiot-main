using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter;

public interface IDeviceServiceReporter
{
    Task<DeviceServiceInvokeResponse> Report(DeviceServiceInvokeRequest? request);
}
