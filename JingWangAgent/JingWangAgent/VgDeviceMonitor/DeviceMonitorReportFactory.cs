using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;

namespace VgDeviceMonitor;

public class DeviceMonitorReportFactory : IExternalDataReportFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DeviceMonitorReportFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IExternalDataReport? Create(Device device)
    {
        return (IExternalDataReport)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(DeviceMonitorReport), device);
    }
}
