using Microsoft.Extensions.Logging;

namespace VgAutoDrill.Fundation.Iot;

internal class PeriodicTimerExecutor : IPeriodicTimerExecutor
{
    private readonly PeriodicTimer _periodicTimer;
    private readonly ILoggerFactory _loggerFactory;
    private readonly CancellationToken _cancellationToken;
    private readonly ILogger<PeriodicTimerExecutor> _logger;

    public event Func<Task> OnTick = () => Task.CompletedTask;

    public PeriodicTimerExecutor(PeriodicTimer periodicTimer,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken = default)
    {
        _periodicTimer = periodicTimer;
        _loggerFactory = loggerFactory;
        _cancellationToken = cancellationToken;
        _logger = _loggerFactory.CreateLogger<PeriodicTimerExecutor>();
    }

    public void Dispose()
    {
        _periodicTimer.Dispose();
    }

    void IThreadPoolWorkItem.Execute()
    {
        _ = ExecuteAsync(_cancellationToken);
    }

    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (_periodicTimer)
        {
            while (!stoppingToken.IsCancellationRequested && await _periodicTimer.WaitForNextTickAsync())
            {
                try
                {
                    await OnTick.Invoke();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                finally
                {
                }
            }
        }
    }

}
