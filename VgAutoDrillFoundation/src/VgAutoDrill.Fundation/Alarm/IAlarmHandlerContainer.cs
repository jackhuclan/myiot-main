using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Alarm;

public interface IAlarmHandlerContainer
{
    IAlarmHandler this[Type type] { get; }
    IAlarmHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IAlarmHandler
        where TDevice : Device;
}
