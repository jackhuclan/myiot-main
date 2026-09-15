// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using SuperSocket.ProtoBase;
using VgAutoDrill.Fundation.CNC.Packet;
using VgAutoDrill.Fundation.Utils;

namespace VgEAPClient.Common.CNC;

internal class SMDncPacketFilter : BeginEndMarkPipelineFilter<SMDncPacket>
{
    private static readonly byte[] BeginBytes = "<SMDNCPACKET".GetBytes();
    private static readonly byte[] EndBytes = "</SMDNCPACKET>".GetBytes();

    public SMDncPacketFilter() : base(BeginBytes, EndBytes)
    {
    }
}
