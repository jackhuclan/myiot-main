// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class LotInfoRequestModel : EQPReportModel<EQPReportHeader, LotInfoRequestBody, EQPReportResult>
{
    public LotInfoRequestModel(EQPReportHeader header, LotInfoRequestBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
