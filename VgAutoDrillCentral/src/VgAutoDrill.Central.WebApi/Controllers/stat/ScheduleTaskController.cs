using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Manager;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/schedule")]
public class ScheduleTaskController : ControllerBase
{
    private readonly IScheduleTaskManager _scheduleTaskManager;

    public ScheduleTaskController(IScheduleTaskManager scheduleTaskManager)
    {
        _scheduleTaskManager = scheduleTaskManager;
    }

    [HttpGet]
    public object? Index(long? id)
    {
        if (!id.HasValue)
        {
            return new
            {
                _scheduleTaskManager.Tasks,
                _scheduleTaskManager.NotStartedSchedules,
                _scheduleTaskManager.NotStartedPinSchedules,
                _scheduleTaskManager.NotStartedShelfSchedules,
                _scheduleTaskManager.NotStartedDrillSchedules,
                _scheduleTaskManager.NotStartedForkSchedules,
                _scheduleTaskManager.NotStartedUnpinSchedules,
            };
        }

        return _scheduleTaskManager.Tasks.FirstOrDefault(x => x.Id == id);
    }
}
