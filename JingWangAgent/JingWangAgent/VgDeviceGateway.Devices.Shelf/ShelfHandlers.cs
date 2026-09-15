using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Shelf;

public class ShelfHandlers
{
    public static void CreateShelfHandler<T>(T device)
        where T : SiloShelf
    {
        var pinType = device.DeviceDescriptor.Extra.ContainsKey("ShelfKind") ? device.DeviceDescriptor.Extra["ShelfKind"].ToStr() : string.Empty;
        switch (pinType)
        {
            case "ChongDa":
                device.PropertyContainer.AddHandler<SiloShelfPropertyHandler, T>(device);
                device.AlarmContainer.AddHandler<SiloShelfAlarmHandler, T>(device);
                device.EventContainer.AddHandler<SiloShelfEventHandler, T>(device);
                device.StateContainer.AddHandler<SiloShelfStateHandler, T>(device);
                device.AGVToShelfSiloShelfLoadPolicy = device.ObjectFactory.CreateObject<AGVToSiloShelf_LoadMaterial_SiloShelf_InteractionPolicy>(device);
                device.AGVToShelfSiloShelfUnloadPolicy = device.ObjectFactory.CreateObject<AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy>(device);
                break;
            // TODO: 添加景旺
            //case "Kinwong":
            //    //device.PropertyContainer.AddHandler<SiloShelfPropertyHandler, T>(device);
            //    //device.AlarmContainer.AddHandler<SiloShelfAlarmHandler, T>(device);
            //    //device.EventContainer.AddHandler<SiloShelfEventHandler, T>(device);
            //    //device.StateContainer.AddHandler<SiloShelfStateHandler, T>(device);
            //    //device.AGVToShelfSiloShelfLoadPolicy = device.ObjectFactory.CreateObject<AGVToSiloShelf_LoadMaterial_SiloShelf_InteractionPolicy>(device);
            //    //device.AGVToShelfSiloShelfUnloadPolicy = device.ObjectFactory.CreateObject<AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy>(device);

            //    device.PropertyContainer.AddHandler<HikPropertyHandler, T>(device);
            //    device.AlarmContainer.AddHandler<HikAlarmHandler, T>(device);
            //    device.EventContainer.AddHandler<HikEventHandler, T>(device);
            //    device.StateContainer.AddHandler<HikStateHandler, T>(device);
            //    device.AGVToShelfSiloShelfLoadPolicy = device.ObjectFactory.CreateObject<AGVToSiloShelf_LoadMaterial_SiloShelf_InteractionPolicy>(device);
            //    device.AGVToShelfSiloShelfUnloadPolicy = device.ObjectFactory.CreateObject<AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy>(device);

            //    break;

            default:
                device.PropertyContainer.AddHandler<SiloShelfPropertyHandler, T>(device);
                device.AlarmContainer.AddHandler<SiloShelfAlarmHandler, T>(device);
                device.EventContainer.AddHandler<SiloShelfEventHandler, T>(device);
                device.StateContainer.AddHandler<SiloShelfStateHandler, T>(device);
                device.AGVToShelfSiloShelfLoadPolicy = device.ObjectFactory.CreateObject<AGVToSiloShelf_LoadMaterial_SiloShelf_InteractionPolicy>(device);
                device.AGVToShelfSiloShelfUnloadPolicy = device.ObjectFactory.CreateObject<AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy>(device);
                break;
        }
    }
}
