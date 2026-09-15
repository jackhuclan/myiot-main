// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPDateTimeRequestModel : EQPReportModel<EQPReportHeader, EQPDateTimeRequestBody, EQPReportResult>
{
    public EQPDateTimeRequestModel(EQPReportHeader header, EQPDateTimeRequestBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
