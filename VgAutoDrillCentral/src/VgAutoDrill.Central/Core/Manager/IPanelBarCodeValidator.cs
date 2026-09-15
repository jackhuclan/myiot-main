using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Manager;

public interface IPanelBarCodeValidator
{
    Task<bool> ValidatePanelBarCodeAsync(ScheduleTaskWithRequest? schedule);
}
