using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Central.Core.Schedule.Logic;

public class TimeoutScheduleTaskLogic
{
    private readonly IObjectFactory _objectFactory;

    public TimeoutScheduleTaskLogic(IObjectFactory objectFactory)
    {
        _objectFactory = objectFactory;
    }

    public Task OnTimeout(ScheduleTask scheduleTask)
    {
        return Task.CompletedTask;
    }
}
