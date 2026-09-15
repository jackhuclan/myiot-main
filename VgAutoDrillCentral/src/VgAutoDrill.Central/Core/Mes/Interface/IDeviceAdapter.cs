using System.Collections.Concurrent;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Central.Core.Mes.Interface;

public interface IDeviceAdapter
{
    Task PersistPanels(string productId, string deviceId, PanelList panels);

    Task<DeviceDto> GetDevice(string deviceId);

    Task<List<string>> GetRouteCodes(string deviceId);

    Task UpdateStatus(DeviceProxy deviceProxy);

    Task<List<DeviceRouteCodesPair>> GetDrillRouteCodes();

    Task<List<DeviceRouteCodesPair>> GetAgvRouteCodes();

    Task PersistCutters(string productId, string deviceId, CutterTrays payloadCutterTrays);

    Task PersistProperties(string productId, string deviceId, ConcurrentDictionary<string, object> Properties);

    Task ReCordDrillRateFactor(string deviceId, DrillRateFactorReason reason, DateTime startTime, DateTime endTime);

    Task BulkReCordDrillRateFactor(List<ReCordDrillRateFactorDto> reqs);
}
