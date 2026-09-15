// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPAlarmReportBody : EQPReportBody
{
    public string? AlarmID { get; set; }
    public string? AlarmLevel { get; set; }
    public string? AlarmStatus { get; set; }
    public string? AlarmText { get; set; }
}
