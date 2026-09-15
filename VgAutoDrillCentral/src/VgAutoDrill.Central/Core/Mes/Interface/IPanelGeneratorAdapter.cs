using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Mes;

public interface IPanelGeneratorAdapter
{
    Task<List<Fundation.Iot.Models.Panel>> GetNextPanelNumber(GetNextPanelRequest request);

    Task<List<Fundation.Iot.Models.Panel>> GetDrillNextPanelNumber(GetDrillNextPanelRequest request);

    Task<LoadSiloPanelResponse> LoadSiloPanelsInfo(GetNextPanelRequest request);
}
