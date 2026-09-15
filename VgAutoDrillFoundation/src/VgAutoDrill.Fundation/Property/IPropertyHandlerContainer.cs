using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Property;

public interface IPropertyHandlerContainer
{
    IPropertyHandler this[Type type] { get; }
    IPropertyHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IPropertyHandler
        where TDevice : Device;
}
