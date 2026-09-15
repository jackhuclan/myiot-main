// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Infrastructure;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC;

internal class DataCollectorStarter : IDataCollectorStarter
{
    private readonly ILogger<DataCollectorStarter> _logger;
    private readonly DataCollectorOptions _options;
    private readonly IObjectFactory _objectFactory;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly List<DataCollectionMetricExecutor> _dataCollectionMetricExecutors = new();

    public DataCollectorStarter(IOptions<DataCollectorOptions> options,
        IObjectFactory objectFactory,
        IHostApplicationLifetime hostApplicationLifetime,
        ILoggerFactory loggerFactory)
    {
        _options = options.Value;
        _logger = loggerFactory.CreateLogger<DataCollectorStarter>();
        _objectFactory = objectFactory;
        _hostApplicationLifetime = hostApplicationLifetime;
        _hostApplicationLifetime.ApplicationStopped.Register(Dispose);
    }

    public void Dispose()
    {
        foreach (var executor in _dataCollectionMetricExecutors)
        {
            executor.Dispose();
        }
    }

    public void Start()
    {
        if (_options.Metrics.Count == 0)
            return;

        foreach (var metric in _options.Metrics.Where(x => x.Enabled))
        {
            var executor = _objectFactory.CreateObject<DataCollectionMetricExecutor>(metric);
            _dataCollectionMetricExecutors.Add(executor);
            _logger.LogInformation($"create DataCollectionMetricExecutor {metric.MetricName} with{metric.CollectionInterval}");
            ThreadPool.UnsafeQueueUserWorkItem(executor, preferLocal: false);
        }
    }
}
