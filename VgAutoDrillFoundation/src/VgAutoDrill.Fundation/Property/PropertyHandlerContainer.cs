using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Property;

public class PropertyHandlerContainer : IPropertyHandlerContainer
{
    private readonly IServiceProvider serviceProvider;
    private readonly Dictionary<Type, IPropertyHandler> _cachedProperties = new();
    public PropertyHandlerContainer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IPropertyHandler this[Type type]
    {
        get { return _cachedProperties[type]; }
    }

    public IPropertyHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IPropertyHandler
        where TDevice : Device
    {
        if (typeof(THandler).IsAbstract) throw new ArgumentException(nameof(THandler));

        if (_cachedProperties.TryGetValue(typeof(TDevice), out var handler))
        {
            return handler;
        }

        handler = (THandler)ActivatorUtilities.CreateInstance(serviceProvider, typeof(THandler), device);
        _cachedProperties.Add(typeof(TDevice), handler);
        handler.AddWatchingProperties();

        return handler;
    }
}
