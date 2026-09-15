// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.Alarm;

internal class DrillAlarmDataCollector : AbstractDataCollector<DrillAlarmData>
{
    private readonly ILogger<DrillAlarmDataCollector> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillAlarmDataCollectorOptions _options;

    public DrillAlarmDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillAlarmDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillAlarmDataCollector>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        _logger.LogInformation($"PullData");

        try
        {
            Data.AlarmText = await _cNCConnector.RetrieveData("aa");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
