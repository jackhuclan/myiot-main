// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;

namespace VgEAPClient.Common.CNC;

internal class DataCollectionMetricExecutor : IThreadPoolWorkItem, IDisposable
{
    private readonly DataCollectionMetric _collectionMetric;
    private readonly PeriodicTimer _periodicTimer;
    private readonly ILogger<DataCollectionMetric> _logger;
    private readonly CancellationToken _cancellationToken;
    private readonly ICNCConnector _cNCConnector;

    public DataCollectionMetricExecutor(ICNCOperatorProvider cNCOperatorProvider,
        DataCollectionMetric collectionMetric,
        ILoggerFactory loggerFactory)
    {
        _collectionMetric = collectionMetric;
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _periodicTimer = new PeriodicTimer(collectionMetric.CollectionInterval);

        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(collectionMetric.CollectionExpired);
        _cancellationToken = tokenSource.Token;
        _logger = loggerFactory.CreateLogger<DataCollectionMetric>();
    }

    public void Dispose()
    {
        _cancellationToken.ThrowIfCancellationRequested();
        _periodicTimer.Dispose();
    }

    void IThreadPoolWorkItem.Execute()
    {
        _ = ExecuteAsync();
    }

    private async Task ExecuteAsync()
    {
        using (_periodicTimer)
        {
            while (!_cancellationToken.IsCancellationRequested && await _periodicTimer.WaitForNextTickAsync())
            {
                try
                {
                    _collectionMetric.MetricValue = await _cNCConnector.RetrieveData(_collectionMetric.MetricName, _cancellationToken);
                    _logger.LogInformation($"collected metric {_collectionMetric.MetricName}'s value {_collectionMetric.MetricValue}");
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
