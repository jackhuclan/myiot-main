// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Inbound;

public class DateTimeCommandBody : EQPReportBody
{
    public string DateTime { get; set; } = string.Empty;
}
