// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPAlarmReportModel : EQPReportModel<EQPReportHeader, EQPAlarmReportBody, EQPReportResult>
{
    public EQPAlarmReportModel(EQPReportHeader header, EQPAlarmReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
