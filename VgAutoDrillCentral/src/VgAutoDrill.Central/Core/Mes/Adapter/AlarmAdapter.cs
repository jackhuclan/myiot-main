using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmType;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

/// <summary>
/// load setting from db
/// save alarmlog to db
/// </summary>
public class AlarmAdapter : IAlarmAdapter
{
    private readonly IMapper _mapper;
    private readonly IAlarmSettingService _alarmSettingService;
    private readonly IAlarmService _alarmService;
    private readonly IAlarmDomainService _alarmDomain;
    public AlarmAdapter(IMapper mapper,
        IAlarmService alarmService,
        IAlarmSettingService alarmSettingService,
        IAlarmDomainService alarmDomain)
    {
        _mapper = mapper;
        _alarmSettingService = alarmSettingService;
        _alarmService = alarmService;
        _alarmDomain = alarmDomain;
    }

    public async Task<List<AlarmSetting>> LoadAlarmSettings()
    {
        var result = await _alarmSettingService.GetList(new GetAlarmSettingListReq());
        return _mapper.Map<List<AlarmSettingDto>, List<AlarmSetting>>(result.Data.List);
    }

    public async Task<bool> TryHandle(long syncId)
    {
        return await _alarmService.TryHandleBySyncId(syncId);
    }

    public async Task<List<AlarmLog>> LoadUnhandledAlarmLogs()
    {
        var result = await _alarmService.GetList(new Admin.Model.ViewModels.Mes.DvAlarmRecord.GetAlarmListReq
        {
            IsHandled = false,
            PageSize = int.MaxValue,
        });

        return _mapper.Map<List<Admin.Model.ViewModels.Mes.DvAlarmRecord.AlarmDto>, List<AlarmLog>>(result.Data.List);
    }

    public async Task<AlarmLog> GetAlarm(long syncId)
    {
        var alarm = await _alarmService.FindSingleBySyncId(syncId);
        return _mapper.Map<AlarmLog>(alarm);
    }

    public async Task<bool> PersisitAlarmLog(List<AlarmLog> alarmLogs)
    {
        //todo, 写入数据库
        //todo, 改为批量 写入
        foreach (var alarmLog in alarmLogs)
        {
            var response = await _alarmService.AddData(new Admin.Model.ViewModels.Mes.DvAlarmRecord.AddOrUpdateAlarmReq
            {
                AlarmCode = alarmLog.AlarmCode,
                AlarmLevel = 2,
                AlarmName = alarmLog.AlarmName,
                AlarmTime = alarmLog.AlarmTime,
                EventId = 0,
                SyncId = alarmLog.SyncId
            });
        }

        return true;
    }

    public async Task<DeviceAlarmReportResponse> Save(DeviceAlarmReportRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.AlarmCode)) return new DeviceAlarmReportResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = "no alarm"
        };

        var result = await _alarmService.AddData(new Admin.Model.ViewModels.Mes.DvAlarmRecord.AddOrUpdateAlarmReq
        {
            AlarmCode = request.AlarmCode,
            AlarmLevel = (int)request.AlarmLevel,
            AlarmName = request.AlarmName,
            AlarmTime = request.AlarmTime,
            AlarmContent = request.AlarmContent,
            EventId = 0,
        });

        return new DeviceAlarmReportResponse
        {
            Code = result == null || string.IsNullOrEmpty(result.Message) ? ErrorCodes.Sys.SUCCESS : ErrorCodes.Sys.FAIL,
            Message = result == null || string.IsNullOrEmpty(result.Message) ? "save DeviceAlarmReportRequest failed" : result.Message
        };
    }

    public async Task<bool> SetHanled(List<string> alarmCodes)
    {
        return await _alarmDomain.UpdateAsync(t => new VgAutoDrill.Admin.Model.Entites.Mes.Alarm() { HandledTime = DateTime.Now, IsHandled = true, ModifyTime = DateTime.Now }
                                                , p => alarmCodes.Contains(p.AlarmCode) && p.IsHandled == false);
    }
}
