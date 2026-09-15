using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter;

public interface IDevicePropertyReporter
{
    Task<DevicePropertiesReportResponse> Report(DevicePropertiesReportRequest? request);
}
