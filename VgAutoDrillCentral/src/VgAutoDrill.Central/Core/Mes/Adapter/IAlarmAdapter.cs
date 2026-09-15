using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public interface IAlarmAdapter
{
    Task<List<AlarmSetting>> LoadAlarmSettings();

    Task<List<AlarmLog>> LoadUnhandledAlarmLogs();
    Task<bool> TryHandle(long syncId);
    Task<bool> PersisitAlarmLog(List<AlarmLog> alarmLogs);
    /// <summary>
    /// 保存告警信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<DeviceAlarmReportResponse> Save(DeviceAlarmReportRequest request);
    Task<AlarmLog> GetAlarm(long syncId);

    Task<bool> SetHanled(List<string> alarmCodes);

}
