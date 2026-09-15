using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.CNC.Packet;
using VgAutoDrill.Fundation.CNC.TCPExtend;

namespace VgAutoDrill.Fundation.CNC;

public class CNCClient
{
    private readonly ILogger<CNCClient> _logger;
    private readonly ILoggerFactory _loggerFactory;
    public CNCClient(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _logger = _loggerFactory.CreateLogger<CNCClient>();
    }
    private int _requestId = 0;

    public bool IsConnect
    {
        get
        {
            return CNC84TcpClient.IsConnected;
        }
    }
    public void SendCmd(SMDncPacket packet)
    {
        var message = XmlConventer.Serialize(packet);

        CNC84TcpClient.SendCmd(CommFunc.Encoding.GetBytes(message));
    }

    public SMDncPacket SendAndRecive(SMDncPacket packet)
    {
        int requestId = CommFunc.GetRequestId();
        var message = XmlConventer.Serialize(packet, requestId);
        var result = new TCPResultModel();
        CNC84TcpClient.RegisterCallback(requestId, result);
        CNC84TcpClient.SendCmd(CommFunc.Encoding.GetBytes(message));

        if (result.ResetEvent.WaitOne(5000))
        {
            return result.Result;
        }
        return null;
    }

    /// <summary>
    /// packet 需要是 AdviceStart
    /// </summary>
    /// <param name="packet"></param>
    /// <param name="callback"></param>
    public void RegisterAdvice(SMDncPacket packet, Func<SMDncPacket, Task> callback)
    {
        var cmdType = packet.CmdType;
        if (cmdType != 4) return;
        var adviceType = packet.CNC.AdviceStart.AdviceType;
        int requestId = CommFunc.GetRequestId();
        var message = XmlConventer.Serialize(packet, requestId);
        var result = new TCPResultModel();
        CNC84TcpClient.RegisterCallback(requestId, result);
        CNC84TcpClient.SendCmd(CommFunc.Encoding.GetBytes(message));

        if (result.ResetEvent.WaitOne(10000))
        {
            CNC84TcpClient.RegisterAdvice(adviceType, callback);
        }
    }

    private int GetRequestId()
    {
        return ++_requestId;
    }
}

