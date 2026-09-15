using Mediator.Net.Context;
using Mediator.Net.Contracts;
using VgAutoDrill.Central.Core.Event;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Central.Core.Handler;

/// <summary>
/// WIP分区上的板料发生变化，即缓存区上的板料发生变化，触发生成料仓任务逻辑
/// </summary>
public class WipLocationPanelChangedEventHandler : IEventHandler<PanelChangedEvent>
{
    public Task Handle(IReceiveContext<PanelChangedEvent> context, CancellationToken cancellationToken)
    {
        var bothForkAndWipMode = context.Message.ChangedLocation.RequestDeviceKind == DeviceKind.PublicPanelSiloWIP;

        var partition = context.Message.ChangedLocation.Partition;

        if (partition != null
            && partition.PartitionKind == PartitionKind.Public
            && bothForkAndWipMode)
        {
            partition.SiloTransferStrategy.ReassignSiloTransferJob();
        }

        context.Result = true;
        return Task.CompletedTask;
    }
}
