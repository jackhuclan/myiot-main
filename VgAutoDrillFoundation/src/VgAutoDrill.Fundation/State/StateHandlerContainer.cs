using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.State;

public class StateHandlerContainer : IStateHandlerContainer
{
    private readonly IServiceProvider serviceProvider;
    private Dictionary<Type, IStateHandler> _cachedStates = new();
    public StateHandlerContainer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IStateHandler this[Type type]
    {
        get { return _cachedStates[type]; }
    }

    public IStateHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IStateHandler
        where TDevice : Device
    {
        if (typeof(THandler).IsAbstract) throw new ArgumentException(nameof(THandler));
        if (_cachedStates.TryGetValue(typeof(TDevice), out var handler))
        {
            return handler;
        }

        handler = (THandler)ActivatorUtilities.CreateInstance(serviceProvider, typeof(THandler), device);
        _cachedStates.Add(typeof(TDevice), handler);
        handler.AddWatchingStates();

        return handler;
    }
}
