using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Event;

public interface IEventHandlerContainer
{
    IEventHandler this[Type type] { get; }
    IEventHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IEventHandler
        where TDevice : Device;
}
