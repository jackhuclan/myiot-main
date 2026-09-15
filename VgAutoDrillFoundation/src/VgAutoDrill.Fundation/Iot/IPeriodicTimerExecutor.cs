namespace VgAutoDrill.Fundation.Iot;

public interface IPeriodicTimerExecutor : IThreadPoolWorkItem
{
    event Func<Task> OnTick;
    void Dispose();
}
