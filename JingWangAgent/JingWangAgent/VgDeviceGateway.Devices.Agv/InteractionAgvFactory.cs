using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Agv.AgvDevice;
using VgDeviceGateway.Devices.Agv.Chassis;
using VgDeviceGateway.Devices.Agv.EventHandler;
using VgDeviceGateway.Devices.Agv.InteractionLoad;
using VgDeviceGateway.Devices.Agv.InteractionUnload;
using VgDeviceGateway.Devices.Agv.PropertyHandler;
using VgDeviceGateway.Devices.Agv.StateHandler;

namespace VgDeviceGateway.Devices.Agv
{
    public class InteractionAgvFactory
    {
        public static Dictionary<string, IAgvUnloadInteraction> CreateDrillInteractiveUnloadAllObject(DefaultAgv defaultAgv)
        {
            //交互类型 来自Request.CallerRequestMaterialKind;
            Dictionary<string, IAgvUnloadInteraction> InteractionAgvs = new Dictionary<string, IAgvUnloadInteraction>();
            InteractionAgvs.TryAdd("BackPanelAgvUnloadPanel", defaultAgv.ObjectFactory.CreateObject<BackPanelAgvUnloadPanel>(defaultAgv));
            InteractionAgvs.TryAdd("BackPanelAgvUnloadPanelSilo", defaultAgv.ObjectFactory.CreateObject<BackPanelAgvUnloadSilo>(defaultAgv));
            InteractionAgvs.TryAdd("FrontPanelAgvUnloadPanel", defaultAgv.ObjectFactory.CreateObject<FrontPanelAgvUnloadPanel>(defaultAgv));
            InteractionAgvs.TryAdd("FrontToolAgvUnloadCutter", defaultAgv.ObjectFactory.CreateObject<FrontToolAgvUnloadCutter>(defaultAgv));
            InteractionAgvs.TryAdd("FrontToolAgvUnloadCutterSilo", defaultAgv.ObjectFactory.CreateObject<FrontToolAgvUnloadSilo>(defaultAgv));
            InteractionAgvs.TryAdd("RollerSiloAgvUnloadPanelSilo", defaultAgv.ObjectFactory.CreateObject<RollerSiloAgvUnloadSilo>(defaultAgv));
            InteractionAgvs.TryAdd("ShelfSiloAgvUnloadPanelSilo", defaultAgv.ObjectFactory.CreateObject<ShelfSiloAgvUnloadSilo>(defaultAgv));
            return InteractionAgvs;
        }

        public static Dictionary<string, IAgvLoadInteraction> CreateDrillInteractiveLoadAllObject(DefaultAgv defaultAgv)
        {
            Dictionary<string, IAgvLoadInteraction> InteractionAgvs = new Dictionary<string, IAgvLoadInteraction>();
            InteractionAgvs.TryAdd("BackPanelAgvLoadPanel", defaultAgv.ObjectFactory.CreateObject<BackPanelAgvLoadPanel>(defaultAgv));
            InteractionAgvs.TryAdd("BackPanelAgvLoadPanelSilo", defaultAgv.ObjectFactory.CreateObject<BackPanelAgvLoadSilo>(defaultAgv));
            InteractionAgvs.TryAdd("FrontPanelAgvLoadPanel", defaultAgv.ObjectFactory.CreateObject<FrontPanelAgvLoadPanel>(defaultAgv));
            InteractionAgvs.TryAdd("FrontToolAgvLoadCutter", defaultAgv.ObjectFactory.CreateObject<FrontToolAgvLoadCutter>(defaultAgv));
            InteractionAgvs.TryAdd("FrontToolAgvLoadCutterSilo", defaultAgv.ObjectFactory.CreateObject<FrontToolAgvLoadSilo>(defaultAgv));
            InteractionAgvs.TryAdd("RollerSiloAgvLoadPanelSilo", defaultAgv.ObjectFactory.CreateObject<RollerSiloAgvLoadSilo>(defaultAgv));
            InteractionAgvs.TryAdd("ShelfSiloAgvLoadPanelSilo", defaultAgv.ObjectFactory.CreateObject<ShelfSiloAgvLoadSilo>(defaultAgv));
            return InteractionAgvs;
        }

        public static IAgvStateHandler CreateAgvStateHandler(DefaultAgv defaultAgv)
        {
            switch (defaultAgv.DeviceDescriptor.DeviceKind)
            {
                case DeviceKind.RollerSiloAgv:
                    return defaultAgv.ObjectFactory.CreateObject<RollerSiloAgvStateHandler>(defaultAgv);

                case DeviceKind.BackPanelAgv:
                    return defaultAgv.ObjectFactory.CreateObject<BackPanelAgvStateHandler>(defaultAgv);

                case DeviceKind.FrontPanelAgv:
                    return defaultAgv.ObjectFactory.CreateObject<FrontPanelAgvStateHandler>(defaultAgv);

                case DeviceKind.FrontToolAgv:
                    return defaultAgv.ObjectFactory.CreateObject<FrontToolAgvStateHandler>(defaultAgv);

                case DeviceKind.ShelfSiloAgv:
                    return defaultAgv.ObjectFactory.CreateObject<ShelfSiloAgvStateHandler>(defaultAgv);

                default:
                    break;
            }
            return null;
        }

        public static IAgvPropertyHandler CreateAgvPropertyHandler(DefaultAgv defaultAgv)
        {
            switch (defaultAgv.DeviceDescriptor.DeviceKind)
            {
                case DeviceKind.RollerSiloAgv:
                    return defaultAgv.ObjectFactory.CreateObject<RollerSiloAgvPropertyHandler>(defaultAgv);

                case DeviceKind.BackPanelAgv:
                    return defaultAgv.ObjectFactory.CreateObject<BackPanelAgvPropertyHandler>(defaultAgv);

                case DeviceKind.FrontPanelAgv:
                    return defaultAgv.ObjectFactory.CreateObject<FrontPanelAgvPropertyHandler>(defaultAgv);

                case DeviceKind.FrontToolAgv:
                    return defaultAgv.ObjectFactory.CreateObject<FrontToolAgvPropertyHandler>(defaultAgv);

                case DeviceKind.ShelfSiloAgv:
                    return defaultAgv.ObjectFactory.CreateObject<ShelfSiloAgvPropertyHandler>(defaultAgv);

                default:
                    break;
            }
            return null;
        }

        public static IAgvEventHandler CreateAgvEventHandler(DefaultAgv defaultAgv)
        {
            switch (defaultAgv.DeviceDescriptor.DeviceKind)
            {
                case DeviceKind.RollerSiloAgv:
                    return defaultAgv.ObjectFactory.CreateObject<RollerSiloAgvvEventHandler>(defaultAgv);

                case DeviceKind.BackPanelAgv:
                    return defaultAgv.ObjectFactory.CreateObject<BackPanelAgvEventHandler>(defaultAgv);

                case DeviceKind.FrontPanelAgv:
                    return defaultAgv.ObjectFactory.CreateObject<FrontPanelAgvEventHandler>(defaultAgv);

                case DeviceKind.FrontToolAgv:
                    return defaultAgv.ObjectFactory.CreateObject<FrontToolAgvEventHandler>(defaultAgv);

                case DeviceKind.ShelfSiloAgv:
                    return defaultAgv.ObjectFactory.CreateObject<ShelfSiloAgvEventHandler>(defaultAgv);

                default:
                    break;
            }
            return null;
        }

        public static IAgvDevice CreatetAgvDevice(DefaultAgv defaultAgv)
        {
            switch (defaultAgv.DeviceDescriptor.DeviceKind)
            {
                case DeviceKind.RollerSiloAgv:
                    return defaultAgv.ObjectFactory.CreateObject<RollerSiloAgv>(defaultAgv);

                case DeviceKind.BackPanelAgv:
                    return defaultAgv.ObjectFactory.CreateObject<BackPanelAgv>(defaultAgv);

                case DeviceKind.FrontPanelAgv:
                    return defaultAgv.ObjectFactory.CreateObject<FrontPanelAgv>(defaultAgv);

                case DeviceKind.FrontToolAgv:
                    return defaultAgv.ObjectFactory.CreateObject<FrontToolAgv>(defaultAgv);

                case DeviceKind.ShelfSiloAgv:
                    return defaultAgv.ObjectFactory.CreateObject<ShelfSiloAgv>(defaultAgv);

                default:
                    break;
            }
            return null;
        }

        public static IAgvChassis CreatetAgvChassis(IObjectFactory objectFactory, string type)
        {
            switch (type)
            {
                case "AjwRobot":
                    return objectFactory.CreateObject<AjwRobotChassis>();

                case "HikRobot":
                    return objectFactory.CreateObject<HikRobotChassis>();

                case "StdRobot":
                    return objectFactory.CreateObject<StdRobotChassis>();

                default:
                    break;
            }
            return null;
        }
    }
}