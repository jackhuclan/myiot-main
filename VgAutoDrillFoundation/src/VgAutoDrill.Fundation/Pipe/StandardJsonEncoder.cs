using System.Text.Json;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Fundation.Pipe;

internal class StandardJsonEncoder : IPacketEncoder
{
    public byte[] Encode<T>(T packet)
    {
        return JsonSerializer.Serialize(packet).GetBytes();
    }
}
