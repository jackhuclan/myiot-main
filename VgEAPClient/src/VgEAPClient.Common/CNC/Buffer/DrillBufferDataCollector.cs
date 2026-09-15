// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.Buffer;

internal class DrillBufferDataCollector : AbstractDataCollector<DrillBufferData>
{
    private readonly ILogger<DrillBufferDataCollector> _logger;
    private readonly DrillBufferDataCollectorOptions _options;

    public DrillBufferDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillBufferDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillBufferDataCollector>();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override Task PullData()
    {
        return base.PullData();
    }
}
