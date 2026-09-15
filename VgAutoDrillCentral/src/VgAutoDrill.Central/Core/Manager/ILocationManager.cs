using System.Collections.ObjectModel;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public interface ILocationManager
{
    IReadOnlyList<Location> Locations { get; }

    /// <summary>
    /// 没有被预约的，有板料，有调度，并且调度没有开始的, 在线的库位
    /// </summary>
    IReadOnlyList<Location> AvailableLocations { get; }

    /// <summary>
    /// 在线的库位
    /// </summary>
    IReadOnlyList<Location> OnlineLocations { get; }

    /// <summary>
    /// 已经被分配的库位
    /// </summary>
    IReadOnlyList<Location> AllocatedLocations { get; }

    /// <summary>
    /// 正在被agv执行任务的库位
    /// </summary>
    IReadOnlyList<Location> RunningLocations { get; }

    Task Refresh();

    /// <summary>
    /// 库位数
    /// </summary>
    int LocationCount();

    /// <summary>
    /// 统计包含所给物料状态的库位数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    int LocationCount(IReadOnlyList<ProductStatus> productStatuses);

    bool TryAddOrUpdateLocation(Location location);

    bool TryGetLocation(string locationCode, out Location? location);

    /// <summary>
    /// 是否存在locationCode的库位
    /// </summary>
    /// <param name="locationCode"></param>
    /// <returns></returns>
    bool HasLocation(string locationCode);

    IReadOnlyList<Panel> GetDevicePanels(string deviceId);

    IReadOnlyList<Panel> GetLocationPanels(string locationCode);

    Location? GetLocation(string locationCode);

    public IReadOnlyList<Partition> Partitions { get; }

    /// <summary>
    /// 对钻机不适用,因为一个钻机有多个position
    /// </summary>
    /// <param name="deviceId">设备id</param>
    /// <param name="position">库位序号从1开始</param>
    /// <returns></returns>
    Location? GetLocation(string deviceId, int position);

    /// <summary>
    /// 获取设备上的库位
    /// </summary>
    /// <param name="deviceId">设备id</param>
    /// <returns></returns>
    IReadOnlyList<Location> GetLocations(string deviceId);

    /// <summary>
    /// 统计包含所给物料状态的库位数量
    /// </summary>
    /// <param name="productStatus">所给物料状态</param>
    /// <returns></returns>
    int LocationCount(ProductStatus productStatus);

    Partition? GetPartition(string locationCode);

    /// <summary>
    /// 空层数量
    /// </summary>
    int GetEmptyLayerCount();

    /// <summary>
    /// 空层数量
    /// </summary>
    /// <param name="siloCode">料仓的设备code</param>
    int GetEmptyLayerCount(string siloCode);

    /// <summary>
    /// 空层数量
    /// </summary>
    /// <param name="deviceId">目标设备id</param>
    int GetDeviceEmptyLayerCount(string deviceId);

    /// <summary>
    /// 空层数量
    /// </summary>
    /// <param name="deviceId">目标设备id</param>
    /// <param name="siloCode">料仓的设备code</param>
    int GetDeviceEmptyLayerCount(string deviceId, string siloCode);

    /// <summary>
    /// 车间所有料仓数（料架，插齿，中转位，agv等所有能放料仓的位置上）
    /// </summary>
    /// <returns></returns>
    int SiloCount();

    /// <summary>
    /// 车间所有包含该料号的料仓数
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    int SiloCountOfContains(string itemCode);

    /// <summary>
    /// 统计包含所给物料状态的料仓数量
    /// </summary>
    /// <param name="productStatuses">所给物料状态</param>
    /// <returns></returns>
    int SiloCount(IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 所有料仓上的板料汇总
    /// </summary>
    /// <returns></returns>
    int PanelCount();

    /// <summary>
    /// 某个料仓上的板料汇总，不同设备上重复的料仓号可能会造成统计错误
    /// </summary>
    /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
    /// <returns></returns>
    int PanelCount(string siloCode);

    /// <summary>
    /// 所有料仓上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatus">物料状态</param>
    /// <returns></returns>
    int PanelCount(ProductStatus productStatus);

    /// <summary>
    /// 所有料仓上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    /// 某个料仓上的某些状态的板料汇总，不同设备上重复的料仓号可能会造成统计错误
    /// </summary>
    /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    int PanelCount(string siloCode, IReadOnlyList<ProductStatus> productStatuses);

    /// <summary>
    ///
    /// </summary>
    /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
    /// <returns></returns>
    int SiloCount(string siloCode);

    /// <summary>
    ///
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
    /// <param name="productStatus"></param>
    /// <returns></returns>
    int PanelCount(string deviceId, string siloCode, ProductStatus productStatus);

    /// <summary>
    ///
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
    /// <param name="productStatuses"></param>
    /// <returns></returns>
    int PanelCount(string deviceId, string siloCode, ReadOnlyCollection<ProductStatus> productStatuses);

    /// <summary>
    ///
    /// </summary>
    /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
    /// <param name="productStatus"></param>
    /// <returns></returns>
    int PanelCount(string siloCode, ProductStatus productStatus);

    /// <summary>
    ///
    /// </summary>
    /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
    /// <param name="location"></param>
    /// <returns></returns>
    bool TryFindLocationWithSiloCode(string siloCode, out Location? location);

    /// <summary>
    /// 根据库位变化创建新的库位
    /// </summary>
    /// <param name="locationCode"></param>
    /// <returns></returns>
    Location Create(string locationCode);
}
