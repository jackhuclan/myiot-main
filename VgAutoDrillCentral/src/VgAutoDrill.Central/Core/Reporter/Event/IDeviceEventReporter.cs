using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter.Event;

public interface IDeviceEventReporter
{
    Task<DeviceEventReportResponse> Report(DeviceEventReportRequest? request);
}
