// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.Common;

public class AutoListRecord
{
    public DateTime StartRunTime { get; set; } = DateTimeUtil.BeginOf1970;
    public DateTime EndRunTime { get; set; } = DateTimeUtil.BeginOf1970;

    public long HitCount { get; set; } = 0;
    public double RouMeters { get; set; } = 0;

    public double WorkDurMin { get; set; } = 0;
    public double StopDurMin { get; set; } = 0;
}
