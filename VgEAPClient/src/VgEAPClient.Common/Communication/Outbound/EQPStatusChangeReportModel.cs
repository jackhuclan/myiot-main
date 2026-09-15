// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPStatusChangeReportModel : EQPReportModel<EQPReportHeader, EQPStatusChangeReportBody, EQPReportResult>
{
    public EQPStatusChangeReportModel(EQPReportHeader header, EQPStatusChangeReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
