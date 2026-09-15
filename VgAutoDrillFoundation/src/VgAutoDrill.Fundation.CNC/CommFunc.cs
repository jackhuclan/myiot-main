using System.Text;

using Microsoft.Extensions.Logging;

namespace VgAutoDrill.Fundation.CNC;

public static class CommFunc
{
    static CommFunc()
    {
        System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding = Encoding.GetEncoding("gb2312");
    }
    private static CNCClient _client;
    private static object _lock = new object();
    private static bool _hasInit = false;
    private static AutoResetEvent _event = new AutoResetEvent(false);
    private static bool _isFinishInit = false;
    public static Encoding Encoding { get; set; } = Encoding.UTF8;
    public static void Init(ILoggerFactory _loggerFactory, string ip, int port)
    {
        CNC84TcpClient.InitClient(_loggerFactory, ip, port);
        Task.Factory.StartNew(async () => await ConnectTCP());
        _hasInit = true;


    }

    public static void Init(ILoggerFactory _loggerFactory)
    {

        Encoding = Encoding.GetEncoding("gb2312");
        CNC84TcpClient.InitClient(_loggerFactory);
        Task.Factory.StartNew(async () => await ConnectTCP());
        _hasInit = true;

    }

    private static async Task ConnectTCP()
    {
        await CNC84TcpClient.ConnectForever();
        _isFinishInit = true;
        _event.Set();
    }

    public static CNCClient GetClient(ILoggerFactory _loggerFactory)
    {
        if (!_hasInit) throw new AggregateException("尚未Init");
        if (!_isFinishInit)
        {
            Console.WriteLine("等待cnc84连接成功");
            _event.WaitOne();
        }
        if (_client == null)
        {
            lock (_lock)
            {
                if (_client == null)
                {
                    _client = new CNCClient(_loggerFactory);
                }
            }
        }
        return _client;
    }

    public static bool DisposeClient()
    {
        return true;
    }

    private static object _requestLock = new object();
    private static int _requestId = 1;
    public static int GetRequestId()
    {
        lock (_requestLock)
        {
            _requestId++;
            if (_requestId > 3000)
                _requestId = 1;
            return _requestId;
        }
    }

    public const string NULLVALUE = "[NULL]";
    public const string REQUESTID = "[REQUESTID]";
}
