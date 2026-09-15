using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;

namespace VgAutoDrill.Fundation.CNC.TCPExtend;

class CNC84Filter : TerminatorReceiveFilter<CNCDataPackageInfo>
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<CNC84Filter> _logger;
    public CNC84Filter(ILoggerFactory loggerFactory)
        : base(CommFunc.Encoding.GetBytes("</SMDNCPACKET>"))
    {
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<CNC84Filter>();
    }
    public override CNCDataPackageInfo ResolvePackage(IBufferStream bufferStream)
    {
        var data = bufferStream.ReadString((int)bufferStream.Length, CommFunc.Encoding);
        bufferStream.Clear();
        data = data.Trim('\0');
        data = data.ToUpper();
        var obj = XmlConventer.Deserialize(data);
        if (obj != null)
        {
            return new CNCDataPackageInfo()
            {
                Key = obj
            };
        }
        _logger.LogDebug($"origin data -- {CommFunc.Encoding.EncodingName}---> {data} ");
        return new CNCDataPackageInfo()
        {
            Key = new Packet.SMDncPacket()
        };
    }

}
