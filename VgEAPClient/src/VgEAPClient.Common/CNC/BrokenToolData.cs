// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC;

public class BrokenToolData
{
    public string BrkToolId { get; set; } = string.Empty;
    public string BrkToolDia { get; set; } = string.Empty;
    public string BrkToolSpindle { get; set; } = string.Empty;
    public string BrokenInfo { get; set; } = string.Empty;

    public string GetBrief()
    {
        return $"{BrkToolId} d:{BrkToolDia} sp:{BrkToolSpindle}";
    }
}
