// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.ToolParam;

public class DrillToolParamData
{
    public int[] ToolParamPodCount { get; set; } = new int[999];
    public double DrillZ { get; set; } = 0;

    public readonly SemaphoreSlim LockListToolParam = new SemaphoreSlim(1, 1);

    public List<ToolParam> ListToolParam = new List<ToolParam>();
}

public class ToolParam
{
    public int ToolId { get; set; } = 0;
    public string ToolD { get; set; } = string.Empty;
    public string ToolS { get; set; } = string.Empty;
    public string ToolF { get; set; } = string.Empty;
    public string ToolR { get; set; } = string.Empty;
    public string ToolN { get; set; } = string.Empty;
    public string ToolB { get; set; } = string.Empty;
    public string ToolZOffset { get; set; } = string.Empty;

    /// <summary>
    /// 下刀深度
    /// </summary>
    public string ToolZ { get; set; } = string.Empty;

    /// <summary>
    /// 钻板速度
    /// </summary>
    public string ToolSpeed { get; set; } = string.Empty;
}
