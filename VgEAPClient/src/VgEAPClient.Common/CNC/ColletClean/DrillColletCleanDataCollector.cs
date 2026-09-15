// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using VgEAPClient.Common.CNC.Options;

namespace VgEAPClient.Common.CNC.ColletClean;

internal class DrillColletCleanDataCollector : AbstractDataCollector<DrillColletCleanData>
{
    private readonly ILogger<DrillColletCleanDataCollector> _logger;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillColletCleanDataCollectorOptions _options;

    public DrillColletCleanDataCollector(ICNCOperatorProvider cNCOperatorProvider,
        ILoggerFactory loggerFactory,
        DrillColletCleanDataCollectorOptions options)
        : base(cNCOperatorProvider.GetCNCConnector(), loggerFactory, options.CollectionTimeSpan)
    {
        _logger = loggerFactory.CreateLogger<DrillColletCleanDataCollector>();
        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _options = options;
    }

    protected override bool Enabled => _options.Enabled;

    protected override async Task PullData()
    {
        try
        {
            Data.CncShowText = await _cNCConnector.RetrieveData("CncShowText");
            if (Data.CncShowText == "Collet clean begin")
            {
                lock (Data.LockCollectCleanData)
                {
                    var dateTime = DateTime.Now;
                    Data.ColletCleanStartTime = dateTime;
                    Data.ColletCleanEndTime = dateTime;
                }

                Data.ColletCleanStartHole = await _cNCConnector.RetrieveData("RunDrillHits");

                _logger.LogInformation($"Collet clean begin- {Data.ColletCleanStartTime}, ColletCleanStartHole {Data.ColletCleanStartHole}");
            }
            else if (Data.CncShowText == "Collet clean end")
            {
                try
                {
                    Data.ColletCleanEndHole = await _cNCConnector.RetrieveData("RunDrillHits");

                    _logger.LogInformation($"Collet clean end- {Data.ColletCleanStartTime}, ColletCleanStartHole {Data.ColletCleanStartHole} - ColletCleanEndHole {Data.ColletCleanEndHole}");
                    if (Data.ColletCleanStartHole != Data.ColletCleanEndHole)
                    {
                        Data.ColletCleanStartHole = string.Empty;
                        return;
                    }

                    lock (Data.LockCollectCleanData)
                    {
                        Data.ColletCleanEndTime = DateTime.Now;

                        if (Data.ColletCleanStartTime == default || Data.ColletCleanEndTime <= Data.ColletCleanStartTime)
                        {
                            return;
                        }

                        TimeSpan curCleanTime = Data.ColletCleanEndTime.Subtract(Data.ColletCleanStartTime);
                        Data.ColletCleanTotalMins += (Data.ColletCleanEndTime - Data.ColletCleanStartTime).TotalMinutes;
                        _logger.LogError(DateTime.Now.ToString() + $"清洗夹头结束- {Data.ColletCleanEndTime} , 清洗夹头开始时间 {Data.ColletCleanStartTime}  累加清洗时间 : {Data.ColletCleanTotalMins} 分钟  单次清洗时间： {curCleanTime.TotalMinutes} 分钟");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Collet clean end - " + ex.Message);
                }
                finally
                {
                    Data.ColletCleanStartTime = default;
                    Data.ColletCleanEndTime = default;
                }
            }

            Data.ShiftsStartTime = await _cNCConnector.RetrieveData("ShiftsStartTime");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
