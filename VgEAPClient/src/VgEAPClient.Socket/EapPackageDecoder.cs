// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using SuperSocket.ProtoBase;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;

public class EapPackageDecoder : IPackageDecoder<EapMessage>
{
    public EapMessage Decode(ref ReadOnlySequence<byte> buffer, object context)
    {
        var reader = new SequenceReader<byte>(buffer);
        var body = reader.ReadString();
        var obj = EapMessageXmlHelper.Deserialize<EapMessage>(body);
        return obj ?? new EapMessage();
    }
}
