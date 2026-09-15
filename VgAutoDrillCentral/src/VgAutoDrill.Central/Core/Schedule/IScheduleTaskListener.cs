using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Schedule;

public interface IScheduleTaskListener
{
    Task OnStatusChanged(ScheduleTask scheduleTask);
}
