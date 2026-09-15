// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.Common;

internal class DrillCommonDataCollectorA : AbstractDataCollector<DrillCommonDataA>
{
    private readonly ILogger<DrillCommonDataCollectorA> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillCommonDataCollectorOptionsA _options;

    public DrillCommonDataCollectorA(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillCommonDataCollectorOptionsA options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillCommonDataCollectorA>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        _logger.LogInformation($"PullData::DrillCommonDataA");
        Data.LastName = "Init";
        try
        {
            Data.Duty = await _cNCConnector.RetrieveData("Duty"); Data.LastName = "Duty";
            Data.ShiftOnlineTime = await _cNCConnector.RetrieveData("ShiftOnlineTime"); Data.LastName = "ShiftOnlineTime";
            Data.ShiftWorkingTime = await _cNCConnector.RetrieveData("ShiftWorkingTime"); Data.LastName = "ShiftWorkingTime";
            Data.ShiftWaitingTime = await _cNCConnector.RetrieveData("ShiftWaitingTime"); Data.LastName = "ShiftWaitingTime";
            Data.ShiftErrorTime = await _cNCConnector.RetrieveData("ShiftErrorTime"); Data.LastName = "ShiftErrorTime";
            Data.ProgramPath = await _cNCConnector.RetrieveData("PgmFilePath"); Data.LastName = "PgmFilePath";
            Data.XYPosition = await _cNCConnector.RetrieveData("XYPosition"); Data.LastName = "XYPosition";
            Data.CurDrillOrRout = await _cNCConnector.RetrieveData("CurDrillOrRout"); Data.LastName = "CurDrillOrRout";
            Data.TotalDrillOrRout = await _cNCConnector.RetrieveData("TotalDrillOrRout"); Data.LastName = "TotalDrillOrRout";
            Data.CurSpindleStatus = await _cNCConnector.RetrieveData("CurSpindleStatus"); Data.LastName = "CurSpindleStatus";
            Data.CurToolId = await _cNCConnector.RetrieveData("CurToolId"); Data.LastName = "CurToolId";
            if (!string.IsNullOrEmpty(Data.CurToolId))
            {
                int nToolId = 0;
                if (int.TryParse(Data.CurToolId, out nToolId))
                {
                    if (nToolId > 0)
                    {
                        Data.CurToolD = await _cNCConnector.RetrieveData("ToolD_" + Data.CurToolId); Data.LastName = "ToolD_";
                        Data.CurToolS = await _cNCConnector.RetrieveData("ToolS_" + Data.CurToolId); Data.LastName = "ToolS_";
                        Data.CurToolF = await _cNCConnector.RetrieveData("ToolF_" + Data.CurToolId); Data.LastName = "ToolF_";
                        Data.CurToolR = await _cNCConnector.RetrieveData("ToolR_" + Data.CurToolId); Data.LastName = "ToolR_";
                        Data.CurToolN = await _cNCConnector.RetrieveData("ToolN_" + Data.CurToolId); Data.LastName = "ToolN_";
                        Data.CurToolB = await _cNCConnector.RetrieveData("ToolB_" + Data.CurToolId); Data.LastName = "ToolB_";
                        Data.CurToolZ = await _cNCConnector.RetrieveData("ToolZ_" + Data.CurToolId); Data.LastName = "ToolZ_";
                        Data.CurToolA = await _cNCConnector.RetrieveData("ToolA_" + Data.CurToolId); Data.LastName = "ToolA_";
                        Data.CurToolSegM = await _cNCConnector.RetrieveData("ToolSegM_" + Data.CurToolId); Data.LastName = "ToolSegM_";
                        Data.CurToolChipl = await _cNCConnector.RetrieveData("ToolChipl_" + Data.CurToolId); Data.LastName = "ToolChipl_";
                    }
                }
            }

            Data.CncRunProgress = await _cNCConnector.RetrieveData("CncRunProgress"); Data.LastName = "CncRunProgress";
            Data.Block = await _cNCConnector.RetrieveData("Block"); Data.LastName = "Block";
            Data.Step = await _cNCConnector.RetrieveData("Step"); Data.LastName = "Step";
            Data.DrillZ = await _cNCConnector.RetrieveData("DrillZ"); Data.LastName = "DrillZ";
            Data.DrillFV = await _cNCConnector.RetrieveData("DrillFV"); Data.LastName = "DrillFV";
            Data.ShiftTotalDrillOrRout = await _cNCConnector.RetrieveData("ShiftTotalDrillOrRout"); Data.LastName = "ShiftTotalDrillOrRout";
            Data.ShiftRunCount = await _cNCConnector.RetrieveData("ShiftRunCount"); Data.LastName = "ShiftRunCount";
            Data.ShiftToolChangeTime = await _cNCConnector.RetrieveData("ShiftToolChangeTime"); Data.LastName = "ShiftToolChangeTime";
            Data.SpindleCount = await _cNCConnector.RetrieveData("SpindleCount"); Data.LastName = "SpindleCount";
            string strIsCncRunning = await _cNCConnector.RetrieveData("IsCncRunning"); Data.LastName = "IsCncRunning";
            Data.IsCncRunning = bool.Parse(strIsCncRunning);

            Data.LastGetTime = DateTime.Now;
            Data.LastName = "InitEnd";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
