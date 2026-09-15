// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPRunningModeReportModel : EQPReportModel<EQPReportHeader, EQPRunningModeReportBody, EQPReportResult>
{
    public EQPRunningModeReportModel(EQPReportHeader header, EQPRunningModeReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
