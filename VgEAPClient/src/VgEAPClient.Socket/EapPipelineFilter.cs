// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Buffers;
using SuperSocket.ProtoBase;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;
public class EapPipelineFilter : IPipelineFilter<EapMessage>
{
    public IPackageDecoder<EapMessage>? Decoder { get; set; }

    public IPipelineFilter<EapMessage>? NextFilter { get; }

    public object? Context { get; set; }

    public EapPipelineFilter() : base()
    {

    }

    public virtual EapMessage Filter(ref SequenceReader<byte> reader)
    {
        EapMessage eap = new EapMessage();

        ReadOnlyMemory<byte> start = (ReadOnlyMemory<byte>)new byte[] { 64, 63 }; // @?
        ReadOnlyMemory<byte> end = (ReadOnlyMemory<byte>)new byte[] { 63, 64 }; // ?@

        if (!reader.TryReadTo(out ReadOnlySequence<byte> sequence, end.Span, advancePastDelimiter: false))
        {
            reader.AdvanceToEnd();
            return eap;
        }

        try
        {
            var readerbyte = new SequenceReader<byte>(sequence);
            if (!readerbyte.TryReadTo(out ReadOnlySequence<byte> sequence2, start.Span, advancePastDelimiter: true))
            {
                return eap;
            }
            eap.Header.MessageName = readerbyte.ReadString();
            return eap;
        }
        finally
        {
            reader.Advance(end.Length);
        }
    }
    public virtual void Reset()
    {

    }

    public EapMessage Deserialize(string Text)
    {
        return EapMessageXmlHelper.Deserialize<EapMessage>(Text) ?? new EapMessage();
    }
}
