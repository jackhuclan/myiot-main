// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.Knife;

internal class DrillBrokenKnifeDataCollector : AbstractDataCollector<DrillBrokenKnifeData>
{
    private readonly ILogger<DrillBrokenKnifeDataCollector> _logger;
    private readonly DrillBrokenKnifeDataCollectorOptions _options;

    public DrillBrokenKnifeDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillBrokenKnifeDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillBrokenKnifeDataCollector>();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override Task PullData()
    {
        return base.PullData();
    }
}
