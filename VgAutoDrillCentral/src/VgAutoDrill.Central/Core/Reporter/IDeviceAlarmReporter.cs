using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter;

public interface IDeviceAlarmReporter
{
    Task<DeviceAlarmReportResponse> Report(DeviceAlarmReportRequest? request);
}
