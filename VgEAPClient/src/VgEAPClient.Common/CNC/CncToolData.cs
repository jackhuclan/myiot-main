// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC;

public class CncToolData
{
    public static CncToolData Empty = new CncToolData();
    public string ToolId { get; set; } = string.Empty;
    public string ToolD { get; set; } = string.Empty;
    public string ToolS { get; set; } = string.Empty;
    public string ToolF { get; set; } = string.Empty;
    public string ToolR { get; set; } = string.Empty;
    public string ToolN { get; set; } = string.Empty;
    public string ToolB { get; set; } = string.Empty;
}
