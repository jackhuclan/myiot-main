namespace VgAutoDrill.Fundation.Iot;

public interface IPeriodicTimerExecutorFactory
{
    IPeriodicTimerExecutor? this[string interval] { get; }
    void Start();
    void Stop();
}
