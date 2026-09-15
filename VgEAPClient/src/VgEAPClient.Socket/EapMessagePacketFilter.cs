// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using SuperSocket.ProtoBase;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;

public class EapMessagePacketFilter : BeginEndMarkPipelineFilter<EapMessage>
{
    private static readonly byte[] BeginBytes = new byte[3] { (byte)'S', (byte)'T', (byte)'X' };
    private static readonly byte[] EndBytes = new byte[3] { (byte)'E', (byte)'T', (byte)'X' };

    public EapMessagePacketFilter() : base(BeginBytes, EndBytes)
    {
    }
}
