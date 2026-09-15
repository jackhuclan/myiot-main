using MessagePack;

namespace VgAutoDrill.Fundation.Pipe;

internal class MessagePackEncoder : IPacketEncoder
{
    public byte[] Encode<T>(T packet)
    {
        return MessagePackSerializer.Serialize(packet);
    }
}
