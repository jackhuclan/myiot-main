// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC;

public class CncAlarmData
{
    public string AlarmId { get; set; } = string.Empty;
    public string AlarmDesc { get; set; } = string.Empty;
    public string AlarmStartTime { get; set; } = string.Empty;
    public string AlarmEndTime { get; set; } = string.Empty;

    public string GetBrief()
    {
        return $"{AlarmId}.'{AlarmDesc}'.{AlarmStartTime}.{AlarmEndTime}";
    }
}
