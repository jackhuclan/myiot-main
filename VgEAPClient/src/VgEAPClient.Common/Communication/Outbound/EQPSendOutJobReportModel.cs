// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPSendOutJobReportModel : EQPReportModel<EQPReportHeader, EQPSendOutJobReportBody, EQPReportResult>
{
    public EQPSendOutJobReportModel(EQPReportHeader header, EQPSendOutJobReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
