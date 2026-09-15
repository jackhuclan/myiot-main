using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Mes.Interface;

public interface ISiloAdapter
{
    Task<List<SiloDto>> GetSilos();
    Task<SiloStatus> GetSiloStatus(string siloCode);
    Task<List<Panel>> GetSiloPanels(string siloCode);
    Task<List<Panel>> GetSiloPanelsByDevice(string deviceId);
    Task<List<Panel>> GetSiloPanelsByDrillDevice(string deviceId, List<int> layers);
}
