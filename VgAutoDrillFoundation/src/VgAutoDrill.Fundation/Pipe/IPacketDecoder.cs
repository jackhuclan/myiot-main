namespace VgAutoDrill.Fundation.Pipe;

public interface IPacketDecoder
{
    T? Decode<T>(ReadOnlySpan<byte> buffer);
}
