// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;
using VgEAPClient.Common.CNC.Status;
using VgEAPClient.Common.CNC.ToolMeasurement;

namespace VgEAPClient.Common.CNC.ToolMeasure;

internal class DrillToolMeasureDataCollector : AbstractDataCollector<DrillToolMeasureData>
{
    private readonly ILogger<DrillStatusDataCollector> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillToolMeasureDataCollectorOptions _options;

    public DrillToolMeasureDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillToolMeasureDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillStatusDataCollector>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        try
        {
            Data.DiameterTol.Checked = await _cNCConnector.RetrieveData("DiameterTolChecked");
            Data.DiameterTol.NegValue = await _cNCConnector.RetrieveData("DiameterTolNegValue");
            Data.DiameterTol.PosValue = await _cNCConnector.RetrieveData("DiameterTolPosValue");

            Data.LengthTol.Checked = await _cNCConnector.RetrieveData("LengthTolChecked");
            Data.LengthTol.NegValue = await _cNCConnector.RetrieveData("LengthTolNegValue");
            Data.LengthTol.PosValue = await _cNCConnector.RetrieveData("LengthTolPosValue");

            Data.RunoutTol.Checked = await _cNCConnector.RetrieveData("RunoutTolChecked");
            Data.RunoutTol.NegValue = await _cNCConnector.RetrieveData("RunoutTolNegValue");
            Data.RunoutTol.PosValue = await _cNCConnector.RetrieveData("RunoutTolPosValue");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
