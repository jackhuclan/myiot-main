// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.Common;

internal class DrillCommonDataCollectorC : AbstractDataCollector<DrillCommonDataC>
{
    private readonly ILogger<DrillCommonDataCollectorC> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillCommonDataCollectorOptionsC _options;
    private readonly EAPClientOptions _eAPClientOptions;

    public DrillCommonDataCollectorC(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        IOptions<EAPClientOptions> eAPClientOptions,
        DrillCommonDataCollectorOptionsC options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillCommonDataCollectorC>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
        _eAPClientOptions = eAPClientOptions.Value;

        for (int i = 0; i < _eAPClientOptions.SpindleCount; i++)
        {
            SpindleMeasureInfo spindleMeasureInfo = new SpindleMeasureInfo() { SpindleId = i + 1 };
            Data.ListSpindleMeasureInfo.Add(spindleMeasureInfo);
        }
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        _logger.LogInformation($"PullData");

        try
        {
            Data.PreDuty = await _cNCConnector.RetrieveData("PreDuty");
            Data.OPID = await _cNCConnector.RetrieveData("OPID");
            Data.SAX = await _cNCConnector.RetrieveData("SAX");
            Data.SAY = await _cNCConnector.RetrieveData("SAY");
            Data.SAZX = await _cNCConnector.RetrieveData("SAZX");
            Data.SAZY = await _cNCConnector.RetrieveData("SAZY");
            foreach (var spindleMesInfo in Data.ListSpindleMeasureInfo)
            {
                spindleMesInfo.SpindleEnable = await _cNCConnector.RetrieveData("SpindleEnable_" + spindleMesInfo.SpindleId.ToString()); //ZS_ZSEL
                spindleMesInfo.TMeasureDia = await _cNCConnector.RetrieveData("TMeasureDia_" + spindleMesInfo.SpindleId.ToString());
                spindleMesInfo.TMeasureLen = await _cNCConnector.RetrieveData("TMeasureLen_" + spindleMesInfo.SpindleId.ToString());
                spindleMesInfo.TRunout = await _cNCConnector.RetrieveData("TRunout_" + spindleMesInfo.SpindleId.ToString());
                spindleMesInfo.SpindleWorkTimes = await _cNCConnector.RetrieveData("Spindle1WorkTimes_" + spindleMesInfo.SpindleId.ToString()); //PC_SpinHisTime
                spindleMesInfo.SpindleWorkTotalMinutes = await _cNCConnector.RetrieveData("SpindleWorkTotalMinutes_" + spindleMesInfo.SpindleId.ToString());
            }

            Data.UserName = await _cNCConnector.RetrieveData("UserName");
            Data.UserLevel = await _cNCConnector.RetrieveData("UserLevel");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
