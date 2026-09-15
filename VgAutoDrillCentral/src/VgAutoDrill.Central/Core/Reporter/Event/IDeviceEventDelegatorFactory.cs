using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal interface IDeviceEventDelegatorFactory
{
    IDeviceEventDelegator CreateEventDelegator(DeviceEventReportRequest request);
}
