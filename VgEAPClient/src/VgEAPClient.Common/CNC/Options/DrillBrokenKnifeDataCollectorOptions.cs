// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.Options;

public class DrillBrokenKnifeDataCollectorOptions
{
    public bool Enabled { get; set; } = false;
    public TimeSpan CollectionTimeSpan { get; set; } = TimeSpan.FromSeconds(5);
}
