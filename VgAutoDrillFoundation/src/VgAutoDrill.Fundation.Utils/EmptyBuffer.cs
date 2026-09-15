namespace VgAutoDrill.Fundation.Utils;

public static class EmptyBuffer
{
#if NET452
    public static readonly byte[] Array = new byte[0];
#else
    public static readonly byte[] Array = System.Array.Empty<byte>();
#endif

    public static readonly ArraySegment<byte> ArraySegment = new ArraySegment<byte>(Array, 0, 0);
}
