// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPCompletedReportModel : EQPReportModel<EQPReportHeader, EQPCompletedReportBody, EQPReportResult>
{
    public EQPCompletedReportModel(EQPReportHeader header, EQPCompletedReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
