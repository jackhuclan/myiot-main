using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Alarm;

public class ScheduleHandlerContainer : IScheduleHandlerContainer
{
    private readonly IServiceProvider serviceProvider;
    private Dictionary<Type, IScheduleHandler> _cachedSchedules = new();

    public ScheduleHandlerContainer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IScheduleHandler this[Type type]
    {
        get { return _cachedSchedules[type]; }
    }

    public IScheduleHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IScheduleHandler
        where TDevice : Device
    {
        if (typeof(THandler).IsAbstract) throw new ArgumentException(nameof(THandler));

        if (_cachedSchedules.TryGetValue(typeof(TDevice), out var handler))
        {
            return handler;
        }

        handler = (THandler)ActivatorUtilities.CreateInstance(serviceProvider, typeof(THandler), device);
        _cachedSchedules.Add(typeof(TDevice), handler);

        return handler;
    }
}
