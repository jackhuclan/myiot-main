using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Alarm;

public class AlarmHandlerContainer : IAlarmHandlerContainer
{
    private readonly IServiceProvider serviceProvider;
    private Dictionary<Type, IAlarmHandler> _cachedAlarms = new();

    public AlarmHandlerContainer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IAlarmHandler this[Type type]
    {
        get { return _cachedAlarms[type]; }
    }

    public IAlarmHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IAlarmHandler
        where TDevice : Device
    {
        if (typeof(THandler).IsAbstract) throw new ArgumentException(nameof(THandler));

        if (_cachedAlarms.TryGetValue(typeof(TDevice), out var handler))
        {
            return handler;
        }

        handler = (THandler)ActivatorUtilities.CreateInstance(serviceProvider, typeof(THandler), device);
        _cachedAlarms.Add(typeof(TDevice), handler);
        handler.AddWatchingAlarms();

        return handler;
    }
}
