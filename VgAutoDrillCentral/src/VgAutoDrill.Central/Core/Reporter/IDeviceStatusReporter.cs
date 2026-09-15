using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter;

public interface IDeviceStatusReporter
{
    Task<DeviceStatusReportResponse> Report(DeviceStatusReportRequest? request);
}
