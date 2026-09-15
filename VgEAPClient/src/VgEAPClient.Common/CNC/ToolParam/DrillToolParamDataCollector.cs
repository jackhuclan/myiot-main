// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.ToolParam;

internal class DrillToolParamDataCollector : AbstractDataCollector<DrillToolParamData>
{
    private readonly ILogger<DrillToolParamDataCollector> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillToolParamDataCollectorOptions _options;

    public DrillToolParamDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillToolParamDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillToolParamDataCollector>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        try
        {
            string strTPodCount = await _cNCConnector.RetrieveData("TParamPodCount");
            string[] aryTPodCount = strTPodCount.Split(',');
            Data.ToolParamPodCount = Array.ConvertAll<string, int>(aryTPodCount, s => int.Parse(s));

            string strDrillZ = await _cNCConnector.RetrieveData("DrillZ");
            Data.DrillZ = Convert.ToDouble(strDrillZ);

            await Data.LockListToolParam.WaitAsync();
            try
            {
                Data.ListToolParam.Clear();
                for (int i = 0; i < Data.ToolParamPodCount.Length; i++)
                {
                    if (Data.ToolParamPodCount[i] == 0)
                    {
                        continue;
                    }

                    ToolParam toolParam = new ToolParam();
                    toolParam.ToolId = i + 1;
                    toolParam.ToolD = await _cNCConnector.RetrieveData($"ToolD_{toolParam.ToolId}");
                    toolParam.ToolS = await _cNCConnector.RetrieveData($"ToolS_{toolParam.ToolId}");
                    toolParam.ToolF = await _cNCConnector.RetrieveData($"ToolF_{toolParam.ToolId}");
                    toolParam.ToolR = await _cNCConnector.RetrieveData($"ToolR_{toolParam.ToolId}");
                    toolParam.ToolN = await _cNCConnector.RetrieveData($"ToolN_{toolParam.ToolId}");
                    toolParam.ToolB = await _cNCConnector.RetrieveData($"ToolB_{toolParam.ToolId}");
                    toolParam.ToolZOffset = await _cNCConnector.RetrieveData($"ToolZOffset_{toolParam.ToolId}");
                    toolParam.ToolZ = (Convert.ToDouble(toolParam.ToolZOffset) + Data.DrillZ).ToString("F3");
                    toolParam.ToolSpeed = await _cNCConnector.RetrieveData($"ToolSpeed_{toolParam.ToolId}");

                    Data.ListToolParam.Add(toolParam);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                Data.LockListToolParam.Release();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
