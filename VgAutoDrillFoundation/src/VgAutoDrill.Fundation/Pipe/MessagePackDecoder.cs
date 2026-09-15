using MessagePack;

namespace VgAutoDrill.Fundation.Pipe;

internal class MessagePackDecoder : IPacketDecoder
{
    public T Decode<T>(ReadOnlySpan<byte> buffer)
    {
        return MessagePackSerializer.Deserialize<T>(buffer.ToArray());
    }
}
