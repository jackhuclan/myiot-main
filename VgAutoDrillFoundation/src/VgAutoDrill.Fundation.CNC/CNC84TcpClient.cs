using System.Collections.Concurrent;
using System.Net;
using Microsoft.Extensions.Logging;
using SuperSocket.ClientEngine;
using VgAutoDrill.Fundation.CNC.Packet;
using VgAutoDrill.Fundation.CNC.TCPExtend;

namespace VgAutoDrill.Fundation.CNC;

internal static class CNC84TcpClient
{
    public static bool IsConnected
    {
        get
        {
            if (_client != null)
            {
                return _client.IsConnected;
            }
            return false;
        }
    }

    private static ILogger _logger = null;
    private static ILoggerFactory _loggerFactory = null;
    private static EasyClient<CNCDataPackageInfo> _client = null;
    private static string _ip = "127.0.0.1";
    private static int _port = 33822;
    private static readonly ConcurrentDictionary<int, TCPResultModel> _resultCallback = new ConcurrentDictionary<int, TCPResultModel>();
    private static bool _autoReconn = false;
    private static Timer _timer;
    private static readonly ConcurrentDictionary<int, Func<SMDncPacket, Task>> _adviceCallback = new ConcurrentDictionary<int, Func<SMDncPacket, Task>>();
    private static readonly ConcurrentDictionary<int, int> _adviceHash = new ConcurrentDictionary<int, int>();

    public static void RegisterCallback(int requestId, TCPResultModel resultModel)
    {
        _resultCallback.AddOrUpdate(requestId, resultModel, (k, r) => resultModel);
    }

    public static void RegisterAdvice(int adviceType, Func<SMDncPacket, Task> callback)
    {
        _adviceCallback.AddOrUpdate(adviceType, callback, (k, r) =>
        {
            r += callback;
            return r;
        });
    }

    public static void InitClient(ILoggerFactory loggerFactory, string ip = "127.0.0.1", int port = 33822)
    {
        _logger = loggerFactory.CreateLogger("CNC84TcpClient");
        _loggerFactory = loggerFactory;
        _ip = ip;
        _port = port;
        CreateClient(loggerFactory);
    }

    public static void InitClient(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        CreateClient(loggerFactory);
    }

    private static async Task<bool> ConnectClient()
    {
        if (_client != null)
        {
            var hasConnect = await _client.ConnectAsync(new IPEndPoint(IPAddress.Parse(_ip), _port));
            if (hasConnect)
            {
                _logger.LogInformation("Cnc84链接成功");
            }
            return hasConnect;
        }
        return false;
    }

    private static void CreateClient(ILoggerFactory loggerFactory)
    {
        _client = new EasyClient<CNCDataPackageInfo>();
        _client.Initialize(new CNC84Filter(loggerFactory));
        _client.NewPackageReceived += Client_NewPackageReceived;
        _client.Closed += Client_Closed;
    }

    private static void Client_Closed(object sender, EventArgs e)
    {
        if (_autoReconn)
        {
            Task.Run(ConnectClient);
        }
        _logger.LogInformation("Cnc84链接断开");
    }

    private static void Client_NewPackageReceived(object sender, PackageEventArgs<CNCDataPackageInfo> e)
    {
        var package = e.Package.Key;
        if (package.CmdType == 0)
        {
            return;
        }
        var cmdType = package.CmdType;
        if (3 == cmdType)
        {
            var adviceType = package.CNC.Advice.AdviceType;
            var messageHash = package.CNC.Advice.GetHashCode();
            bool isSame = false;
            _adviceHash.AddOrUpdate(adviceType, messageHash, (k, v) =>
            {
                if (v == messageHash)
                {
                    isSame = true;
                }
                return messageHash;
            });
            if (isSame)
            {
                _client.Send(CommFunc.Encoding.GetBytes(package.ToString()));
                return;
            }

            _logger.LogInformation("222222  SendCmd  : " + CommFunc.Encoding.GetBytes(package.ToString()));
            _client.Send(CommFunc.Encoding.GetBytes(package.ToString()));
            if (_adviceCallback.TryGetValue(package.CNC.Advice.AdviceType, out Func<SMDncPacket, Task> callback))
            {
                Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        await callback(package);
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
        else
        {
            //LogHelper.Debug(XmlConventer.Serialize(e.Package.Key));
            var requestId = -1;
            switch (cmdType)
            {
                case 1:
                    int.TryParse(package.CNC.Request.Value, out requestId);
                    break;

                case 2:
                    int.TryParse(package.CNC.Execute.Value, out requestId);
                    break;

                case 4:
                    int.TryParse(package.CNC.AdviceStart.Value, out requestId);
                    break;

                case 5:
                    int.TryParse(package.CNC.AdviceStop.Value, out requestId);
                    break;

                default: break;
            }
            if (requestId > 0 && _resultCallback.TryRemove(requestId, out TCPResultModel resultModel))
            {
                resultModel.Result = package;
                resultModel.ResetEvent.Set();
            }
        }
    }

    public static async Task ConnectForever()
    {
        if (_client == null)
        {
            CreateClient(_loggerFactory);
        }
        if (_timer == null)
        {
            _timer = new Timer(TimerCallback, null, 10000, 10000);
        }
        _timer.Change(1000, 10000);
        _autoReconn = true;
        if (!_client.IsConnected)
        {
            await ConnectClient();
        }
    }

    private static void TimerCallback(object state)
    {
        if (!_autoReconn)
        {
            _timer.Dispose();
        }
        else if (_client != null && !_client.IsConnected)
        {
            Task.Run(ConnectClient);
        }
    }

    public static async Task<bool> Connect()
    {
        return await ConnectClient();
    }

    public static async Task<bool> Close()
    {
        if (!_autoReconn)
        {
            if (_client != null)
            {
                if (_client.IsConnected)
                {
                    return await _client.Close();
                }
            }
            return true;
        }
        return false;
    }

    public static void SendCmd(byte[] msg)
    {
        Thread.Sleep(100);
        _client.Send(msg);
    }
}
