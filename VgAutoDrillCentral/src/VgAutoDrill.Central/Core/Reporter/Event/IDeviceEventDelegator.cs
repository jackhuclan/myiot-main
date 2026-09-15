using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter.Event;

public interface IDeviceEventDelegator
{
    Task<DeviceEventReportResponse> HandleEvent(DeviceEventReportRequest request);
}
