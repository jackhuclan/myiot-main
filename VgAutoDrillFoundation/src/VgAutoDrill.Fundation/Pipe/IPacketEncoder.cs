namespace VgAutoDrill.Fundation.Pipe;

public interface IPacketEncoder
{
    byte[] Encode<T>(T packet);
}
