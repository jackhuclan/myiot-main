using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes.Interface;
using VgAutoDrill.Central.Core.Reporter.Event;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Reporter;

internal class DeviceStatusReporter : IDeviceStatusReporter
{
    private readonly ILogger<DeviceEventReporter> _logger;
    private readonly IDeviceManager _deviceHolder;
    private readonly ILocationAdapter _locationAdapter;
    private readonly ISysConfigManager _sysConfigManager;

    public DeviceStatusReporter(IDeviceManager deviceHolder,
        ILocationAdapter locationAdapter,
        ISysConfigManager sysConfigManager,
        ILoggerFactory loggerFactory)
    {
        _deviceHolder = deviceHolder;
        _logger = loggerFactory.CreateLogger<DeviceEventReporter>();
        _locationAdapter = locationAdapter;
        _sysConfigManager = sysConfigManager;
    }

    public async Task<DeviceStatusReportResponse> Report(DeviceStatusReportRequest? request)
    {
        ThrowHelper.ThrowArgumentNullException(request);

        var newStatus = request.NewStatus;
        _logger.LogTrace($"set device status {request.DeviceId} new status {newStatus}");

        if (!_deviceHolder.TryGetLocalDevice<DeviceProxy>(request.DeviceId, out var deviceProxy) || deviceProxy == null)
        {
            return new DeviceStatusReportResponse
            {
                Code = ErrorCodes.Sys.WRONG_DEVICE_CODE,
                Message = ErrorCodes.Sys.WRONG_DEVICE_MESSAGE
            };
        }

        await deviceProxy.RefreshProperties(request.Params);

        if (PanelChanged(request, deviceProxy))
        {
            // 仅当板料发生时，才调用此方法。
            _logger.LogDebug($"{request.DeviceId}:仅当板料发生时，才调用此方法");
            await deviceProxy.SetPanels(request.PayloadPanels);

            _ = TraceLocationPanels(request.PayloadPanels, deviceProxy.DeviceKind, $"料仓数据发生变更");
        }

        if (DeviceKindExtensions.IsAGV(deviceProxy.Descriptor.DeviceKind))
        {
            var agvDevice = deviceProxy as Agv;
            await agvDevice.RefreshCarCurrentPos(request);

            //if (request.NewStatus != DeviceStatus.Working
            //    && request.NewStatus != DeviceStatus.Ready
            //    && !string.IsNullOrEmpty(agvDevice.TargetDevice))
            //{
            //    agvDevice.TargetDevice = string.Empty;
            //}

            if (request.NewStatus == DeviceStatus.Ready)
            {
                deviceProxy.Properties[ScheduleConstants.PARAMS_TASK_ID] = string.Empty;
                deviceProxy.Properties[ScheduleConstants.PARAMS_TASK_ITEM_CODE] = string.Empty;
                deviceProxy.Properties[ScheduleConstants.PARAMS_TASK_ITEM_COUNT] = 0;
                deviceProxy.Properties["changedSpindles"] = string.Empty;
                deviceProxy.Properties["changedBehavior"] = string.Empty;
                deviceProxy.Properties["RequestInteractionBehavior"] = InteractionBehavior.Noop;
                deviceProxy.Properties["RequestInteractionBehaviorName"] = string.Empty;

                if (deviceProxy.Status != DeviceStatus.Ready)
                {
                    deviceProxy.DeviceStandbyTime = DateTime.Now;
                }

                if (!string.IsNullOrEmpty(agvDevice.TargetDevice))
                {
                    agvDevice.ReleaseAgv();
                }
            }
            else
            {
                deviceProxy.DeviceStandbyTime = null;
            }
        }

        if (deviceProxy.DeviceKind == DeviceKind.FrontToolAgv
            && request.PayloadCutterTrays.Any())
        {
            await deviceProxy.SetCutterTrays(request.PayloadCutterTrays);
        }

        var oldStatus = deviceProxy.Status;
        deviceProxy.Status = request.NewStatus;
        _logger.LogInformation($"change {request.DeviceId} status from {oldStatus} to {newStatus}");

        var responseSuccess = new DeviceStatusReportResponse
        {
            Code = ErrorCodes.Sys.SUCCESS,
            Message = "success"
        };

        return responseSuccess;
    }
    private async Task TraceLocationPanels(PanelList payloadPanels, DeviceKind deviceKind, string subject)
    {
        switch (deviceKind)
        {
            case DeviceKind.BackPanelAgv:
            case DeviceKind.FrontPanelAgv:
                if (await _sysConfigManager.GetBoolValue("EnableTraceLocationPanels-AGV"))
                {
                    await _locationAdapter.TraceLocationPanels(payloadPanels, subject);
                }
                break;
        }
    }
    private bool PanelChanged(DeviceStatusReportRequest request, DeviceProxy deviceProxy)
    {
        if (!DeviceKindExtensions.IsAGV(deviceProxy.DeviceKind))
        {
            return true;
        }
        else
        {
            if (deviceProxy.PayloadPanels != null
                && request.PayloadPanels != null)
            {
                if (request.PayloadPanels.Count != deviceProxy.PayloadPanels.Count
                    || request.PayloadPanels.UndrilledPanelCount() != deviceProxy.PayloadPanels.UndrilledPanelCount()
                    || request.PayloadPanels.DrilledPanelCount() != deviceProxy.PayloadPanels.DrilledPanelCount())
                {
                    return true;
                }

                for (int i = 0; i < deviceProxy.PayloadPanels.Count(); i++)
                {
                    var panel = deviceProxy.PayloadPanels[i];
                    if (panel.SiloCode.ToLower() != request.PayloadPanels[i].SiloCode.ToLower()
                        || panel.ItemCode.ToLower() != request.PayloadPanels[i].ItemCode.ToLower()
                        || panel.PanelCode.ToLower() != request.PayloadPanels[i].PanelCode.ToLower()
                        || panel.ProductStatus != request.PayloadPanels[i].ProductStatus
                        )
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
