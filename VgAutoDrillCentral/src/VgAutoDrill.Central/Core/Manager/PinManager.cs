using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Manager;

public class PinManager : IPinManager
{
    private readonly IScheduleTaskManager _scheduleTaskManager;
    private readonly IDeviceManager _deviceManager;

    public PinManager(IServiceProvider serviceProvider)
    {
        _scheduleTaskManager = serviceProvider.GetRequiredService<IScheduleTaskManager>();
        _deviceManager = serviceProvider.GetRequiredService<IDeviceManager>();
    }

    public IReadOnlyList<Pin> Pins => _deviceManager.Pins;

    public IReadOnlyList<Location> Locations => Pins.SelectMany(x => x.Locations).Where(x => x != null).ToList().AsReadOnly()!;

    public IReadOnlyList<PinScheduleTask> NotStartedSchedules => _scheduleTaskManager.NotStartedPinSchedules;

    /// <summary>
    /// 存在未完成的库位
    /// </summary>
    public IReadOnlyList<Location> NotStartedLocations => Locations.Where(x => x != null && x.HasNotStartedSchedule)
        .ToList().AsReadOnly()!;

    /// <summary>
    /// 存在未分配agv的库位
    /// </summary>
    public IReadOnlyList<Location> NotAllocatedLocations => Locations.Where(x => x != null && x.HasNotAllocatedSchedule)
        .ToList().AsReadOnly()!;

    /// <summary>
    /// 所有pin上的板料汇总
    /// </summary>
    /// <returns></returns>
    public int PanelCount()
    {
        return Locations.Sum(s => s == null ? 0 : s.PanelCount());
    }

    /// <summary>
    /// 某个pin上的板料汇总
    /// </summary>
    /// <param name="deviceId">pin的设备id</param>
    /// <returns></returns>
    public int PanelCount(string deviceId)
    {
        return Locations.Sum(s => s == null ? 0 : s.PanelCount());
    }

    /// <summary>
    /// 所有pin上的某些状态的板料汇总
    /// </summary>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int PanelCount(List<ProductStatus> productStatuses)
    {
        return Locations.Sum(s => s == null ? 0 : s.PanelCount(productStatuses));
    }

    /// <summary>
    /// 某个pin上的某些状态的板料汇总
    /// </summary>
    /// <param name="deviceId">pin的设备id</param>
    /// <param name="productStatuses">物料状态</param>
    /// <returns></returns>
    public int PanelCount(string deviceId, List<ProductStatus> productStatuses)
    {
        return Locations.Sum(s => s == null ? 0 : s.PanelCount(productStatuses));
    }

    public Task Fetch()
    {
        return Task.CompletedTask;
    }
}
