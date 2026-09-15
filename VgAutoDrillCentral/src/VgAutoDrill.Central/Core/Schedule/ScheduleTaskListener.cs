using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Schedule;

public class ScheduleTaskListener : IScheduleTaskListener
{
    private readonly IServiceProvider _serviceProvider;

    public ScheduleTaskListener(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnStatusChanged(ScheduleTask scheduleTask)
    {
        //scheduleTask.Appointed = scheduleTask.IsCompleted;

        var _scheduleTaskAdapter = _serviceProvider.GetRequiredService<IScheduleTaskAdapter>();
        var distributedCache = _serviceProvider.GetRequiredService<IDistributedCache>();
        if (scheduleTask.UpdateSource == ScheduledTaskStatusUpdateSource.FromMemory)
        {
            var updateDto = new AddOrUpdateScheduleReq
            {
                Code = scheduleTask.Code,
                ScheduledTaskStatus = scheduleTask.ScheduledTaskStatus,
            };

            var response = await _scheduleTaskAdapter.UpdateDbScheduleTaskTable(updateDto);
            if (!string.IsNullOrEmpty(response))
            {
                scheduleTask.UpdateSource = ScheduledTaskStatusUpdateSource.FromDB;
            }
        }

        if (scheduleTask.IsCompleted)
        {
            await distributedCache.RemoveAsync(scheduleTask.RoutingKey);
        }
    }
}
