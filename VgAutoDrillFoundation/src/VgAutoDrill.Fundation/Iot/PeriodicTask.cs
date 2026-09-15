using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VgAutoDrill.Fundation.Iot;

/// <summary>
/// 周期性的任务
/// </summary>
public abstract class PeriodicTask
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<PeriodicTask> _logger;
    private readonly PeriodicTimer _periodicTimer;

    public PeriodicTask(IHostApplicationLifetime hostApplicationLifetime,
        ILoggerFactory loggerFactory,
        TimeSpan timeSpan)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = loggerFactory.CreateLogger<PeriodicTask>();
        _periodicTimer = new PeriodicTimer(timeSpan);
        _hostApplicationLifetime.ApplicationStopped.Register(() => Dispose());
    }

    protected virtual void Dispose()
    {
        _periodicTimer.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        using (_periodicTimer)
        {
            while (!cancellationToken.IsCancellationRequested
                && await _periodicTimer.WaitForNextTickAsync())
            {
                _ = EexecuteAsync(cancellationToken);
            }
        }
    }

    protected abstract Task EexecuteAsync(CancellationToken cancellationToken);
}
