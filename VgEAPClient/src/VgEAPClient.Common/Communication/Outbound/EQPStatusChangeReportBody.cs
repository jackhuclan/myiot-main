// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPStatusChangeReportBody : EQPReportBody
{
    public string? Status { get; set; }//1：Auto 2:Manua ||1：IDLE，2：RUN 3：DOWN，4：PM
}
