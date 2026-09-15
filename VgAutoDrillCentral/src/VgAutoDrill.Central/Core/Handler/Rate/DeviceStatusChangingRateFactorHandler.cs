using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Fundation.Iot.Extensions;

namespace VgAutoDrill.Central.Core.Handler.Rate;

/// <summary>
/// 设备状态发生改变的稼动率因素要持久化到数据库
/// </summary>
internal class DeviceStatusChangingRateFactorHandler
{
    private readonly IDeviceManager _deviceManager;
    private readonly IDrillRateFactorService _drillRateFactorService;
    private readonly ILogger<DeviceStatusChangingRateFactorHandler> _logger;
    public DeviceStatusChangingRateFactorHandler(IDeviceManager deviceManager,
        IDrillRateFactorService drillRateFactorService,
        ILoggerFactory loggerFactory)
    {
        _deviceManager = deviceManager;
        _drillRateFactorService = drillRateFactorService;
        _logger = loggerFactory.CreateLogger<DeviceStatusChangingRateFactorHandler>();
    }

    public Task Handle(DeviceStatusChangingEventArgs args)
    {
        if (!_deviceManager.TryGetOnlineDevice<DeviceProxy>(args.DeviceId, out var device)
            || device == null)
        {
            return Task.CompletedTask;
        }

        if (DeviceKindExtensions.IsPanelAGV(device.DeviceKind)
            && args.Status == Fundation.Iot.DeviceStatus.Exception)
        {
            _drillRateFactorService.Add(new AddOrUpdateDrillRateFactorReq
            {
                DeviceId = args.DeviceId,
                Reason = DrillRateFactorReason.PanelAgvException,
                StartTime = args.StartTime,
                EndTime = args.EndTime,
            });

            _logger.LogInformation("DeviceStatusChangingRateFactorHandler save PanelAGV exception factor");
        }

        return Task.CompletedTask;
    }
}
