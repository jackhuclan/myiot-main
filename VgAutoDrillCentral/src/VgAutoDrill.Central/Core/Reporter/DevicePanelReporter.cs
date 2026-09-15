using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter;

internal class DevicePanelReporter : IDevicePanelReporter
{
    private readonly ILogger<DevicePanelReporter> _logger;
    private readonly IDeviceManager _deviceManager;
    private readonly IScheduleTaskManager _scheduleTaskManager;

    public DevicePanelReporter(IDeviceManager deviceManager,
        IScheduleTaskManager scheduleTaskManager,
        ILoggerFactory loggerFactory)
    {
        _deviceManager = deviceManager;
        _scheduleTaskManager = scheduleTaskManager;
        _logger = loggerFactory.CreateLogger<DevicePanelReporter>();
    }

    public async Task<DevicePanelChangedResponse> Report(DevicePanelChangedRequest? request)
    {
        ArgumentNullException.ThrowIfNull(request);
        _logger.LogTrace(request.ToJson());

        var deviceProxy = _deviceManager.GetOnlineDevice(request.DeviceId);
        if (deviceProxy == null)
        {
            return new DevicePanelChangedResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
            };
        }

        if (deviceProxy.IsDrill
            && _scheduleTaskManager.TryGetNotStartedScheduleByLocationCode(request.DeviceId, out var drillSchedule)
            && drillSchedule != null)
        {
            await _scheduleTaskManager.CancelSingleSchedule(drillSchedule.Code, true, "钻机板料已变更");
        }

        await deviceProxy.SetPanels(request.PanelList);

        return new DevicePanelChangedResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = string.Empty
        };
    }
}
