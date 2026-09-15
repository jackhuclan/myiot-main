// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPCurrentChangeRecipeReportModel : EQPReportModel<EQPReportHeader, EQPCurrentChangeRecipeReportBody, EQPReportResult>
{
    public EQPCurrentChangeRecipeReportModel(EQPReportHeader header, EQPCurrentChangeRecipeReportBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }
}
