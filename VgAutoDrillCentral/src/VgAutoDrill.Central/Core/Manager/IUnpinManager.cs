using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public interface IUnpinManager
{
    /// <summary>
    /// 所有unpin
    /// </summary>
    IReadOnlyList<UnPin> UnPins { get; }
    IReadOnlyList<UnpinScheduleTask> NotStartedSchedules { get; }
    IReadOnlyList<Location> Locations { get; }
    /// <summary>
    /// 存在未完成的库位
    /// </summary>
    IReadOnlyList<Location> NotStartedLocations { get; }

    /// <summary>
    /// 存在未分配agv的库位
    /// </summary>
    IReadOnlyList<Location> NotAllocatedLocations { get; }
    /// <summary>
    /// 所有unpin上的板料汇总
    /// </summary>
    /// <returns></returns>
    int PanelCount();

    /// <summary>
    /// 某个unpin上的板料汇总
    /// </summary>
    /// <param name="deviceId">unpin的设备id</param>
    /// <returns></returns>
    int PanelCount(string deviceId);

    /// <summary>
    /// 所有unpin上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(List<ProductStatus> productStatuses);

    /// <summary>
    /// 某个unpin上的某些状态的板料汇总
    /// </summary>
    /// <param name="deviceId">unpin的设备id</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(string deviceId, List<ProductStatus> productStatuses);

    Task Fetch();
}
