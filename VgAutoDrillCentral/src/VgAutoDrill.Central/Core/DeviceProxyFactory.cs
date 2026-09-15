using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core;

public class DeviceProxyFactory : IDeviceProxyFactory
{
    private readonly IServiceProvider _provider;
    private readonly IObjectFactory _objectFactory;

    public DeviceProxyFactory(IServiceProvider provider, IObjectFactory objectFactory)
    {
        _provider = provider;
        _objectFactory = objectFactory;
    }

    public DeviceProxy Create(DeviceDescriptor deviceDescriptor)
    {
        switch (deviceDescriptor.DeviceKind)
        {
            case DeviceKind.CNC84Drill:
                return (CNC84Drill)ActivatorUtilities.CreateInstance(_provider, typeof(CNC84Drill));

            case DeviceKind.CNC95Drill:
                return (CNC95Drill)ActivatorUtilities.CreateInstance(_provider, typeof(CNC95Drill));

            case DeviceKind.FrontPanelAgv:
                return (FrontPanelAgv)ActivatorUtilities.CreateInstance(_provider, typeof(FrontPanelAgv));

            case DeviceKind.BackPanelAgv:
                return (BackPanelAgv)ActivatorUtilities.CreateInstance(_provider, typeof(BackPanelAgv));

            case DeviceKind.ShelfSiloAgv:
                return (TransferSiloAgv)ActivatorUtilities.CreateInstance(_provider, typeof(TransferSiloAgv));

            case DeviceKind.PanelSiloFork:
                return (PanelSiloFork)ActivatorUtilities.CreateInstance(_provider, typeof(PanelSiloFork));

            case DeviceKind.PublicPanelSiloWIP:
                return (PanelSiloShelf)ActivatorUtilities.CreateInstance(_provider, typeof(PanelSiloShelf));

            case DeviceKind.Pin:
                return (Pin)ActivatorUtilities.CreateInstance(_provider, typeof(Pin));

            case DeviceKind.UnPin:
                return (UnPin)ActivatorUtilities.CreateInstance(_provider, typeof(UnPin));

            default:
                return Create();
        }
    }

    public DeviceProxy Create()
    {
        return (DeviceProxy)ActivatorUtilities.CreateInstance(_provider, typeof(DeviceProxy));
    }
}
