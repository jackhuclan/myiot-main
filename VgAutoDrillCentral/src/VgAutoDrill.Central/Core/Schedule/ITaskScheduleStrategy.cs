namespace VgAutoDrill.Central.Core.Schedule;

public interface ITaskScheduleStrategy
{
    /// <summary>
    /// 处理调度任务
    /// </summary>
    /// <returns></returns>
    Task HandleScheduleTask();

    ///// <summary>
    ///// 根据设备的在线列表，刷新已离线的设备
    ///// </summary>
    ///// <returns></returns>
    //Task RefreshDevicesStatus();
    ///// <summary>
    ///// 处理没有在途调度记录的redis key
    ///// </summary>
    ///// <returns></returns>
    //Task RefreshTimeoutRedisKey();
    Task SetIdleAgvToRestPoint();
}
