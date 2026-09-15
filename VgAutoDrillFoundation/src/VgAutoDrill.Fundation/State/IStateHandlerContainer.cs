using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.State;

public interface IStateHandlerContainer
{
    IStateHandler this[Type type] { get; }
    IStateHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IStateHandler
        where TDevice : Device;
}
