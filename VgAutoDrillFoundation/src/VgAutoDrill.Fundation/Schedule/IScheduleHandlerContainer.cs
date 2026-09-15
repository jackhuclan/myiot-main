using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Fundation.Alarm;

public interface IScheduleHandlerContainer
{
    IScheduleHandler this[Type type] { get; }
    IScheduleHandler AddHandler<THandler, TDevice>(TDevice device)
        where THandler : IScheduleHandler
        where TDevice : Device;
}
