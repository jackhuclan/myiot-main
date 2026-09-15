using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Mes.Adapter;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Reporter;

public class DeviceAlarmReporter : IDeviceAlarmReporter
{
    private readonly IAlarmAdapter _alarmAdapter;
    private readonly ILogger<DeviceAlarmReporter> _logger;

    public DeviceAlarmReporter(IAlarmAdapter alarmAdapter, ILogger<DeviceAlarmReporter> logger)
    {
        _alarmAdapter = alarmAdapter;
        _logger = logger;
    }

    public Task<DeviceAlarmReportResponse> Report(DeviceAlarmReportRequest? request)
    {
        if (request == null)
        {
            return Task.FromResult(new DeviceAlarmReportResponse
            {
                Code = ErrorCodes.Sys.FAIL,
                Message = $"{nameof(DeviceAlarmReportRequest)} is null"
            });
        }

        return _alarmAdapter.Save(request);
    }
}
