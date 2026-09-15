using Mediator.Net.Context;
using Mediator.Net.Contracts;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Event;
using VgAutoDrill.Central.Core.Mes.Interface;

namespace VgAutoDrill.Central.Core.Handler;

internal class PersistLocationPanelHandler : IEventHandler<PanelChangedEvent>
{
    private readonly IDeviceAdapter _deviceAdapter;
    private readonly ILogger<PersistLocationPanelHandler> _logger;

    public PersistLocationPanelHandler(IDeviceAdapter deviceAdapter,
        ILoggerFactory loggerFactory)
    {
        _deviceAdapter = deviceAdapter;
        _logger = loggerFactory.CreateLogger<PersistLocationPanelHandler>();
    }

    public Task Handle(IReceiveContext<PanelChangedEvent> context, CancellationToken cancellationToken)
    {
        if (context.Message.ChangedLocation.HostDevice == null)
            return Task.CompletedTask;

        var productId = context.Message.ChangedLocation.HostDevice.ProductId;
        var deviceId = context.Message.ChangedLocation.HostDevice.DeviceId;
        _logger.LogInformation($"saved {context.Message.ChangedLocation.Code}'s panels");
        return _deviceAdapter.PersistPanels(productId, deviceId, context.Message.ChangedLocation.OriginPanels);
    }
}
