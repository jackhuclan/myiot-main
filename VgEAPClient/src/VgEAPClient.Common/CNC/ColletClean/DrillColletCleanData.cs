// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.ColletClean;

/// <summary>
/// 目前只实现了采集 9X
/// </summary>
public class DrillColletCleanData
{
    private string _shiftsStartTime = string.Empty;

    public readonly object LockCollectCleanData = new object();

    public string CncShowText { get; set; } = string.Empty;
    public DateTime ColletCleanStartTime { get; set; } = default;
    public DateTime ColletCleanEndTime { get; set; } = default;
    public double ColletCleanTotalMins { get; set; } = 0;
    public string ColletCleanStartHole { get; set; } = string.Empty;
    public string ColletCleanEndHole { get; set; } = string.Empty;

    public string ShiftsStartTime
    {
        get => _shiftsStartTime;
        set
        {
            bool bChanged = _shiftsStartTime != value;
            _shiftsStartTime = value;

            if (bChanged)
            {
                lock (LockCollectCleanData)
                {
                    ColletCleanTotalMins = 0;
                    ColletCleanStartTime = default;
                    ColletCleanEndTime = default;
                }
            }
        }
    }
}
