
using Microsoft.Extensions.Hosting;
using VgAutoDrill.Central.Core.Mes.Model;

namespace VgAutoDrill.Central.Core.Manager;

public interface IAlarmLogManager : IHostedService
{
    Task<bool> TryResetAlarm(string code);
    Task AddLogAsync(AlarmLog log);
    Task Refresh();
    Task<bool> ResetAlarm(string code);
}
