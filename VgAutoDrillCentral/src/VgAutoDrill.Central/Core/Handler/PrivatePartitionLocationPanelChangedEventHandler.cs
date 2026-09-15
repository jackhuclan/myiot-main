using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Central.Core.Event;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Handler;

/// <summary>
/// 私有分区上的板料发生变化，即钻机或者插齿的板料发生变化，触发生成料仓任务逻辑
/// </summary>
public class PrivatePartitionLocationPanelChangedEventHandler : IEventHandler<PanelChangedEvent>
{
    private readonly ILogger<PrivatePartitionLocationPanelChangedEventHandler> _logger;
    private readonly ISysConfigManager _sysConfigManager;

    public PrivatePartitionLocationPanelChangedEventHandler(ILoggerFactory loggerFactory,
        ISysConfigManager sysConfigManager)
    {
        _logger = loggerFactory.CreateLogger<PrivatePartitionLocationPanelChangedEventHandler>();
        _sysConfigManager = sysConfigManager;
    }

    public async Task Handle(IReceiveContext<PanelChangedEvent> context, CancellationToken cancellationToken)
    {
        var requestFromDrillOrFork = DeviceKindExtensions.IsDrill(context.Message.ChangedLocation.RequestDeviceKind)
            || context.Message.ChangedLocation.RequestDeviceKind == DeviceKind.PanelSiloFork;

        var partition = context.Message.ChangedLocation.Partition;

        if (partition != null
            && partition.PartitionKind == PartitionKind.Private
            && requestFromDrillOrFork
            && partition.Status == 1)
        {
            if (await _sysConfigManager.GetBoolValue(MESConfigConstants.CENTRAL_CONTROL_SYSTEM_IS_MAINTAIN))
            {
                _logger.LogWarning($"系统即将停机维护: ForkOrDrillLocationPanelChangedEventHandler not excute");
                return;
            }

            _logger.LogInformation($"ForkOrDrillLocationPanelChangedEventHandler,partitionCode:{partition.PartCode},Start Excute Handle==========================\r\n");

            partition.SiloTransferStrategy.ReassignSiloTransferJob();

            _logger.LogInformation($"ForkOrDrillLocationPanelChangedEventHandler,partitionCode:{partition.PartCode},End Excute Handle==========================\r\n");
        }
    }
}
