using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public interface IDeviceManager
{
    /// <summary>
    /// 所有钻机
    /// </summary>
    IReadOnlyList<Drill> Drills { get; }

    /// <summary>
    /// 所有agv
    /// </summary>
    IReadOnlyList<Agv> Agvs { get; }

    /// <summary>
    /// 所有板料agv
    /// </summary>
    IReadOnlyList<PanelAgv> PanelAgvs { get; }

    /// <summary>
    /// 所有上下料agv
    /// </summary>
    IReadOnlyList<BackPanelAgv> BackPanelAgvs { get; }

    /// <summary>
    /// 所有转运agv
    /// </summary>
    IReadOnlyList<TransferSiloAgv> TransferSiloAgvs { get; }

    /// <summary>
    /// 所有架子，包括料架和插齿
    /// </summary>
    IReadOnlyList<PanelSiloRack> PanelSiloRacks { get; }

    /// <summary>
    /// 所有料架
    /// </summary>
    IReadOnlyList<PanelSiloShelf> PanelSiloShelfs { get; }

    /// <summary>
    /// 所有插齿
    /// </summary>
    IReadOnlyList<PanelSiloFork> PanelSiloForks { get; }

    /// <summary>
    /// 所有pin
    /// </summary>
    IReadOnlyList<Pin> Pins { get; }

    /// <summary>
    /// 所有unpin
    /// </summary>
    IReadOnlyList<UnPin> UnPins { get; }

    Task AddOrUpdateDevice(DeviceProxy deviceProxy);

    Task OfflineDevice(string deviceId);

    Task RemoveDeviceByClient(string clientId);

    /// <summary>
    /// 获取所有设备, 包括status==offline
    /// </summary>
    /// <param name="deviceId"></param>
    /// <returns></returns>
    DeviceProxy? GetOnlineDevice(string? deviceId);

    bool TryGetLocalDevice<T>(string deviceId, out T? deviceProxy) where T : DeviceProxy;

    bool TryGetOnlineDevice<T>(string? deviceId, out T? deviceProxy) where T : DeviceProxy;

    /// <summary>
    /// 获取在线设备，status!=offline
    /// </summary>
    IReadOnlyList<DeviceProxy> OnlineDevices { get; }

    IReadOnlyList<Drill> OnlineDrills { get; }
    IReadOnlyList<DeviceProxy> Devices { get; }

    /// <summary>
    /// 获取有处理该产品状态能力的agv
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    List<Agv> GetCapableAgvs(ProductStatus status);

    /// <summary>
    /// 得到空闲的后上料agv
    /// </summary>
    /// <param name="routeCode">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<BackPanelAgv> GetIdleBackPanelAgvs(string routeCode = "");

    /// <summary>
    /// 得到空闲的前上料agv
    /// </summary>
    /// <param name="routeCode">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<FrontPanelAgv> GetIdleFrontPanelAgvs(string routeCode = "");

    /// <summary>
    /// 得到空闲的转运agv
    /// </summary>
    /// <param name="routeCode">工艺路线</param>
    /// <returns></returns>
    IReadOnlyList<TransferSiloAgv> GetIdleTransferSiloAgvs(string routeCode = "");

    /// <summary>
    /// 尝试获取一个空闲的转运agv
    /// </summary>
    /// <param name="routeCode">工艺路线</param>
    /// <param name="panelAgv"></param>
    /// <returns></returns>
    bool TryGetIdleTransferSiloAgv(string routeCode, out TransferSiloAgv? panelAgv);

    /// <summary>
    /// 尝试获取一个后上下料agv
    /// </summary>
    /// <param name="routeCode">工艺路线</param>
    /// <param name="panelAgv"></param>
    /// <returns></returns>
    bool TryGetIdleBackPanelAgv(string routeCode, out BackPanelAgv? panelAgv);

    ///// <summary>
    ///// 获取同类型的在线设备
    ///// </summary>
    ///// <param name="deviceKind"></param>
    ///// <returns></returns>
    //List<string> GetOnlineSameKindDevices(DeviceKind deviceKind);

    /// <summary>
    /// 读取调度配置中的AGV关联的工艺路线
    /// </summary>
    /// <returns></returns>
    Task RefreshAgvRouteCodes();

    Task RefreshDrillRouteCodes();

    bool TryGetOnlineDrill(string deviceId, out Drill? deviceProxy);
}
