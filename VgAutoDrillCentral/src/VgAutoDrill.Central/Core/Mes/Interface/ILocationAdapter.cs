using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Central.Core.Mes.Interface;

public interface ILocationAdapter
{
    /// <summary>
    /// 从数据库获取所有库位
    /// </summary>
    /// <returns></returns>
    Task<List<Location>> GetLocations();

    Task<Location> GetLocation(string locationCode);
    Task TraceLocationPanels(PanelList panelList, string changeReason, long scheduleId = 0, long transferSiloTaskId = 0);
}
