// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class BrokenKnifeAlarmReportModel : EQPReportModel<EQPReportHeader, BrokenKnifeAlarmReportBody, EQPReportResult>
{
    public BrokenKnifeAlarmReportModel(EQPReportHeader header, BrokenKnifeAlarmReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
