using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Infrastructure;

namespace VgAutoDrill.Fundation.Iot;

public class PeriodicTimerExecutorFactory : IPeriodicTimerExecutorFactory
{
    private readonly PeriodicTimerExecutorFactoryOptions _options;
    private readonly ILoggerFactory _loggerFactory;
    private readonly Dictionary<string, IPeriodicTimerExecutor> _periodicTimers;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public PeriodicTimerExecutorFactory(IOptions<PeriodicTimerExecutorFactoryOptions> options,
        ILoggerFactory loggerFactory)
    {
        _options = options.Value;
        _periodicTimers = new Dictionary<string, IPeriodicTimerExecutor>();
        _cancellationTokenSource = new CancellationTokenSource();
        _loggerFactory = loggerFactory;

        foreach (var item in _options.Intervals)
        {
            var interval = item.Trim();
            if (_periodicTimers.ContainsKey(interval)) continue;

            var excutor = Create(interval);
            if (excutor == null) continue;

            _periodicTimers.Add(interval, excutor);
        }

        Start();
    }

    public IPeriodicTimerExecutor? this[string interval]
    {
        get
        {
            if (!_periodicTimers.ContainsKey(interval)) return null;
            return _periodicTimers[interval];
        }
    }

    public void Start()
    {
        if (_periodicTimers.Count == 0) return;
        if (!_options.Enabled) return;

        foreach (var executor in _periodicTimers.Values)
        {
            ThreadPool.UnsafeQueueUserWorkItem(executor, preferLocal: false);
        }
    }

    public void Stop()
    {
        if (_periodicTimers.Count == 0) return;

        foreach (var executor in _periodicTimers.Values)
        {
            executor.Dispose();
        }
    }

    private IPeriodicTimerExecutor? Create(string interval)
    {
        if (TimeSpanParser.TryParse(interval, out TimeSpan timeSpan))
        {
            var _timer = new PeriodicTimer(timeSpan);
            return new PeriodicTimerExecutor(_timer, _loggerFactory, _cancellationTokenSource.Token);
        }

        return null;
    }
}
