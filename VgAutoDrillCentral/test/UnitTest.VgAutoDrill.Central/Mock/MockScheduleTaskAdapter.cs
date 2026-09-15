using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Central.Core;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Model;

namespace UnitTest.VgAutoDrill.Central.Mock;

internal class MockScheduleTaskAdapter : IScheduleTaskAdapter
{
    public Task BatchCancelSchedule(List<long> scheduleIds) => throw new NotImplementedException();

    public Task<string> CancelSingleSchedule(string traceId, bool isForced = false, string reason = "") => throw new NotImplementedException();

    public Task<ScheduleTaskWithRequest> FindScheduleByTraceId(string traceId) => throw new NotImplementedException();

    public Task<string> FindSingleBySubDeviceCode(string subDeviceCode) => throw new NotImplementedException();

    public Task<ScheduleTaskWithRequest> FindTodoScheduleByRoutingKey(string routingKey) => throw new NotImplementedException();

    public Task<List<ScheduleTask>> GetList(QueryScheduleRequest request) => throw new NotImplementedException();

    public Task<List<ScheduleTaskWithRequest>> GetSchedule(QueryScheduleRequest request) => throw new NotImplementedException();

    public Task<bool> HasUncheduledTask(string routingKey) => throw new NotImplementedException();

    public Task<bool> HasCompletedScheduleWithNotStartedWorkTask(string taskId) => throw new NotImplementedException();

    public Task SpecifyFollowedSchedule(string currentScheduleCode, string macthedAgv, string relateCode, string message) => throw new NotImplementedException();

    public Task<int> RefreshTimeoutSchedule() => throw new NotImplementedException();

    public Task UpdateScheduleForChangingSilo(string eventTraceId, DeviceProxy agvDevice, string message) => throw new NotImplementedException();

    public Task<bool> HasUnstartedOrDoingScheduleTaskByRoutingKey(string routingKey) => throw new NotImplementedException();

    public Task<ScheduleTaskWithRequest> GetSchedule(long scheduleId) => throw new NotImplementedException();

    public Task<List<T>> GetSchedule<T>(QueryScheduleRequest request) where T : ScheduleTaskWithRequest => throw new NotImplementedException();

    Task<List<DrillScheduleTask>> IScheduleTaskAdapter.GetNotStartedDrillSchedule() => throw new NotImplementedException();

    Task<List<PinScheduleTask>> IScheduleTaskAdapter.GetNotStartedPinSchedule() => throw new NotImplementedException();

    Task<List<UnpinScheduleTask>> IScheduleTaskAdapter.GetNotStartedUnpinSchedule() => throw new NotImplementedException();

    Task<List<ShelfScheduleTask>> IScheduleTaskAdapter.GetNotStartedForkOrShelfSchedule() => throw new NotImplementedException();

    public Task<List<ScheduleTaskWithRequest>> GetNotStartedFollowedSchedule() => throw new NotImplementedException();

    public Task<List<ScheduleTaskWithRequest>> GetNotStartedSpecifiedAgvSchedule() => throw new NotImplementedException();

    public Task<ScheduleTask> FindMasterSchedule(ScheduleTask inputSchedule) => throw new NotImplementedException();

    public Task<List<DrillScheduleTask>> GetNotStartedDrillSchedule(string deviceId) => throw new NotImplementedException();

    public Task<bool> HasUnstartedOrDoingScheduleTaskByAllocatedAgv(string allocatedAgv) => throw new NotImplementedException();

    public Task AddScheduleLog(long masterId, string message) => throw new NotImplementedException();

    public Task<string> UpdateDbScheduleTaskTable(AddOrUpdateScheduleReq updateDto) => throw new NotImplementedException();

    public Task<string> AddData(AddOrUpdateScheduleReq updateDto) => throw new NotImplementedException();

    public Task UpdateAllocatedAgv(ScheduleTaskWithRequest schedule) => throw new NotImplementedException();
}
