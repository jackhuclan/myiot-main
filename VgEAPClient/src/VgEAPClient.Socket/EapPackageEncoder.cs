// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using SuperSocket.ProtoBase;
using VgAutoDrill.Fundation.Utils;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;

public class EapPackageEncoder : IPackageEncoder<EapMessage>
{
    public int Encode(IBufferWriter<byte> writer, EapMessage pack)
    {
        string message = EapMessageXmlHelper.Serialize(pack);
        var data = $"STX{message}ETX".GetBytes();
        writer.Write(data);
        return data.Length;
    }
}
