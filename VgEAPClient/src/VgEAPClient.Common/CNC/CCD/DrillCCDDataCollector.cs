// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.CCD;

internal class DrillCCDDataCollector : AbstractDataCollector<DrillCCDData>
{
    private readonly ILogger<DrillCCDDataCollector> _logger;
    private readonly DrillCCDDataCollectorOptions _options;

    public DrillCCDDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillCCDDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillCCDDataCollector>();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override Task PullData()
    {
        return base.PullData();
    }
}
