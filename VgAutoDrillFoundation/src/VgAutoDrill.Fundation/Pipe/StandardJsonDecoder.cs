using System.Text.Json;

namespace VgAutoDrill.Fundation.Pipe;

internal class StandardJsonDecoder : IPacketDecoder
{
    public T? Decode<T>(ReadOnlySpan<byte> buffer)
    {
        return JsonSerializer.Deserialize<T>(buffer.ToArray());
    }
}
