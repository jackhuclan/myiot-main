// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.Common;

internal class DrillCommonDataCollectorB : AbstractDataCollector<DrillCommonDataB>
{
    private readonly ILogger<DrillCommonDataCollectorB> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillCommonDataCollectorOptionsB _options;

    public DrillCommonDataCollectorB(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillCommonDataCollectorOptionsB options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillCommonDataCollectorB>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        _logger.LogInformation($"PullData");

        try
        {
            Data.DiaFilePath = await _cNCConnector.RetrieveData("DiaFilePath");
            Data.AtpFilePath = await _cNCConnector.RetrieveData("AtpFilePath");
            Data.DrillH = await _cNCConnector.RetrieveData("DrillH");
            Data.DrillQ = await _cNCConnector.RetrieveData("DrillQ");
            Data.DrillK = await _cNCConnector.RetrieveData("DrillK");
            Data.DrillKi = await _cNCConnector.RetrieveData("DrillKi");
            Data.CncVersion = await _cNCConnector.RetrieveData("CncVersion");
            Data.SpindleYaw = await _cNCConnector.RetrieveData("SpindleYaw");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
