using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Channel;

public interface IExternalDataReportFactory
{
    IExternalDataReport? Create(Device device);
}
