// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VgEAPClient.Common.CNC;

internal class AbstractDataCollector<TCollectionData> : BackgroundService, IDataCollector<TCollectionData>
    where TCollectionData : class, new()
{
    private readonly ICNCConnector _cncConnector;
    private readonly PeriodicTimer _periodicTimer;
    private readonly ILogger<AbstractDataCollector<TCollectionData>> _logger;

    public AbstractDataCollector(ICNCConnector cNCConnector,
        ILoggerFactory loggerFactory,
        TimeSpan timeSpan)
    {
        _cncConnector = cNCConnector;
        _periodicTimer = new PeriodicTimer(timeSpan);
        _logger = loggerFactory.CreateLogger<AbstractDataCollector<TCollectionData>>();
    }

    public TCollectionData Data { get; private set; } = new TCollectionData();

    public override void Dispose()
    {
        _periodicTimer.Dispose();
    }

    protected virtual bool Enabled => true;

    /// <summary>
    /// 主动拉取数据
    /// </summary>
    /// <returns></returns>
    protected virtual Task PullData() => Task.CompletedTask;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (_periodicTimer)
        {
            while (!stoppingToken.IsCancellationRequested
                && Enabled
                && await _periodicTimer.WaitForNextTickAsync())
            {
                try
                {
                    if (!_cncConnector.IsConnected)
                    {
                        await _cncConnector.Connect(stoppingToken);
                    }

                    await PullData();
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
