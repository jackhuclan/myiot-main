using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot.Extensions;

public static class DeviceKindExtensions
{
    public static bool IsAuxiliary(DeviceKind deviceKind)
    {
        switch (deviceKind)
        {
            case DeviceKind.PanelSiloFork:
            case DeviceKind.PublicPanelSiloWIP:
            case DeviceKind.CutterSiloShelf:
                return true;

            default:
                return false;
        }
    }

    public static bool IsDrill(DeviceKind deviceKind)
    {
        switch (deviceKind)
        {
            case DeviceKind.CNC95Drill:
            case DeviceKind.CNC84Drill:
                return true;

            default:
                return false;
        }
    }

    public static bool IsDrill(DeviceKind? deviceKind)
    {
        switch (deviceKind)
        {
            case DeviceKind.CNC95Drill:
            case DeviceKind.CNC84Drill:
                return true;

            default:
                return false;
        }
    }

    //public static bool IsPanelSiloFork(DeviceKind deviceKind)
    //{
    //    switch (deviceKind)
    //    {
    //        case DeviceKind.PanelSiloFork:
    //            return true;
    //        default:
    //            return false;
    //    }
    //}
    public static bool IsCutterSiloShelf(DeviceKind deviceKind)
    {
        switch (deviceKind)
        {
            case DeviceKind.CutterSiloShelf:
                return true;

            default:
                return false;
        }
    }

    public static bool IsAGV(DeviceKind deviceKind)
    {
        switch (deviceKind)
        {
            case DeviceKind.BackPanelAgv:
            case DeviceKind.FrontPanelAgv:
            case DeviceKind.FrontToolAgv:
            case DeviceKind.RollerSiloAgv:
            case DeviceKind.ShelfSiloAgv:
                return true;

            default:
                return false;
        }
    }

    public static bool IsPanelAGV(DeviceKind deviceKind)
    {
        switch (deviceKind)
        {
            case DeviceKind.BackPanelAgv:
            case DeviceKind.FrontPanelAgv:
            case DeviceKind.RollerSiloAgv:
            case DeviceKind.ShelfSiloAgv:
                return true;

            default:
                return false;
        }
    }

    public static bool IsSameKind(DeviceKind deviceKind, DeviceKind currentKind)
    {
        switch (currentKind)
        {
            //case DeviceKind.AGV:
            //    return deviceKind == DeviceKind.AGV;
            case DeviceKind.CNC95Drill:
            case DeviceKind.CNC84Drill:
                return deviceKind == DeviceKind.CNC95Drill || deviceKind == DeviceKind.CNC84Drill;

            case DeviceKind.UnPin:
                return deviceKind == DeviceKind.UnPin;

            case DeviceKind.Pin:
                return deviceKind == DeviceKind.Pin;

            case DeviceKind.PublicPanelSiloWIP:
                return deviceKind == DeviceKind.PublicPanelSiloWIP;

            case DeviceKind.ProcessedTagingDesk:
                return deviceKind == DeviceKind.ProcessedTagingDesk;

            case DeviceKind.RawTagingDesk:
                return deviceKind == DeviceKind.RawTagingDesk;

            default:
                return false;
        }
    }
}
