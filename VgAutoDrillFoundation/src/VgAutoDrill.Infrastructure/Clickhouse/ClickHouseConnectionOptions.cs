// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgAutoDrill.Infrastructure.Clickhouse;

public class ClickHouseConnectionOptions
{
    /// <summary>
    /// 
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public bool Compression { get; set; } = true;
    /// <summary>
    /// 
    /// </summary>
    public bool Session { get; set; } = false;
    /// <summary>
    /// 
    /// </summary>
    public bool CustomDecimals { get; set; } = true;
    public bool Enabled { get; set; } = true;
}
