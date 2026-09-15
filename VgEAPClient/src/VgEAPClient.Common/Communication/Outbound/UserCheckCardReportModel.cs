// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class UserCheckCardReportModel : EQPReportModel<EQPReportHeader, UserCheckCardReportBody, EQPReportResult>
{
    public UserCheckCardReportModel(EQPReportHeader header, UserCheckCardReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
