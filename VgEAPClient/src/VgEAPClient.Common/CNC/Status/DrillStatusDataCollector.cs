// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Quartz.Util;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.Status;

internal class DrillStatusDataCollector : AbstractDataCollector<DrillStatusData>
{
    private readonly ILogger<DrillStatusDataCollector> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillStatusDataCollectorOptions _options;

    public DrillStatusDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillStatusDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillStatusDataCollector>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
        Data.OnCncStatusEcChanged += OnCncStatusEcChanged;
    }

    private void OnCncStatusEcChanged()
    {
        _logger.LogInformation($"EcChanged - {Data.CncStatusText} \r\n {Data.BlockText}");
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        _logger.LogInformation($"PullData");

        try
        {
            string strStatus = await _cNCConnector.RetrieveData("CncStatus");
            string strBlockText = await _cNCConnector.RetrieveData("BlockText");
            Data.CncBlockColor = await _cNCConnector.RetrieveData("CncBlockColor");
            Data.BlockText = strBlockText;
            Data.CncStatusText = strStatus;

            Data.CncError = await _cNCConnector.RetrieveData("CncError");
            Data.CncComm = await _cNCConnector.RetrieveData("CncComm");
            if (!Data.IsStopGetRunTime)
            {
                Data.PgmRunStartTime = await _cNCConnector.RetrieveData("PgmRunStartTime");
                Data.PgmRunEndTime = await _cNCConnector.RetrieveData("PgmRunEndTime");
            }


            if (Data.PgmRunEndTime != string.Empty)
            {
                _logger.LogInformation("程式开始结束时间 - " + Data.PgmRunStartTime + " - " + Data.PgmRunEndTime);

                Data.PgmRunTotalTime = (DateTime.Parse(Data.PgmRunEndTime) - DateTime.Parse(Data.PgmRunStartTime)).ToString();
            }
            else if (Data.PgmRunStartTime != string.Empty)
            {
                Data.PgmRunTotalTime = (DateTime.Now - DateTime.Parse(Data.PgmRunStartTime)).ToString();
            }

            if (Data.IsBlockTextChanged)
            {
                string sCncPgmNum = await _cNCConnector.RetrieveData("AutoListRunNumber");
                if (int.TryParse(sCncPgmNum, out int iCncPgmNum) && iCncPgmNum > 0)
                {
                    Data.CncPgmNum = iCncPgmNum;
                }
                _logger.LogInformation($"BlockTextChanged - {Data.BlockText}");

                if ((Data.BlockText.Contains("断刀") || Data.BlockText.Contains("Broken Tool", StringComparison.OrdinalIgnoreCase)) && (!Data.BlockText.Contains("在中转刀库")))
                {
                    _logger.LogInformation($"BrokenTool - {Data.BlockText}");
                    Data.BrokenToolData = _cNCConnector.ParseBrokenToolData(Data.BrokenToolData, Data.BlockText);

                    if (!Data.BrokenToolData.BrkToolId.IsNullOrWhiteSpace()
                        || !Data.BrokenToolData.BrkToolDia.IsNullOrWhiteSpace()
                        || !Data.BrokenToolData.BrkToolSpindle.IsNullOrWhiteSpace())
                    {
                        _logger.LogInformation("Data.BrokenToolData - " + Data.BrokenToolData.BrkToolId + Data.BrokenToolData.BrkToolDia + Data.BrokenToolData.BrkToolSpindle);

                        _ = Task.Run(async () =>
                        {
                            await GetToolDataById(Data.CncToolData, Data.BrokenToolData.BrkToolId);
                            await GetProgramData(Data.CurProgramData);
                        }).ContinueWith(t =>
                        {
                            Data.IsBrokenTool = true;
                        }, TaskContinuationOptions.OnlyOnRanToCompletion);
                    }
                }
            }

            if (Data.IsPullHitOrRout)
            {
                Data.CurDrillOrRout = await _cNCConnector.RetrieveData("CurDrillOrRout");
                Data.IsPullHitOrRout = false;
            }

            if (Data.IsRequiredLoadFile)
            {
                _logger.LogInformation($"Data.IsRequiredLoadFile = true - 调用加载文件");

                Data.IsRequiredLoadFile = false;
                _ = LoadFile(Data.DrillRequiredLoadProgramDataModel)
                    .ContinueWith(t => { Data.IsLoadFileFinished = t.Result; }, TaskContinuationOptions.OnlyOnRanToCompletion);
            }

            if (Data.IsPullProgramDataOnce)
            {
                _ = GetProgramData(Data.CurProgramData);
                Data.IsPullProgramDataOnce = false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private async Task<bool> LoadFile(CncLoadFileData drillLoadFileData)
    {
        try
        {
            _logger.LogInformation($"LoadFile - 开始加载文件");
            if (!string.IsNullOrEmpty(drillLoadFileData.AtpLoadModel.FilePath))
            {
                drillLoadFileData.AtpLoadModel.LoadResult = await _cNCConnector.RetrieveData("LoadAtpFile_" + drillLoadFileData.AtpLoadModel.FilePath);
                _logger.LogInformation($"LoadFile - 加载ATP 文件完成");
            }

            if (!string.IsNullOrEmpty(drillLoadFileData.DiaLoadModel.FilePath))
            {
                drillLoadFileData.DiaLoadModel.LoadResult = await _cNCConnector.RetrieveData("LoadDiaFile_" + drillLoadFileData.DiaLoadModel.FilePath);
                _logger.LogInformation($"LoadFile - 加载DIA 文件完成");
            }

            if (!string.IsNullOrEmpty(drillLoadFileData.PgmLoadModel.FilePath))
            {
                drillLoadFileData.PgmLoadModel.LoadResult = await _cNCConnector.RetrieveData("LoadPgmFile_" + drillLoadFileData.PgmLoadModel.FilePath);
                _logger.LogInformation($"LoadFile - 加载PGM 文件完成");
            }
            _logger.LogInformation($"LoadFile - 加载文件完成");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LoadFile Exception - " + ex.Message);
        }
        return false;
    }

    private async Task GetProgramData(ProgramDataModel pgmData)
    {
        try
        {
            pgmData.PgmFilePath = await _cNCConnector.RetrieveData("PgmFilePath");
            pgmData.DiaFilePath = await _cNCConnector.RetrieveData("DiaFilePath");
            pgmData.AtpFilePath = await _cNCConnector.RetrieveData("AtpFilePath");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetProgramData Exception - " + ex.Message);
        }
    }

    private async Task GetToolDataById(CncToolData cncToolData, string strToolId)
    {
        try
        {
            if (!string.IsNullOrEmpty(strToolId))
            {
                int nToolId = 0;

                if (int.TryParse(strToolId, out nToolId))
                {
                    cncToolData.ToolId = strToolId;
                    cncToolData.ToolD = await _cNCConnector.RetrieveData("ToolD_" + strToolId);
                    cncToolData.ToolS = await _cNCConnector.RetrieveData("ToolS_" + strToolId);
                    cncToolData.ToolF = await _cNCConnector.RetrieveData("ToolF_" + strToolId);
                    cncToolData.ToolR = await _cNCConnector.RetrieveData("ToolR_" + strToolId);
                    cncToolData.ToolN = await _cNCConnector.RetrieveData("ToolN_" + strToolId);
                    cncToolData.ToolB = await _cNCConnector.RetrieveData("ToolB_" + strToolId);
                }
            }
            else
            {
                _logger.LogError("Data.BrokenToolData.BrkToolId is null.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolDataById Exception - " + ex.Message);
        }
    }
}
