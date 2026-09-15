// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using System.Text;
using SuperSocket.ProtoBase;
using VgAutoDrill.Fundation.CNC.Packet;

namespace VgEAPClient.Common.CNC;

internal class SMDncPacketEncoder : IPackageEncoder<SMDncPacket>
{
    private readonly Encoding _encoding;

    public SMDncPacketEncoder()
    {
        _encoding = Encoding.UTF8;
    }

    public int Encode(IBufferWriter<byte> writer, SMDncPacket pack)
    {
        var content = pack.ToString();
        writer.Write(content, _encoding);
        return content.Length;
    }
}
