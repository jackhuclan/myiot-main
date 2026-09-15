using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter.Event;

internal class DeviceEventDelegatorFactory : IDeviceEventDelegatorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DeviceEventDelegatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDeviceEventDelegator CreateEventDelegator(DeviceEventReportRequest request)
    {
        if (request.EventId == Events.SCAN_PANEL_CODE)
        {
            return (IDeviceEventDelegator)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(ScanPanelCodeEventDelegator));
        }

        var behavior = (InteractionBehavior)request.RequestInteractionBehavior;
        List<string> panelReleatedEvents = new List<string> {
            //Events.REQUEST_AGV_LOAD_SILO_ONLY,     // 料架没有料仓
            //Events.REQUEST_AGV_UNLOAD_SILO_ONLY,   // 料架的料仓，可能全空的，也可能有生料、有熟料
            Events.REQUEST_AGV_LOAD_PANEL_ONLY,
            Events.REQUEST_AGV_UNLOAD_PANEL_ONLY,
            Events.REQUEST_AGV_LOAD_PANEL_THEN_UNLOAD_PANEL,
            Events.REQUEST_AGV_UNLOAD_PANEL_THEN_LOAD_PANEL,
            Events.REQUEST_AGV_LOAD_SILO_THEN_UNLOAD_SILO,
            Events.REQUEST_AGV_UNLOAD_SILO_THEN_LOAD_SILO,
        };

        List<string> cutterReleatedEvents = new List<string> {
            Events.REQUEST_AGV_CHANGE_CUTTER
        };

        bool isPanelRequest = behavior.MaterialKind == MaterialKind.Panel;
        bool isCutterRequest = behavior.MaterialKind == MaterialKind.Cutter;
        bool isPanelSiloRequest = behavior.MaterialKind == MaterialKind.PanelSilo;
        bool isCutterSiloRequest = behavior.MaterialKind == MaterialKind.CutterSilo;

        if (isPanelRequest && panelReleatedEvents.Contains(request.EventId))
        {
            return (IDeviceEventDelegator)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(PanelRequestEventDelegator));
        }
        else if (isCutterRequest && cutterReleatedEvents.Contains(request.EventId))
        {
            return (IDeviceEventDelegator)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(CutterRequestEventDelegator));
        }
        else if (isPanelSiloRequest)
        {
            return (IDeviceEventDelegator)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(PanelSiloRequestEventDelegator));
        }
        else if (isCutterSiloRequest)
        {
            return (IDeviceEventDelegator)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(CutterSiloRequestEventDelegator));
        }

        return (IDeviceEventDelegator)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(DefaultRequestEventDelegator)); ;
    }
}
