// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VgEAPClient.Common;

/// <summary>
/// 后台周期性的任务
/// </summary>
public class BackgroundPeriodicTask : IPeriodicTask
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<BackgroundPeriodicTask> _logger;
    private readonly PeriodicTimer _periodicTimer;

    public BackgroundPeriodicTask(IHostApplicationLifetime hostApplicationLifetime,
        ILoggerFactory loggerFactory,
        TimeSpan timeSpan)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = loggerFactory.CreateLogger<BackgroundPeriodicTask>();
        _periodicTimer = new PeriodicTimer(timeSpan);
        _hostApplicationLifetime.ApplicationStopped.Register(() => this.Dispose());
    }

    public void Dispose()
    {
        _periodicTimer.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
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

    protected virtual Task EexecuteAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
