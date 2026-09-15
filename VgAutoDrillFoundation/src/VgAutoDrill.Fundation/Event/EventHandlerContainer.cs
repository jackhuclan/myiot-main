using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Event;

public class EventHandlerContainer : IEventHandlerContainer
{
    private readonly IServiceProvider serviceProvider;
    private Dictionary<Type, IEventHandler> _cachedEvents = new();

    public EventHandlerContainer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IEventHandler this[Type type]
    {
        get { return _cachedEvents[type]; }
    }

    public IEventHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IEventHandler
        where TDevice : Device
    {
        if (typeof(THandler).IsAbstract) throw new ArgumentException(nameof(THandler));

        if (_cachedEvents.TryGetValue(typeof(TDevice), out var handler))
        {
            return handler;
        }

        handler = (THandler)ActivatorUtilities.CreateInstance(serviceProvider, typeof(THandler), device);
        _cachedEvents.Add(typeof(TDevice), handler);
        handler.AddWatchingEvents();

        return handler;
    }
}
