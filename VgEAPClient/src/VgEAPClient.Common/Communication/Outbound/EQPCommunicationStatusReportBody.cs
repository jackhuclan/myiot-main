// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPCommunicationStatusReportBody : EQPReportBody
{
    public string? CommunicationStatus { get; set; }//1:CIM On在线模式，2:CIM Off离线模式
}
