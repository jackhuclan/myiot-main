// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgAutoDrill.Fundation.Iot.Models;

namespace VgEAPClient.Common.CNC;

/// <summary>
/// 数据采集项
/// </summary>
public class DataCollectionMetric
{
    public bool Enabled { get; set; } = false;
    public string MetricName { get; set; } = string.Empty;
    public DateTime CollectionTime { get; set; } = DateTime.Now;
    public string MetricValue { get; set; } = string.Empty;
    public TimeSpan CollectionInterval { get; set; } = TimeSpan.FromSeconds(2);
    public TimeSpan CollectionExpired { get; set; } = TimeSpan.FromSeconds(2);
    public DeviceKind CNCKind { get; set; } = DeviceKind.Unknown;
    public DataCollectionProtocal Protocal { get; set; } = DataCollectionProtocal.Unspecified;
}
