// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class IPPortReportModel : EQPReportModel<EQPReportHeader, IPPortReportBody, EQPReportResult>
{
    public IPPortReportModel(EQPReportHeader header, IPPortReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
