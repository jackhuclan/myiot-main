using System.Text;

namespace VgAutoDrill.Fundation.Utils;

public static class ArraySegmentExtensions
{
    public static string ToStr(this ArraySegment<byte> segment)
    {
        if (segment == EmptyBuffer.ArraySegment
            || segment.Array == null)
            return string.Empty;

        return Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
    }
}
