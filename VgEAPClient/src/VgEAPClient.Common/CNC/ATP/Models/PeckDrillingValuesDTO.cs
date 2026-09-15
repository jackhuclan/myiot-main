// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/*
 * 分段钻实体类，ATP解析的子类
 */

namespace VgEAPClient.Common.CNC.ATP.Models;

public class PeckDrillingValuesDTO
{
    public int toolNumber { get; set; }
    public int partialStrokeNumber { get; set; }
    public string lowerPlane { get; set; } = string.Empty;
    public float retractionPlane { get; set; }
    public float infeedRate { get; set; }
    public float retractRate { get; set; }
}
