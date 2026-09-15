using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Store;

public interface IDeviceStore
{
    Task SaveScheduleTasks(Device device);
    Task SavePayloadPanels(Device device);
    Task SavePayloadCutterTrays(Device device);
    Task LoadPayloadPanels(Device device);
    Task LoadPayloadCutterTrays(Device device);
    Task LoadScheduleTasks(Device device);
    Task SavePayloadPanels(PanelList panels);
    Task LoadPayloadPanels(string locationCode, PanelList panels);
}
