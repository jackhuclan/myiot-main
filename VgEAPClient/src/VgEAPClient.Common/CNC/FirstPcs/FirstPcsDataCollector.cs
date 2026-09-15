// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.FirstPcs;

internal class FirstPcsDataCollector : AbstractDataCollector<FirstPcsData>
{
    private readonly ILogger<FirstPcsDataCollector> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly FirstPcsDataCollectorOptions _options;

    public FirstPcsDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        FirstPcsDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<FirstPcsDataCollector>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        try
        {
            try
            {
                Data.PgmFilePath = await _cNCConnector.RetrieveData("PgmFilePath");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{ex.Message}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
