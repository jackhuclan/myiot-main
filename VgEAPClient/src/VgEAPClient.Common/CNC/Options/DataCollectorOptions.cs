// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.Options;

public class DataCollectorOptions
{
    public DrillAlarmDataCollectorOptions AlarmOptions { get; set; } = new();
    public DrillBrokenKnifeDataCollectorOptions BrokenKnifeOptions { get; set; } = new();
    public DrillBufferDataCollectorOptions BufferOptions { get; set; } = new();
    public DrillCCDDataCollectorOptions CCDOptions { get; set; } = new();
    public DrillCommonDataCollectorOptionsA CommonDataOptionsA { get; set; } = new();
    public DrillCommonDataCollectorOptionsB CommonDataOptionsB { get; set; } = new();
    public DrillCommonDataCollectorOptionsC CommonDataOptionsC { get; set; } = new();
    public DrillToolMeasureDataCollectorOptions ToolMeasureDataOptions { get; set; } = new();
    public DrillToolParamDataCollectorOptions ToolParamDataOptions { get; set; } = new();
    public DrillColletCleanDataCollectorOptions ColletCleanDataOptions { get; set; } = new();
    public Write9XNodeDataCollectorOptions Write9XNodeDataOptions { get; set; } = new();
    public FirstPcsDataCollectorOptions FirstPcsDataOptions { get; set; } = new();
    public DrillStatusDataCollectorOptions StatusDataOptions { get; set; } = new();

    public List<DataCollectionMetric> Metrics { get; set; } = new();
}
