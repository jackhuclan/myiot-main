using VgAutoDrill.Fundation.CNC.Packet;

namespace VgAutoDrill.Fundation.CNC.TCPExtend;

class TCPResultModel
{
    public AutoResetEvent ResetEvent { get; private set; } = new AutoResetEvent(false);
    public SMDncPacket Result { get; set; }
}
