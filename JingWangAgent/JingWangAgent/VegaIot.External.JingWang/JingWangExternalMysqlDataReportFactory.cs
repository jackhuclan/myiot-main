using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Channel;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VegaIot.External.JingWang;

internal class JingWangExternalMysqlDataReportFactory : IExternalDataReportFactory
{
    private readonly IServiceProvider _serviceProvider;

    public JingWangExternalMysqlDataReportFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IExternalDataReport? Create(Device device)
    {
        switch (device.DeviceDescriptor.DeviceKind)
        {
            case DeviceKind.CNC84Drill:
            case DeviceKind.CNC95Drill:
                return (IExternalDataReport)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(JingWangDrillMysqlDataReport), device);
        }

        return null;
    }
}
