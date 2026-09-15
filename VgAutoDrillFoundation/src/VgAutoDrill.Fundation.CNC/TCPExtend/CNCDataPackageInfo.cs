using SuperSocket.ProtoBase;
using VgAutoDrill.Fundation.CNC.Packet;

namespace VgAutoDrill.Fundation.CNC.TCPExtend;

class CNCDataPackageInfo : IPackageInfo<SMDncPacket>, IPackageInfo
{
    public SMDncPacket Key { get; set; }
}
