// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class LotInfoRequestBody : EQPReportBody
{
    public string? LotID { get; set; }
    public string? ItemNum { get; set; }

}
