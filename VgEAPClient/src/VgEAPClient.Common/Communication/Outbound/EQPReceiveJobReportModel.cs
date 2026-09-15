// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPReceiveJobReportModel : EQPReportModel<EQPReportHeader, EQPReceiveJobReportBody, EQPReportResult>
{
    public EQPReceiveJobReportModel(EQPReportHeader header, EQPReceiveJobReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
