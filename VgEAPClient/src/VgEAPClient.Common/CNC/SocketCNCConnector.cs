// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperSocket.ClientEngine;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.Status;

namespace VgEAPClient.Common.CNC;

public class SocketCNCConnector : ICNCConnector, IDisposable
{
    public event Action? OnCNCConnected;

    public event Action? OnCNCDisconnected;

    public event Action<Exception>? OnCNCConnectException;

    public event Action<Exception>? OnCNCDisonnectException;

    public event Action<Exception>? OnCNCDataReceivedException;

    public event Action? OnCNCConnectFailed;

    public event Action? OnCNCDataReceived;

    public event Action? OnCNCClosed;

    public event Action? OnCNCError;

    public event Action? OnCNCDataSent;

    public event Action<Exception>? OnCNCDataSentException;

    private volatile bool _isConnected = false;
    private readonly AsyncTcpSession _socketCNC84 = new();
    private const int _receiveBufferSize = 10240;
    private readonly object _lockSendCNC84Socket = new object();
    private readonly Channel<string> channelSendCNC = Channel.CreateUnbounded<string>(new UnboundedChannelOptions() { SingleReader = true, SingleWriter = true });
    private readonly Channel<string> channelDDE = Channel.CreateUnbounded<string>(new UnboundedChannelOptions() { SingleReader = true, SingleWriter = true });
    private readonly ILogger<SocketCNCConnector> _logger;
    private readonly IAsyncTaskWaiter _asyncTaskWaiter;
    private static volatile int _refreshRouteCodes = 0;
    private readonly EAPClientOptions _eAPClientOptions;
    private string _oldCncStatusEc = string.Empty;
    private bool _isNewRun = false;
    private string _oldPgmRunStartTime = string.Empty;
    private string _oldPgmRunEndTime = string.Empty;
    private string _oldCncComm = string.Empty;
    private string _cncblockColor = string.Empty;

    private string _cncStatusData = string.Empty;

    private long _recvBytesOfSocket = 0;

    private DateTime _lastPrintSocketStatisticTime = DateTime.Now;
    private long _lastPrintRecvBytesOfSocket = 0;

    private DateTime _lastPrintCncStatusBriefTime = DateTime.Now.AddSeconds(-60);

    private readonly ReceiveBuffHoldAndParseForCNC8X _buffHold = null;

    private string _recvBytesLogFilePath = "";

    #region FromAutoList

    private const int AUTO_LIST_LINE_COUNT = 200;

    private CNC8XAutoListReaderAsync _cncAutoListReader = null;

    private volatile int _indexOfCurJob = 0;
    private DateTime _startTimeOfCurJob = DateTimeUtil.Future3000;
    private DateTime _endTimeOfCurJob = DateTimeUtil.Future3000;

    #endregion FromAutoList

    public SocketCNCConnector(ILogger<SocketCNCConnector> logger,
        IOptions<EAPClientOptions> options,
        IAsyncTaskWaiter asyncTaskWaiter)
    {
        _logger = logger;
        _asyncTaskWaiter = asyncTaskWaiter;
        _eAPClientOptions = options.Value;
        _socketCNC84.ReceiveBufferSize = _receiveBufferSize;
        _socketCNC84.DataReceived += socketCNC84_DataReceived;
        _socketCNC84.Connected += socketCNC84_Connected;
        _socketCNC84.Error += socketCNC84_Error;
        _socketCNC84.Closed += socketCNC84_Closed;

        InitEcList();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Task.Factory.StartNew(SendMsg2CNCLoop, TaskCreationOptions.LongRunning);
        Task.Factory.StartNew(ProcessChannelDDEData, TaskCreationOptions.LongRunning);

        Encoding enc = Encoding.Default;
        try
        {
            enc = Encoding.GetEncoding("gb2312");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get encoding of gb2312 failed ! ");
        }
        _buffHold = new VgEAPClient.Common.ReceiveBuffHoldAndParseForCNC8X(enc, 512000, "", BuffHoldOutputImp);

        _cncAutoListReader = new CNC8XAutoListReaderAsync(IsConnectionOK,
            ReadAutoListStartTimeImp, ReadAutoListLineImp, AutoListReaderLogImp, AUTO_LIST_LINE_COUNT);
        _cncAutoListReader.OnBatchReadEnd += AutoListReaderOnBatchReadEnd;

        if (_eAPClientOptions.CNC84GetJobInfoByAutoList)
        {
            _cncAutoListReader.Start();
        }

        if (_eAPClientOptions.IsDebug)
        {
            string dtText = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            _recvBytesLogFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"logs\\recv_bin_log_{dtText}.dat");
        }
    }

    private void InitEcList()
    {
        EcList = [];
        EcList.Add("0001");
        EcList.Add("0002");
        EcList.Add("0003");
        EcList.Add("0004");
        EcList.Add("0005");
        EcList.Add("0006");
        EcList.Add("0008");
        EcList.Add("0011");
        EcList.Add("0013");
        EcList.Add("0014");
        EcList.Add("0016");
        EcList.Add("0017");
        EcList.Add("0018");
        EcList.Add("0019");
        EcList.Add("0020");
        EcList.Add("0021");
        EcList.Add("0022");
        EcList.Add("0023");
        EcList.Add("0024");
        EcList.Add("0025");
        EcList.Add("0027");
        EcList.Add("0028");
        EcList.Add("0030");
        EcList.Add("0031");
        EcList.Add("0032");
        EcList.Add("0033");
        EcList.Add("0034");
        EcList.Add("0036");
        EcList.Add("0037");
        EcList.Add("0038");
        EcList.Add("0039");
        EcList.Add("0040");
        EcList.Add("0041");
        EcList.Add("0042");
        EcList.Add("0043");
        EcList.Add("0044");
        EcList.Add("0045");
        EcList.Add("0046");
        EcList.Add("0047");
        //EcList.Add("0048");
        EcList.Add("0049");
        EcList.Add("0052");
        EcList.Add("0054");
        EcList.Add("0056");
        EcList.Add("0057");
        EcList.Add("0058");
        EcList.Add("0060");
        EcList.Add("0061");
        EcList.Add("0062");
        EcList.Add("0063");
        EcList.Add("0072");
        EcList.Add("0073");
        EcList.Add("0074");
        EcList.Add("0075");
        EcList.Add("0076");
        EcList.Add("0077");
        EcList.Add("0078");
        EcList.Add("0080");
        EcList.Add("0081");
        EcList.Add("0082");
        EcList.Add("0084");
        EcList.Add("0090");
        EcList.Add("0091");
        EcList.Add("0093");
        EcList.Add("0094");
        EcList.Add("0095");
        EcList.Add("0096");
        EcList.Add("0100");
        EcList.Add("0101");
        EcList.Add("0102");
        EcList.Add("0103");
        EcList.Add("0104");
        EcList.Add("0105");
        EcList.Add("0106");
        EcList.Add("0112");
        EcList.Add("0130");
        EcList.Add("0144");
        EcList.Add("0146");
        EcList.Add("0147");
        EcList.Add("0148");
        EcList.Add("0149");
        EcList.Add("0157");
        EcList.Add("0162");
        EcList.Add("0163");
        EcList.Add("0164");
        EcList.Add("0165");
        EcList.Add("0166");
        EcList.Add("0167");
        EcList.Add("0176");
        EcList.Add("0181");
        EcList.Add("0182");
        EcList.Add("0183");
        EcList.Add("0184");
        EcList.Add("0186");
        EcList.Add("0188");
        EcList.Add("0189");
        EcList.Add("0190");
        EcList.Add("0191");
        EcList.Add("0192");
        EcList.Add("0193");
        EcList.Add("0194");
        EcList.Add("0195");
        EcList.Add("0196");
        EcList.Add("0197");
        EcList.Add("0198");
        EcList.Add("0300");
        EcList.Add("0301");
        EcList.Add("0302");
        EcList.Add("0501");
        EcList.Add("0502");
        EcList.Add("0503");
        EcList.Add("0504");
        EcList.Add("0505");
        EcList.Add("0506");
        EcList.Add("0507");
        EcList.Add("0900");
        EcList.Add("0901");
        EcList.Add("0903");
        EcList.Add("0904");
        EcList.Add("0905");
        EcList.Add("1501");
        EcList.Add("1502");
        EcList.Add("1503");
        EcList.Add("1504");
        EcList.Add("1505");
        EcList.Add("1506");
        EcList.Add("1507");
        EcList.Add("1508");
        EcList.Add("1509");
        EcList.Add("1510");
        EcList.Add("1511");
        EcList.Add("1512");
        EcList.Add("1513");
        EcList.Add("1514");
        EcList.Add("1515");
        EcList.Add("1516");
        EcList.Add("1517");
        EcList.Add("1518");
        EcList.Add("1519");
        EcList.Add("1522");
        EcList.Add("1523");
        EcList.Add("1524");
        EcList.Add("1525");
        EcList.Add("1526");
        EcList.Add("1527");
        EcList.Add("1528");
        EcList.Add("1529");
        EcList.Add("1530");
        EcList.Add("1531");
        EcList.Add("1532");
        EcList.Add("1533");
        EcList.Add("1534");
        EcList.Add("1538");
        EcList.Add("1539");
        EcList.Add("1540");
        EcList.Add("1541");
        EcList.Add("1542");
        EcList.Add("1543");
        EcList.Add("1544");
        EcList.Add("1545");
        EcList.Add("1546");
        EcList.Add("1547");
        EcList.Add("1548");
        EcList.Add("1549");
        EcList.Add("1550");
        EcList.Add("1551");
        EcList.Add("1552");
        EcList.Add("1553");
        EcList.Add("1554");
        EcList.Add("1555");
        EcList.Add("1556");
        EcList.Add("1557");
        EcList.Add("1558");
        EcList.Add("1559");
        EcList.Add("1563");
        EcList.Add("1564");
        EcList.Add("1565");
        EcList.Add("1566");
        EcList.Add("1567");
        EcList.Add("1568");
        EcList.Add("1569");
        EcList.Add("1570");
        EcList.Add("1571");
        EcList.Add("1572");
        EcList.Add("1577");
        EcList.Add("1578");
        EcList.Add("1580");
        EcList.Add("1581");
        EcList.Add("1582");
        EcList.Add("1583");
        EcList.Add("1584");
        EcList.Add("1585");
        EcList.Add("1586");
        EcList.Add("1587");
        EcList.Add("1588");
        EcList.Add("1589");
        EcList.Add("1590");
        EcList.Add("1591");
        EcList.Add("1592");
        EcList.Add("1593");
        EcList.Add("1594");
        EcList.Add("1595");
        EcList.Add("1596");
        EcList.Add("1597");
        EcList.Add("1598");
        EcList.Add("1599");
        EcList.Add("1600");
        EcList.Add("1601");
        EcList.Add("1602");
        EcList.Add("1603");
        EcList.Add("1604");
        EcList.Add("1605");
        EcList.Add("1606");
        EcList.Add("1607");
        EcList.Add("1608");
        EcList.Add("1609");
        EcList.Add("1610");
        EcList.Add("1611");
        EcList.Add("1612");
        EcList.Add("1613");
        EcList.Add("1614");
        EcList.Add("1615");
        EcList.Add("1616");
        EcList.Add("1617");
        EcList.Add("1618");
        EcList.Add("1622");
        EcList.Add("1623");
        EcList.Add("1624");
        EcList.Add("1625");
        EcList.Add("1626");
        EcList.Add("1627");
        EcList.Add("1630");
        EcList.Add("1631");
        EcList.Add("1632");
        EcList.Add("1633");
        EcList.Add("1634");
        EcList.Add("1635");
        EcList.Add("1636");
        EcList.Add("1637");
        EcList.Add("1640");
        EcList.Add("1641");
        EcList.Add("1642");
        EcList.Add("1643");
        EcList.Add("1644");
        EcList.Add("1645");
        EcList.Add("1646");
        EcList.Add("1647");
        EcList.Add("1648");
        EcList.Add("1650");
        EcList.Add("1651");
        EcList.Add("1652");
        EcList.Add("1653");
        EcList.Add("1654");
        EcList.Add("1656");
        EcList.Add("1657");
        EcList.Add("1658");
        EcList.Add("1661");
        EcList.Add("1662");
        EcList.Add("1663");
        EcList.Add("1664");
        EcList.Add("1665");
        EcList.Add("1666");
        EcList.Add("1667");
        EcList.Add("1668");
        EcList.Add("1669");
        EcList.Add("1670");
        EcList.Add("1673");
        EcList.Add("1674");
        EcList.Add("1675");
        EcList.Add("1677");
        EcList.Add("1680");
        EcList.Add("1681");
        EcList.Add("1682");
        EcList.Add("1683");
        EcList.Add("2001");
        EcList.Add("2002");
        EcList.Add("2003");
        EcList.Add("2005");
        EcList.Add("2006");
        EcList.Add("2007");
        EcList.Add("2008");
        EcList.Add("2009");
        EcList.Add("2010");
        EcList.Add("2014");
        EcList.Add("2015");
        EcList.Add("2016");
        EcList.Add("2017");
        EcList.Add("2020");
        EcList.Add("2021");
        EcList.Add("2022");
        EcList.Add("2023");
        EcList.Add("2024");
        EcList.Add("2025");
        EcList.Add("2026");
        EcList.Add("2027");
        EcList.Add("2028");
        EcList.Add("2100");
        EcList.Add("2101");
        EcList.Add("2102");
        EcList.Add("2103");
        EcList.Add("2104");
        EcList.Add("2105");
        EcList.Add("2106");
        EcList.Add("2107");
        EcList.Add("2108");
        EcList.Add("2109");
        EcList.Add("2110");
        EcList.Add("2111");
        EcList.Add("2112");
        EcList.Add("2113");
        EcList.Add("2114");
        EcList.Add("2115");
        EcList.Add("2116");
        EcList.Add("2117");
        EcList.Add("2118");
        EcList.Add("2119");
        EcList.Add("2120");
        EcList.Add("2121");
        EcList.Add("2122");
        EcList.Add("2123");
        EcList.Add("2124");
        EcList.Add("2125");
        EcList.Add("2126");
        EcList.Add("2127");
        EcList.Add("2128");
        EcList.Add("2129");
        EcList.Add("2130");
        EcList.Add("2131");
        EcList.Add("2132");
        EcList.Add("2133");
        EcList.Add("2200");
        EcList.Add("2201");
        EcList.Add("2202");
        EcList.Add("2203");
        EcList.Add("2204");
        EcList.Add("2206");
        EcList.Add("2207");
        EcList.Add("2208");
        EcList.Add("2209");
        EcList.Add("2210");
        EcList.Add("2211");
        EcList.Add("2212");
        EcList.Add("2213");
        EcList.Add("2213");
        EcList.Add("2214");
        EcList.Add("2502");
        EcList.Add("2503");
        EcList.Add("2504");
    }

    public void Dispose()
    {
        if (null != _cncAutoListReader)
        {
            _cncAutoListReader.OnBatchReadEnd -= AutoListReaderOnBatchReadEnd;

            _cncAutoListReader.Stop();
        }
    }

    public bool IsConnected
    {
        get => _isConnected;
        private set => _isConnected = value;
    }
    public List<string> EcList { get; set; } = [];
    public DrillCommonDataA _drillCommonDataA { get; set; }
    public DrillStatusData _drillStatusData { get; set; }

    public async Task Connect(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug($"连接CNC : {_eAPClientOptions.CNC84Server} : {_eAPClientOptions.CNC84Port}");

            if (!_socketCNC84.IsConnected)
            {
                _isConnected = false;
                try
                {
                    var ip = IPAddress.Parse(_eAPClientOptions.CNC84Server);
                    var point = new IPEndPoint(ip, _eAPClientOptions.CNC84Port);
                    _socketCNC84.Connect(point);
                    Thread.Sleep(2000);

                    if (_socketCNC84.IsConnected)
                    {
                        _logger.LogDebug("连接CNC成功！ IP : " + ip + ":" + point);
                        _isConnected = true;

                        OnCNCConnected?.Invoke();
                    }
                    else
                    {
                        _logger.LogError("连接CNC失败！ IP : " + ip + ":" + point);
                        _isConnected = false;

                        OnCNCConnectFailed?.Invoke();
                        Thread.Sleep(1000);
                        await Connect(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    _isConnected = false;

                    OnCNCConnectException?.Invoke(ex);
                }
            }
        }
        catch (SocketException ex)
        {
            _logger.LogError(ex, "CNC84未开启\r\n" + ex.Message);
            _isConnected = false;

            OnCNCConnectException?.Invoke(ex);
            //Application.DoEvents();
            //Connect();
        }
    }

    public Task Disonnect(CancellationToken cancellationToken)
    {
        try
        {
            if (_socketCNC84 != null)
            {
                _socketCNC84.Close();
                _logger.LogInformation("CNC84已关闭");
                _isConnected = false;

                OnCNCDisconnected?.Invoke();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "断开CNC84服务器失败" + ex.Message);

            OnCNCDisonnectException?.Invoke(ex);
        }

        return Task.CompletedTask;
    }

    private void socketCNC84_Closed(object? sender, EventArgs e)
    {
        try
        {
            _logger.LogInformation("CNC84 Closed");

            OnCNCClosed?.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void socketCNC84_Error(object? sender, SuperSocket.ClientEngine.ErrorEventArgs e)
    {
        try
        {
            _logger.LogInformation("CNC84 连接出错");

            OnCNCError?.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void socketCNC84_Connected(object? sender, EventArgs e)
    {
        try
        {
            _logger.LogInformation("CNC84 已连接");
            _isConnected = true;

            OnCNCConnected?.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public async Task<string> RetrieveData(string key, CancellationToken cancellationToken = default)
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        tokenSource.CancelAfter(TimeSpan.FromSeconds(2));
        cancellationToken = tokenSource.Token;

        try
        {
            if (key.Contains("_"))
            {
                string[] strInfo = key.Split("_");
                string strValue = strInfo[1];
                if (strInfo.Length > 2)
                {
                    strValue = key.Substring(key.IndexOf("_") + 1);
                }

                switch (strInfo[0])
                {
                    case "ToolD":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParD({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolS":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParS({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolF":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParF({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolR":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParR({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolN":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParN({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolB":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParB({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolZ":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParZ({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolA":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParA({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolSegM":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParPKCntVisCheck({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolChipl":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"Chipload({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "LoadPgmFile":
                        {
                            _logger.LogDebug($"Load file : pgm. by retrieve data ({strValue}). ");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                CncLoadFile(LoadFileType.PROGRAM, strValue),
                                cancellationToken);

                            string return_res = result.ToStr() ?? string.Empty;
                            _logger.LogDebug($"Load file : pgm. result '{return_res}'.");
                            return return_res;
                        }
                    case "LoadDiaFile":
                        {
                            _logger.LogDebug($"Load file : dia. by retrieve data ({strValue}). ");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                CncLoadFile(LoadFileType.DIA, strValue),
                                cancellationToken);

                            string return_res = result.ToStr() ?? string.Empty;
                            _logger.LogDebug($"Load file : dia. result '{return_res}'.");
                            return return_res;
                        }
                    case "LoadAtpFile":
                        {
                            _logger.LogDebug($"Load file : atp. by retrieve data ({strValue}). ");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                CncLoadFile(LoadFileType.ATP, strValue),
                                cancellationToken);

                            string return_res = result.ToStr() ?? string.Empty;
                            _logger.LogDebug($"Load file : atp. result '{return_res}'.");
                            return return_res;
                        }
                    case "SpindleEnable":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"ZS_ZSEL({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "TMeasureDia":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TMesD({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "TMeasureLen":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TMesL({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "TRunout":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TMesR({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "Spindle1WorkTimes":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"PC_SpinHisTime({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "Cnc9XWriteNodeData":
                        {
                            return string.Empty;
                        }
                    case "ToolEnalbe":
                        {
                            string medata = $@"JOBS_TOOLOFFER({GetToolIndexById(strInfo[1])})";
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, medata, UseXmldoc: true);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "HitEnalbe":
                        {
                            string medata = $@"JOBS_HITOFFER({GetToolIndexById(strInfo[1])})";
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, medata, UseXmldoc: true);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolReqNum":
                        {
                            string medata = $@"JOBS_TOOLREQUEST({GetToolIndexById(strInfo[1])})";
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, medata, UseXmldoc: true);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "SaveAtpFile":
                        {
                            _logger.LogDebug($"Save file : atp. by retrieve data ({strValue}). ");
                            string sendMsg = GetCommandSaveFileStr(strValue);
                            string sendkey = $"CNCCOMMAND=@SF,{strValue}";
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(sendkey,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                }
            }
            else
            {
                switch (key)
                {
                    case "CncStatus":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.CNCSTATUS);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "BlockText":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.SCREENSAVER);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "CncError":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.CNCERROR);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "CncComm":
                        {
                            return _oldCncComm;
                        }
                    case "CncRunProgress":
                        {
                            return int.Parse(CncStatus.AP).ToString() ?? string.Empty;
                        }
                    case "PgmRunStartTime":
                        {
                            if (_eAPClientOptions.CNC84GetJobInfoByAutoList)
                            {
                                return DateTimeUtil.DateTimeToDBString(_startTimeOfCurJob);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(CncStatus.AR) && CncStatus.AR != "00:00:00")
                                {
                                    DateTime dtAR = DateTime.ParseExact(CncStatus.AR, "HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                                    TimeSpan ts = dtAR - DateTime.ParseExact("00:00:00", "HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

                                    if (ts.TotalSeconds > 0 && ts.TotalSeconds <= 4 && _isNewRun == false)
                                    {
                                        _isNewRun = true;
                                        _oldPgmRunStartTime = DateTime.Now.AddSeconds(-ts.TotalSeconds).ToString("yyyy/MM/dd HH:mm:ss");
                                        return _oldPgmRunStartTime;
                                    }
                                }
                                return _oldPgmRunStartTime;
                            }
                        }
                    case "PgmRunEndTime":
                        {
                            if (_eAPClientOptions.CNC84GetJobInfoByAutoList)
                            {
                                return DateTimeUtil.DateTimeToDBString(_endTimeOfCurJob);
                            }
                            else
                            {
                                if (_isNewRun)
                                {
                                    if (_oldCncStatusEc != CncStatus.EC && CncStatus.EC.Equals("0048"))
                                    {
                                        _isNewRun = false;
                                        _oldCncStatusEc = CncStatus.EC;
                                        _oldPgmRunEndTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                        return _oldPgmRunEndTime;
                                    }
                                    _oldCncStatusEc = CncStatus.EC;
                                }
                                else
                                {
                                    return _oldPgmRunEndTime;
                                }
                                return string.Empty;
                            }
                        }
                    case "CncBlockColor":
                        {
                            return _cncblockColor;
                        }
                    case "PgmFilePath":
                        {
                            return await GetCurPgmFilePathAsync(key, cancellationToken);
                        }
                    case "DiaFilePath":
                        {
                            return await GetCurDiaFilePathAsync(key, cancellationToken);
                        }
                    case "AtpFilePath":
                        {
                            return await GetCurAtpFilePathAsync(key, cancellationToken);
                        }
                    case "CurDrillOrRout":
                    case "TotalDrillOrRout":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMESTRING, "%S(HFCNTValue)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "Duty":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.DUTY);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ShiftOnlineTime":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeTimes(1262)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ShiftWorkingTime":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeTimes(1363)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ShiftWaitingTime":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeTimes(1464)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ShiftErrorTime":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeTimes(1565)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "XYPosition":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.XYPOSITION);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "CurSpindleStatus":
                        {
                            return CncStatus.ZS ?? string.Empty;
                        }
                    case "CurToolId":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.CNCTOOLS);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "DrillH":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComH");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "DrillQ":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComQUIK");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "DrillK":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComK");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "DrillKi":
                        {
                            string medata = "ComKi";
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, medata, UseXmldoc: true);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "DrillZ":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComZ");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "Block":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "HSYS55_Block");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "Step":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "HSYS55_Step");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "DrillFV":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComFV");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "CncVersion":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.VERSION);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ShiftTotalDrillOrRout":
                        {
                            return string.Empty;
                        }
                    case "ShiftRunCount":
                    case "ShiftToolChangeTime":
                        {
                            return string.Empty;
                        }
                    case "SpindleCount":
                        {
                            return _eAPClientOptions.SpindleCount.ToString();
                        }
                    case "PreDuty":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeData(1868)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "OPID":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.OPID);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "SAX":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ViewComSAX");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "SAY":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ViewComSAY");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "SAZX":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComSAZX");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "SAZY":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComSAZY");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "UserName":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.USERNAME);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "UserLevel":
                        {
                            string sendMsg = GetRequestStr(CNCRequestPacketKind.USERLEVEL);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "CncShowText":
                    case "RunDrillHits":
                    case "ShiftsStartTime":
                        {
                            return string.Empty;
                        }
                    case "IsCncRunning":
                        {
                            return _socketCNC84.IsConnected.ToStr() ?? string.Empty;
                        }
                    case "DiameterTolChecked":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComTCDVisCheck");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "DiameterTolNegValue":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "TMesN(0)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "DiameterTolPosValue":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "TMesP(0)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "LengthTolChecked":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComTCL");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "LengthTolNegValue":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "TMesN(1)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "LengthTolPosValue":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "TMesP(1)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "RunoutTolChecked":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "RunoutComTCR");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "RunoutTolNegValue":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "TMesN(2)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "RunoutTolPosValue":
                        {
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "TMesP(2)");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "AutoListRunNumber":
                        {
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                GetAutoListRunNumber(key, cancellationToken),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case CncDataNameUnity.RecentAutoListRecords:
                        {
                            var recList = new List<AutoListRecord>();
                            int curIdx = _cncAutoListReader.GetCurJobIndex();

                            for (int idx = curIdx; idx >= 0; idx--)
                            {
                                var line = _cncAutoListReader.GetLineCopy(idx);
                                if (null != line)
                                {
                                    var rec = new AutoListRecord()
                                    {
                                        StartRunTime = line.StartTime,
                                        EndRunTime = line.EndTime,

                                        HitCount = line.HoleCnt,
                                        RouMeters = 0,

                                        WorkDurMin = line.WorkDurMin,
                                        StopDurMin = line.StopDurMin,
                                    };
                                    recList.Add(rec);
                                }
                            }//for

                            string res = JsonSerializer.Serialize(recList);
                            return res;
                        }
                    case "CncStart":
                        {
                            string medata = "START";
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.CNCKEY, medata, UseXmldoc: true);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "CncStop":
                        {
                            string medata = "STOP";
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.CNCKEY, medata, UseXmldoc: true);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case CncDataNameUnity.CM:
                        {
                            var res = await ExecuteCM();
                            return res.ToStr();
                        }

                    case CncDataNameUnity.XOfBasePoint:
                        {
                            string medata = CNC8XCmd.FIXXY_X;
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMESTRING, CNC8XCmd.FIXXY_X);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case CncDataNameUnity.YOfBasePoint:
                        {
                            string medata = CNC8XCmd.FIXXY_Y;
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMESTRING, CNC8XCmd.FIXXY_Y);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }

                    case "SpindleYaw":
                        {
                            string medata = "TTolRW";
                            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, medata, UseXmldoc: true);
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(medata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return string.Empty;
    }

    #region CreateCNCMessageStr

    private string GetRequestStr(CNCRequestPacketKind message, string Text = "")
    {
        try
        {
            CNCPacket[] lmesinfo = new CNCPacket[4];
            lmesinfo[0].Name = "SMDNCPACKET";
            lmesinfo[0].Value = DateTime.Now.ToString("ssfffffff");
            lmesinfo[1].Name = "CNC";
            lmesinfo[1].Value = "1";
            lmesinfo[2].Name = "REQUEST";
            lmesinfo[2].Value = DateTime.Now.ToString("mmssfffffff");
            lmesinfo[3].Name = message.ToString().Replace("At", "@");
            //lmesinfo[3].Value = "ADVISEOFF";
            if (Text != "")
                lmesinfo[3].Text = Text;
            return GetTcpIpMessage(lmesinfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetExecuteStr(CNCExecutePacketKind message, string messageData, bool UseXmldoc = false)
    {
        try
        {
            CNCPacket[] lmesinfo = new CNCPacket[4];
            lmesinfo[0].Name = "SMDNCPACKET";
            lmesinfo[0].Value = DateTime.Now.ToString("ssfffffff");
            lmesinfo[1].Name = "CNC";
            lmesinfo[1].Value = "1";
            lmesinfo[2].Name = "EXECUTE";
            lmesinfo[2].Value = DateTime.Now.ToString("mmssfffffff");
            lmesinfo[3].Name = message.ToString();
            lmesinfo[3].Text = messageData;
            if (UseXmldoc)
            {
                return GetTcpIpMessageXML(lmesinfo);
            }
            else
            {
                return GetTcpIpMessage(lmesinfo);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetTcpIpMessage(CNCPacket[] message)
    {
        try
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < message.Count(); i++)
            {
                str.Append("<");
                str.Append(message[i].Name);

                if (message[i].Value != "" && message[i].Value != null)//if (!("COMMAND".Equals(message[i].Name) || "RUNTIMEVALUE".Equals(message[i].Name)))
                {
                    str.Append(" Value=" + message[i].Value);
                }
                str.Append(">");

                if (message[i].Text != "")
                {
                    str.Append(message[i].Text);
                }
            }

            for (int i = 0; i < message.Count(); i++)
            {
                str.Append("</");
                str.Append(message[message.Count() - i - 1].Name);
                str.Append(">");
            }
            //_logger.LogDebug(str.ToString(0, str.Length));
            //ShowResult(str.ToString(0, str.Length), "GetTcpIpMessage");
            return str.ToString(0, str.Length);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetTcpIpMessageXML(CNCPacket[] message)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode node0 = xmlDoc.CreateElement(message[0].Name);
        if (!string.IsNullOrEmpty(message[0].Value))
        {
            XmlAttribute att = xmlDoc.CreateAttribute("Value");
            att.Value = message[0].Value;
            node0.Attributes.Append(att);
        }
        if (!string.IsNullOrEmpty(message[0].Text))
        {
            node0.InnerText = message[0].Text;
        }
        xmlDoc.AppendChild(node0);

        XmlNode node1 = xmlDoc.CreateElement(message[1].Name);
        if (!string.IsNullOrEmpty(message[1].Value))
        {
            XmlAttribute att = xmlDoc.CreateAttribute("Value");
            att.Value = message[1].Value;
            node1.Attributes.Append(att);
        }
        if (!string.IsNullOrEmpty(message[1].Text))
        {
            node1.InnerText = message[1].Text;
        }
        node0.AppendChild(node1);

        XmlNode node2 = xmlDoc.CreateElement(message[2].Name);
        if (!string.IsNullOrEmpty(message[2].Value))
        {
            XmlAttribute att = xmlDoc.CreateAttribute("Value");
            att.Value = message[2].Value;
            node2.Attributes.Append(att);
        }
        if (!string.IsNullOrEmpty(message[2].Text))
        {
            node2.InnerText = message[2].Text;
        }
        node1.AppendChild(node2);

        XmlNode node3 = xmlDoc.CreateElement(message[3].Name);
        if (!string.IsNullOrEmpty(message[3].Value))
        {
            XmlAttribute att = xmlDoc.CreateAttribute("Value");
            att.Value = message[3].Value;
            node3.Attributes.Append(att);
        }
        if (!string.IsNullOrEmpty(message[3].Text))
        {
            node3.InnerText = message[3].Text;
        }
        node2.AppendChild(node3);

        return xmlDoc.InnerXml;
    }

    private string GetCommandLoadFileStr(string filePath)
    {
        try
        {
            CNCPacket[] lmesinfo = new CNCPacket[4];
            lmesinfo[0].Name = "SMDNCPACKET";
            lmesinfo[0].Value = DateTime.Now.ToString("ssfffffff");
            lmesinfo[1].Name = "CNC";
            lmesinfo[1].Value = "1";
            lmesinfo[2].Name = "EXECUTE";
            lmesinfo[2].Value = DateTime.Now.ToString("ssfffffff");
            lmesinfo[3].Name = "COMMAND";
            lmesinfo[3].Text = $"CNCCOMMAND=@LF,{filePath}";// $"CNCCOMMAND=@LF,{ATPFile}";
            return GetTcpIpMessage(lmesinfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCommandSaveFileStr(string filePath)
    {
        try
        {
            CNCPacket[] lmesinfo = new CNCPacket[4];
            lmesinfo[0].Name = "SMDNCPACKET";
            lmesinfo[0].Value = DateTime.Now.ToString("ssfffffff");
            lmesinfo[1].Name = "CNC";
            lmesinfo[1].Value = "1";
            lmesinfo[2].Name = "EXECUTE";
            lmesinfo[2].Value = DateTime.Now.ToString("ssfffffff");
            lmesinfo[3].Name = "COMMAND";
            lmesinfo[3].Text = $"CNCCOMMAND=@SF,{filePath}";// $"CNCCOMMAND=@LF,{ATPFile}";
            return GetTcpIpMessageXML(lmesinfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private string GetCncCommandStr(string operation)
    {
        try
        {
            CNCPacket[] lmesinfo = new CNCPacket[4];
            lmesinfo[0].Name = "SMDNCPACKET";
            lmesinfo[0].Value = DateTime.Now.ToString("ssfffffff");
            lmesinfo[1].Name = "CNC";
            lmesinfo[1].Value = "1";
            lmesinfo[2].Name = "EXECUTE";
            lmesinfo[2].Value = DateTime.Now.ToString("ssfffffff");
            lmesinfo[3].Name = "CNCCOMMAND";
            lmesinfo[3].Text = operation;
            return GetTcpIpMessage(lmesinfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    #endregion CreateCNCMessageStr

    #region ParseCNCMessage

    public BrokenToolData ParseBrokenToolData(BrokenToolData brokenToolData, string blockText, CancellationToken cancellationToken = default)
    {
        try
        {
            brokenToolData.BrokenInfo = blockText;
            int brokentoolIndex = blockText.IndexOf("断刀");
            if (brokentoolIndex < 0)
            {
                brokentoolIndex = blockText.IndexOf("Broken Tool", StringComparison.OrdinalIgnoreCase);
            }

            string strPrefix = blockText.Substring(brokentoolIndex);
            int nNumIndex = strPrefix.IndexOf("Z");
            strPrefix = strPrefix.Substring(0, nNumIndex).Trim();

            Regex regexNum = new Regex(@"\d");
            int nTIndex = regexNum.Match(strPrefix).Index;
            string strBrkToolNum = strPrefix.Substring(nTIndex);
            brokenToolData.BrkToolId = strBrkToolNum;

            string strSpindle = blockText.Substring(blockText.IndexOf("Z"));
            string strBrokenSpindle = "";

            if (!String.IsNullOrEmpty(strSpindle))
            {
                while (strSpindle.IndexOf("Z") >= 0)
                {
                    strSpindle = strSpindle.Substring(strSpindle.IndexOf("Z"));
                    strBrokenSpindle += strSpindle.Substring(1, 1);
                    strSpindle = strSpindle.Substring(1);
                }
            }
            brokenToolData.BrkToolSpindle = strBrokenSpindle;

            string strDia = blockText.Substring(blockText.IndexOf("D") + 1);
            strDia = strDia.Substring(0, 5);
            float fDia;

            if (float.TryParse(strDia, out fDia))
            {
                brokenToolData.BrkToolDia = fDia.ToString("F3");
            }

            _logger.LogInformation($"ParseBrokenTool - T{brokenToolData.BrkToolId} D{brokenToolData.BrkToolDia} Z{brokenToolData.BrkToolSpindle}");
            return brokenToolData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ParseBrokenToolData 异常" + ex.Message);
        }
        return new BrokenToolData();
    }

    private string GetRunTimeStringValue(string str, string keyText)
    {
        string rtnStr = string.Empty;
        try
        {
            //得到结果：1:D:\3225\3225.DIA <SMDNCPACKET Value=249772240><CNC Value=1><EXECUTE Value=52044><RUNTIMESTRING Value=1:D:\3225\3225.DIA>%S(DiaFileNameWithDialog,0)</RUNTIMESTRING></EXECUTE></CNC></SMDNCPACKET>
            string tmpstr = keyText + "</RUNTIMESTRING>";
            int pos;
            pos = str.IndexOf(tmpstr);

            if (pos != -1)
            {
                str = str.Substring(0, pos - 1);
                tmpstr = "<RUNTIMESTRING Value=";
                pos = str.IndexOf(tmpstr);

                if (pos != -1)
                {
                    str = str.Remove(0, pos + tmpstr.Length);
                    rtnStr = str;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return rtnStr;
    }

    private string GetText(string str, string message)
    {
        string rtnStr = string.Empty;
        try
        {
            //recv: <SMDNCPACKET Value="1193350104"><CNC Value="1"><REQUEST Value="4"><DUTY Value="ADVISEOFF">2</DUTY></REQUEST></CNC></SMDNCPACKET>
            string tmpstr = "<" + message + " Value=ADVISEOFF>";
            int pos;
            pos = str.IndexOf(tmpstr);

            if (pos != -1)
            {
                str = str.Substring(pos + tmpstr.Length);
                tmpstr = "</" + message + ">";
                pos = str.IndexOf(tmpstr);

                if (pos != -1)
                {
                    str = str.Remove(pos);
                    str.Replace("'", "\'");
                    str.Replace("\\", "\\\\");
                    str.Replace("\"", "\\\"");
                    rtnStr = str;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return rtnStr;
    }

    private string GetRuntimeValueValue(string str, string message)
    {
        string rtnStr = string.Empty;
        try
        {
            //recv: <SMDNCPACKET Value="1193350195"><CNC Value="1"><EXECUTE Value="4"><RUNTIMEVALUE Value="0">ComAOFFX</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>
            string tmpstr = message + "</RUNTIMEVALUE>";
            int pos;
            pos = str.IndexOf(tmpstr);

            if (pos != -1)
            {
                str = str.Substring(0, pos - 1);
                tmpstr = "<RUNTIMEVALUE Value=";
                pos = str.IndexOf(tmpstr);

                if (pos != -1)
                {
                    str = str.Remove(0, pos + tmpstr.Length);
                    str.Replace("'", "\'");
                    str.Replace("\\", "\\\\");
                    str.Replace("\"", "\\\"");
                    rtnStr = str;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return rtnStr;
    }

    private static long sGetValueWithIndexCount = 0;

    public string GetValueWithIndex(string str, string req_data, out int index)
    {//添加了包含后缀的情况
     //带后缀的EG :
     //<SMDNCPACKET Value="159635668"><CNC Value="1"><EXECUTE Value="22222"><RUNTIMEVALUE Value="-1">JOB_STARTTIME(4)</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>
     //无后缀的例子：
     //<SMDNCPACKET Value="159635228"><CNC Value="1"><EXECUTE Value="22222"><RUNTIMEVALUE Value="4">HSYS55_JobIndex</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>
        string recv_packet = str;
        index = -1;

        int message_head_idx = str.IndexOf(req_data);
        if (message_head_idx < 0)
            return "";

        int message_tail_idx = message_head_idx + req_data.Length;
        if (message_tail_idx + 3 >= str.Length)
            return "";

        string tmpstr = req_data + "</RUNTIMEVALUE>";
        string rt_part = req_data + "</RUNTIMEVALUE>";

        bool has_index_in_req = false;
        //考虑带后缀的情况
        if (str[message_tail_idx] == '(')
        {
            int left_bi = message_tail_idx;
            int right_bi = str.IndexOf(')', message_tail_idx + 1);
            if (right_bi > message_tail_idx + 1)
            {
                string index_str = str.Substring(message_tail_idx + 1, right_bi - (message_tail_idx + 1));
                index = (int)StringUtil.TryParseLong(index_str, -1);
                if (index >= 0)
                {
                    int next_lb_idx = str.IndexOf('<', message_tail_idx + 1);
                    if (next_lb_idx > 0)
                    {
                        tmpstr = str.Substring(message_head_idx, next_lb_idx - message_head_idx);

                        has_index_in_req = true;
                        //if (sGetValueWithIndexCount % 100 == 0) _logger.LogDebug($"Get value with index : msg '{tmpstr}'.");
                    }
                }
            }
        }
        sGetValueWithIndexCount += 1;

        //Console.WriteLine(str);
        string rtnStr = "";
        int pos;
        pos = str.IndexOf(tmpstr);
        if (pos != -1)
        {
            str = str.Substring(0, pos - 1);
            tmpstr = "<RUNTIMEVALUE Value=";
            pos = str.IndexOf(tmpstr);
            if (pos != -1)
            {
                str = str.Remove(0, pos + tmpstr.Length);
                str.Replace("'", "\'");
                str.Replace("\\", "\\\\");
                str.Replace("\"", "\\\"");
                rtnStr = str;
            }
        }

        //if (has_index_in_req) _logger.LogDebug($"8xAutoList : Get value : Index {index}. '{rt_part}'. recv '{recv_packet}'");

        return rtnStr;
    }

    private string GetExecuteRuntimeValue(string str, string RTValueData, string Item = "RUNTIMEVALUE")
    {
        try
        {
            string strRuntimeValuePre = "<" + Item + " Value=";
            int posRuntimeValuePre = str.IndexOf(strRuntimeValuePre);

            if (posRuntimeValuePre > -1)
            {
                int posValueEnd = str.IndexOf(">", posRuntimeValuePre);
                int nLen = posValueEnd - (posRuntimeValuePre + strRuntimeValuePre.Length);
                string strValue = str.Substring(posRuntimeValuePre + strRuntimeValuePre.Length, nLen);
                return strValue;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetExecuteRuntimeValue函数异常 - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetExecuteRuntimeValueXML(string str, string Item = "RUNTIMEVALUE")
    {
        string value = "";
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode? node = xmlDoc.SelectSingleNode($@"//{Item}");
            if (node != null)
            {
                value = node?.Attributes?["Value"]?.Value ?? "";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetExecuteRuntimeValue函数异常 - " + ex.Message);
        }
        return value;
    }

    private string GetExecuteRuntimeTextXML(string str, string Item = "RUNTIMEVALUE")
    {
        string text = "";
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);

            XmlNode? node = xmlDoc.SelectSingleNode($@"//{Item}");
            if (node != null)
            {
                text = node.InnerText;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetExecuteRuntimeValue函数异常 - " + ex.Message);
        }
        return text;
    }

    private string ParseToolIndexData(string strRecvMsg)
    {
        try
        {
            int nStartIndex = strRecvMsg.IndexOf("(") + 1;
            int nEndIndex = strRecvMsg.IndexOf(")");
            return strRecvMsg.Substring(nStartIndex, nEndIndex - nStartIndex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ParseToolIndexData 函数异常 - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolIdByIndex(string strToolIndex)
    {
        try
        {
            int nToolId = 0;

            if (int.TryParse(strToolIndex, out nToolId))
            {
                nToolId += 1;
                return nToolId.ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolIdByIndex 函数异常 - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetToolIndexById(string strToolId)
    {
        try
        {
            int nToolIndex = 0;

            if (int.TryParse(strToolId, out nToolIndex))
            {
                if (nToolIndex > 0)
                {
                    nToolIndex -= 1;
                    return nToolIndex.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetToolIndexById 函数异常 - " + ex.Message);
        }
        return string.Empty;
    }

    private string GetFormatInt2FloatString(string strOrg, int nDiv, string strFormat = "F3")
    {
        try
        {
            float fOrg = 0;

            if (float.TryParse(strOrg, out fOrg))
            {
                fOrg /= nDiv;
                return fOrg.ToString(strFormat);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetFormatInt2FloatString 函数异常 - " + ex.Message);
        }
        return string.Empty;
    }

    #endregion ParseCNCMessage

    #region SendMsg2CNC

    //某些情况下，  在发送消息后SEELP 一会， 才能收到回复
    private async Task<bool> SendMessageToCNC84(string content, int sleepMS = 0)
    {
        try
        {
            if (0 == Interlocked.Exchange(ref _refreshRouteCodes, 1))
            {
                await channelSendCNC.Writer.WriteAsync(content);
                Interlocked.Exchange(ref _refreshRouteCodes, 0);

                Thread.Sleep(sleepMS);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendMessageToCNC84 失败" + ex.Message);
            return false;
        }

        return true;
    }

    private void PrintCncStatusBrief()
    {
        string autoListPart = "";
        if (_eAPClientOptions.CNC84GetJobInfoByAutoList)
        {
            autoListPart = $"Auto-list : cur job idx {_indexOfCurJob}. t {_startTimeOfCurJob} ~ {_endTimeOfCurJob}.";
        }

        _logger.LogDebug($"Cnc status brief : '{_cncStatusData}'. " + autoListPart);
    }

    private async Task SendMsg2CNCLoop()
    {
        DateTime dtStart = DateTime.Now;
        DateTime dtEnd = DateTime.Now;
        TimeSpan timeSpan = dtStart - dtEnd;
        try
        {
            while (await channelSendCNC.Reader.WaitToReadAsync())
            {
                if (channelSendCNC.Reader.TryRead(out var sendMsg))
                {
                    dtStart = DateTime.Now;
                    timeSpan = dtStart - dtEnd;

                    while (timeSpan.TotalMilliseconds <= 300)
                    {
                        await Task.Delay(50);
                        dtStart = DateTime.Now;
                        timeSpan = dtStart - dtEnd;
                    }

                    Send2CNC84Socket(sendMsg);
                    dtEnd = DateTime.Now;
                }

                if ((DateTime.Now - _lastPrintCncStatusBriefTime).TotalSeconds > 30)
                {
                    _lastPrintCncStatusBriefTime = DateTime.Now;

                    PrintCncStatusBrief();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendMsg2CNCLoop 异常 - " + ex.Message);
        }
    }

    private bool Send2CNC84Socket(string content)
    {
        bool flag = false;
        try
        {
            if (!_socketCNC84.IsConnected)
            {
                _isConnected = false;
                _logger.LogError("丢失CNC84连接\r\n");
                return false;
            }

            lock (_lockSendCNC84Socket)
            {
                //byte[] bytes = Encoding.UTF8.GetBytes(content);
                byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(content);
                flag = _socketCNC84.TrySend(bytes);

                if (!flag)
                {
                    _logger.LogError("Send2CNC84Socket - 发送给CNC84失败。");
                }
                else
                {
                    OnCNCDataSent?.Invoke();

                    if (_eAPClientOptions.IsDebug)
                    {
                        _logger.LogDebug("发送CNC84数据\r\n" + content);
                    }
                }
            }

            return flag;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Send2CNC84Socket Exception - " + ex.Message);
            OnCNCDataSentException?.Invoke(ex);
            return flag;
        }
    }

    #endregion SendMsg2CNC

    private async Task ProcessChannelDDEData()
    {
        while (await channelDDE.Reader.WaitToReadAsync())
        {
            if (channelDDE.Reader.TryRead(out var recvMsg))
            {
                if (_eAPClientOptions.IsDebug)
                {
                    _logger.LogDebug("接收CNC84数据\r\n" + recvMsg);
                }

                await processCNC84DataReceived(recvMsg);
            }
        }
    }

    private Task processCNC84DataReceived(string recvMsg)
    {
        try
        {
            if (recvMsg.Contains(CNCRequestPacketKind.CNCSTATUS.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.CNCSTATUS.ToString());
                _cncStatusData = strParseData;

                _asyncTaskWaiter.TrySetResult("CncStatus", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.SCREENSAVER.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.SCREENSAVER.ToString());
                _cncblockColor = strParseData.Substring(strParseData.Trim().IndexOf(" ")).Trim();

                _asyncTaskWaiter.TrySetResult("BlockText", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.CNCERROR.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.CNCERROR.ToString());

                if (strParseData != string.Empty && strParseData.EndsWith(";3398"))
                {
                    _oldCncComm = strParseData;
                }

                _asyncTaskWaiter.TrySetResult("CncError", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.ACTPROGRAM.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.ACTPROGRAM.ToString());

                _asyncTaskWaiter.TrySetResult("PgmFilePath", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("%S(DiaFileNameWithDialog,0)"))
            {
                string strParseData = GetRunTimeStringValue(recvMsg, "%S(DiaFileNameWithDialog,0)").Replace("1:", "");

                _asyncTaskWaiter.TrySetResult("DiaFilePath", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("%S(ATPFileNameWithDialog,0)"))
            {
                string strParseData = GetRunTimeStringValue(recvMsg, "%S(ATPFileNameWithDialog,0)").Replace("1:", "");

                _asyncTaskWaiter.TrySetResult("AtpFilePath", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("%S(HFCNTValue)"))
            {
                string strParseData = GetRunTimeStringValue(recvMsg, "%S(HFCNTValue)").Replace("1:", "");
                string strCur = "0";
                string strTotal = "0";
                if (strParseData.Contains(@"_\_"))
                {
                    string[] pds = strParseData.Split([@"_\_"], StringSplitOptions.None);
                    strCur = pds[0];
                    strTotal = pds[1];
                }
                else
                {
                    //CNC82 钻机 只输出当前加工孔数
                    strCur = strParseData;
                }
                if (strParseData.Length > 2 && strParseData.Contains("_"))
                {
                    strCur = strParseData.Substring(0, strParseData.IndexOf("_"));
                    strTotal = strParseData.Substring(strParseData.IndexOf("_\\_") + 3);
                }
                _asyncTaskWaiter.TrySetResult("CurDrillOrRout", strCur.GetBytes());
                _asyncTaskWaiter.TrySetResult("TotalDrillOrRout", strTotal.GetBytes());
            }
            else if (recvMsg.Contains("TParD"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParD");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolD_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TParS"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParS");
                strParseData = GetFormatInt2FloatString(strParseData, 10, "F1");
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolS_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TParF"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParF");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolF_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TParR"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParR");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolR_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TParN"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParN");
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolN_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TParB"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParB");
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolB_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TParZ"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParZ");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolZ_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TParA"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParA");
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolA_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("Chipload"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"Chipload");
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolChipl_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TParPKCntVisCheck"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TParPKCntVisCheck");
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("ToolSegM_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.DUTY.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.DUTY.ToString());

                _asyncTaskWaiter.TrySetResult("Duty", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("PC_HisBdeTimes(1262)"))//<SMDNCPACKET Value="119246767"><CNC Value="1"><EXECUTE Value="41"><RUNTIMEVALUE Value="0">ComFAX</RUNTIMEVALUE></EXECUTE></CNC></SMDNCPACKET>
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "PC_HisBdeTimes(1262)");
                strParseData = GetFmtTimeBySeconds(strParseData);

                _asyncTaskWaiter.TrySetResult("ShiftOnlineTime", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("PC_HisBdeTimes(1363)"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "PC_HisBdeTimes(1363)");
                strParseData = GetFmtTimeBySeconds(strParseData);

                _asyncTaskWaiter.TrySetResult("ShiftWorkingTime", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("PC_HisBdeTimes(1464)"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "PC_HisBdeTimes(1464)");
                strParseData = GetFmtTimeBySeconds(strParseData);

                _asyncTaskWaiter.TrySetResult("ShiftWaitingTime", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("PC_HisBdeTimes(1565)"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "PC_HisBdeTimes(1565)");
                strParseData = GetFmtTimeBySeconds(strParseData);

                _asyncTaskWaiter.TrySetResult("ShiftErrorTime", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.XYPOSITION.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.XYPOSITION.ToString());

                _asyncTaskWaiter.TrySetResult("XYPosition", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.CNCTOOLS.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.CNCTOOLS.ToString());
                string[] strToolInfos = strParseData.Split(',');
                int nToolId = 0;

                foreach (string toolInfo in strToolInfos)
                {
                    if (toolInfo.StartsWith("T"))
                    {
                        if (int.TryParse(toolInfo.Replace("T", ""), out nToolId))
                        {
                            break;
                        }
                    }
                }

                _asyncTaskWaiter.TrySetResult("CurToolId", nToolId.ToString().GetBytes());
            }
            else if (recvMsg.Contains("CNCCOMMAND=@LF,"))
            {
                _logger.LogDebug($"Load file : Recv ack msg : '{recvMsg}' ");
                _asyncTaskWaiter.TrySetResult("LoadFile", "LoadFileReturn".GetBytes());
            }
            else if (recvMsg.Contains("CNCCOMMAND=@SF,"))
            {
                _logger.LogDebug($"Save file : Recv ack msg : '{recvMsg}' ");
                string Text = GetExecuteRuntimeTextXML(recvMsg, "COMMAND");
                _asyncTaskWaiter.TrySetResult(Text, "SaveFileReturn".GetBytes());
            }
            else if (recvMsg.Contains("ComFV"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ComFV");

                _asyncTaskWaiter.TrySetResult("DrillFV", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ComH"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ComH");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                _asyncTaskWaiter.TrySetResult("DrillH", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ComQUIK"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ComQUIK");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                _asyncTaskWaiter.TrySetResult("DrillQ", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ComKi"))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg);
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string Text = GetExecuteRuntimeTextXML(recvMsg);
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());

                _asyncTaskWaiter.TrySetResult("DrillK", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ComK"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ComK");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                _asyncTaskWaiter.TrySetResult("DrillK", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ComZ"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ComZ");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                _asyncTaskWaiter.TrySetResult("DrillZ", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("HSYS55_Block"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "HSYS55_Block");

                _asyncTaskWaiter.TrySetResult("Block", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("HSYS55_Step"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "HSYS55_Step");

                _asyncTaskWaiter.TrySetResult("Step", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.VERSION.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.VERSION.ToString());

                _asyncTaskWaiter.TrySetResult("CncVersion", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("PC_HisBdeData(1868)"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "PC_HisBdeData(1868)");

                _asyncTaskWaiter.TrySetResult("PreDuty", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.OPID.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.OPID.ToString());

                _asyncTaskWaiter.TrySetResult("OPID", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ViewComSAX"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ViewComSAX");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                _asyncTaskWaiter.TrySetResult("SAX", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ViewComSAY"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ViewComSAY");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                _asyncTaskWaiter.TrySetResult("SAY", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ComSAZX"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ComSAZX");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                _asyncTaskWaiter.TrySetResult("SAZX", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ComSAZY"))
            {
                string strParseData = GetRuntimeValueValue(recvMsg, "ComSAZY");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                _asyncTaskWaiter.TrySetResult("SAZY", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ZS_ZSEL"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"ZS_ZSEL");
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("SpindleEnable_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TMesD"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TMesD");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("TMeasureDia_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TMesL"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TMesL");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("TMeasureLen_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TMesR"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TMesR");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("TRunout_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("PC_SpinHisTime"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"PC_SpinHisTime");
                strParseData = GetFmtTimeBySeconds(strParseData);
                string strToolId = GetToolIdByIndex(ParseToolIndexData(recvMsg));

                _asyncTaskWaiter.TrySetResult("Spindle1WorkTimes_" + strToolId, strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.USERNAME.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.USERNAME.ToString());

                _asyncTaskWaiter.TrySetResult("UserName", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNCRequestPacketKind.USERLEVEL.ToString()))
            {
                string strParseData = GetText(recvMsg, CNCRequestPacketKind.USERLEVEL.ToString());

                _asyncTaskWaiter.TrySetResult("UserLevel", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("TMesN"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TMesN");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                SetTolNegPosValue(strParseData, "TMesN", ParseToolIndexData(recvMsg));
            }
            else if (recvMsg.Contains("TMesP"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"TMesP");
                strParseData = GetFormatInt2FloatString(strParseData, 1000);

                SetTolNegPosValue(strParseData, "TMesP", ParseToolIndexData(recvMsg));
            }
            else if (recvMsg.Contains("ComTCDVisCheck"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"ComTCDVisCheck");

                _asyncTaskWaiter.TrySetResult("DiameterTolChecked", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("ComTCL"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"ComTCL");

                _asyncTaskWaiter.TrySetResult("LengthTolChecked", strParseData.GetBytes());
            }
            else if (recvMsg.Contains("RunoutComTCR"))
            {
                string strParseData = GetExecuteRuntimeValue(recvMsg, $"RunoutComTCR");

                _asyncTaskWaiter.TrySetResult("RunoutTolChecked", strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNC8XCmd.HSYS55_JobIndex))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg);
                string Text = GetExecuteRuntimeTextXML(recvMsg);
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());
            }
            else if (recvMsg.Contains(CNC8XCmd.JOB_RUN))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg);
                string Text = GetExecuteRuntimeTextXML(recvMsg);
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("START", StringComparison.OrdinalIgnoreCase))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg, Item: "CNCKEY");
                string Text = GetExecuteRuntimeTextXML(recvMsg, Item: "CNCKEY");
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("STOP", StringComparison.OrdinalIgnoreCase))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg, Item: "CNCKEY");
                string Text = GetExecuteRuntimeTextXML(recvMsg, Item: "CNCKEY");
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("JOBS_HITOFFER", StringComparison.OrdinalIgnoreCase))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg);
                string Text = GetExecuteRuntimeTextXML(recvMsg);
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("JOBS_TOOLOFFER", StringComparison.OrdinalIgnoreCase))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg);
                string Text = GetExecuteRuntimeTextXML(recvMsg);
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());
            }
            else if (recvMsg.Contains("JOBS_TOOLREQUEST", StringComparison.OrdinalIgnoreCase))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg);
                string Text = GetExecuteRuntimeTextXML(recvMsg);
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());
            }
            else if (recvMsg.Contains(">FAX") && recvMsg.Contains("</CNCCOMMAND>"))
            {
                _asyncTaskWaiter.TrySetResult(CNC8XCmd.FA, "true".GetBytes());
            }
            else if (recvMsg.Contains(CNC8XCmd.CM_NO_CONFIRM) && recvMsg.Contains("</CNCCOMMAND>"))
            {
                _asyncTaskWaiter.TrySetResult(CNC8XCmd.CM_NO_CONFIRM, "true".GetBytes());
            }
            else if (recvMsg.Contains(CNC8XCmd.FIXXY_X) && recvMsg.Contains("</RUNTIMESTRING>"))
            {
                _logger.LogDebug($"Recv ack msg of {CNC8XCmd.FIXXY_X} : '{recvMsg}'.");
                string text = GetRunTimeStringValue(recvMsg, CNC8XCmd.FIXXY_X);//1:-12.000
                string[] segs = text.Split(':');
                if (segs.Length >= 2)
                    text = segs[1];


                _asyncTaskWaiter.TrySetResult(CNC8XCmd.FIXXY_X, text.GetBytes());
            }
            else if (recvMsg.Contains(CNC8XCmd.FIXXY_Y) && recvMsg.Contains("</RUNTIMESTRING>"))
            {
                _logger.LogDebug($"Recv ack msg of {CNC8XCmd.FIXXY_Y} : '{recvMsg}'.");
                string text = GetRunTimeStringValue(recvMsg, CNC8XCmd.FIXXY_Y);//1:-12.000
                string[] segs = text.Split(':');
                if (segs.Length >= 2)
                    text = segs[1];

                _asyncTaskWaiter.TrySetResult(CNC8XCmd.FIXXY_Y, text.GetBytes());
            }

            else if (recvMsg.Contains("TTolRW", StringComparison.OrdinalIgnoreCase))
            {
                string strParseData = GetExecuteRuntimeValueXML(recvMsg);
                strParseData = GetFormatInt2FloatString(strParseData, 1000);
                string Text = GetExecuteRuntimeTextXML(recvMsg);
                _asyncTaskWaiter.TrySetResult(Text, strParseData.GetBytes());
            }

            #region ForAutoList

            else if (recvMsg.Contains(CNC8XCmd.JOB_STARTTIME))
            {
                HandleAutoList_JobStartTime(recvMsg);
            }
            else if (recvMsg.Contains(CNC8XCmd.JOB_ENDTIME))
            {
                HandleAutoList_JobEndTime(recvMsg);
            }
            else if (recvMsg.Contains(CNC8XCmd.JOB_TIME))
            {
                HandleAutoList_JobTime(recvMsg);
            }
            else if (recvMsg.Contains(CNC8XCmd.JOB_HOLE_CNT))
            {
                HandleAutoList_JobHoleCnt(recvMsg);
            }

            #endregion ForAutoList
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return Task.CompletedTask;
    }

    private void SetTolNegPosValue(string strValue, string strNegOrPos, string strMsgIndex)
    {
        try
        {
            if (strNegOrPos.Contains("TMesN"))
            {
                switch (strMsgIndex)
                {
                    case "0":
                        _asyncTaskWaiter.TrySetResult("DiameterTolNegValue", strValue.GetBytes());
                        break;

                    case "1":
                        _asyncTaskWaiter.TrySetResult("LengthTolNegValue", strValue.GetBytes());
                        break;

                    case "2":
                        _asyncTaskWaiter.TrySetResult("RunoutTolNegValue", strValue.GetBytes());
                        break;
                }
            }
            else
            {
                switch (strMsgIndex)
                {
                    case "0":
                        _asyncTaskWaiter.TrySetResult("DiameterTolPosValue", strValue.GetBytes());
                        break;

                    case "1":
                        _asyncTaskWaiter.TrySetResult("LengthTolPosValue", strValue.GetBytes());
                        break;

                    case "2":
                        _asyncTaskWaiter.TrySetResult("RunoutTolPosValue", strValue.GetBytes());
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    #region ForBuffHold

    private void BuffHoldOutputImp(string line)
    {
        _logger.LogDebug(line);
    }

    #endregion ForBuffHold

    private void socketCNC84_DataReceived(object? sender, DataEventArgs e)
    {
        if (e.Data == null) { return; }
        try
        {
            byte[] buffer = new byte[e.Length];
            Array.Copy(e.Data, 0, buffer, 0, e.Length);

            /* 原来的实现有缺陷（没有考虑 不完整、残留、粘包）
            if (buffer.Length > 0)
            {
                string recvMsg = Encoding.GetEncoding("gb2312").GetString(buffer);
                channelDDE.Writer.WriteAsync(recvMsg);

                OnCNCDataReceived?.Invoke();
            }
            */

            //新的解析实现
            int validRecvCount = _buffHold.PushData(buffer, 0, buffer.Length, _recvBytesLogFilePath);
            _recvBytesOfSocket += validRecvCount;
            while (true)
            {
                string recvMsg = _buffHold.TryPopOnePacket();
                if (string.IsNullOrEmpty(recvMsg))
                {
                    break;
                }
                else
                {
                    channelDDE.Writer.WriteAsync(recvMsg);

                    OnCNCDataReceived?.Invoke();
                }
            }

            double recvCountDelta = (_recvBytesOfSocket - _lastPrintRecvBytesOfSocket);
            double elapseSec = (DateTime.Now - _lastPrintSocketStatisticTime).TotalSeconds;
            if (elapseSec > 120)
            {
                double count_per_sec = recvCountDelta / elapseSec;

                _lastPrintSocketStatisticTime = DateTime.Now;
                _lastPrintRecvBytesOfSocket = _recvBytesOfSocket;

                _logger.LogDebug($"CNC8X statistic : recv bytes per sec {count_per_sec:F2} ");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private string GetFmtTimeBySeconds(string strSeconds)
    {
        string strTime = "";
        try
        {
            TimeSpan ts = new TimeSpan(0, 0, int.Parse(strSeconds));
            int Hours = int.Parse(strSeconds) / 3600;
            strTime = Hours.ToString() + ":" + string.Format("{0:00}", ts.Minutes) + ":" + string.Format("{0:00}", ts.Seconds);
            return strTime;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return strTime;
    }

    private void ParseCncStatusText(string strCncStatusText)
    {
        try
        {
            if (!string.IsNullOrEmpty(strCncStatusText))
            {
                string[] aryStrStatus = strCncStatusText.Split(",");
                foreach (var item in aryStrStatus)
                {
                    if (item.StartsWith("AR")) { string AR = item.Replace("AR", ""); if (CncStatus.AR != AR) CncStatus.AR = AR; }
                    if (item.StartsWith("AP")) { string AP = item.Replace("AP", ""); if (CncStatus.AP != AP) CncStatus.AP = AP; }
                    if (item.StartsWith("ZS")) { string ZS = item.Replace("ZS", ""); if (CncStatus.ZS != ZS) CncStatus.ZS = ZS; }
                    if (item.StartsWith("MO")) { string MO = item.Replace("MO", ""); if (CncStatus.MO != MO) CncStatus.MO = MO; }
                    if (item.StartsWith("EC")) { string EC = item.Replace("EC", ""); if (CncStatus.EC != EC) CncStatus.EC = EC; }
                    if (item.StartsWith("FN")) { string FN = item.Replace("FN", ""); if (CncStatus.FN != FN) CncStatus.FN = FN; }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ParseCncStatusText Exception - " + ex.Message);
        }
    }


    private async Task<bool> ExecuteCM(CancellationToken cancellationToken = default)
    {
        try
        {
            string msg = GetCncCommandStr(CNC8XCmd.CM_NO_CONFIRM);

            _logger.LogDebug($"CM : pre send msg '{msg}' ");

            //Plan A : 实测发现， 有时候CNC不会给回复。
            //var ack = await _asyncTaskWaiter.ExecuteTaskAsWaiter(CNC8XCmd.CM_NO_CONFIRM,
            //                    SendMessageToCNC84(msg, 500),
            //                    cancellationToken);
            //string ackMsgRes = ack.ToStr();
            //if (string.IsNullOrEmpty(ackMsgRes))
            //{
            //    return false;
            //}

            //Plan B : 只发送请求，不等回复了
            await SendMessageToCNC84(msg, 500);

            bool isDone = false;
            string curPgm = "";

            int waitLoopCount = 0;
            DateTime totPoint = DateTime.Now.AddSeconds(15);
            while (DateTime.Now < totPoint)
            {
                waitLoopCount += 1;

                curPgm = await this.RetrieveData("PgmFilePath");

                if (string.IsNullOrEmpty(curPgm))
                {
                    _logger.LogDebug($"CM : is done.  cur pgm '{curPgm}'. ");

                    isDone = true;
                    break;
                }
                else
                {
                    if (waitLoopCount % 6 == 0) _logger.LogDebug($"CM : waiting for result ...  cur pgm '{curPgm}'. ");
                }

                await Task.Delay(750);
            }

            if (false == isDone) _logger.LogDebug($"CM : waiting for result time out !!!  cur pgm '{curPgm}'. ");

            return isDone;
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exp2205021 CM : exp " + exp.Message);
            _logger.LogError($"Exp2205021 CM : exp " + exp.StackTrace);
        }
        return false;
    }


    public async Task<bool> CncSetXYOfProgramZero(double x, double y, CancellationToken cancellationToken = default)
    {
        try
        {
            string finalCmd = CNC8XCmd.FA + $"X{x:F4}Y{y:F4}";
            string msg = GetCncCommandStr(finalCmd);

            if (_eAPClientOptions.IsDebug) _logger.LogDebug($"Set program zero : pre send msg '{msg}' ");
            var ack = await _asyncTaskWaiter.ExecuteTaskAsWaiter(CNC8XCmd.FA,
                                SendMessageToCNC84(msg, 500),
                                cancellationToken);
            string ackMsgRes = ack.ToStr();
            if (string.IsNullOrEmpty(ackMsgRes))
            {
                return false;
            }

            DateTime totPoint = DateTime.Now.AddSeconds(15);

            bool allFit = false;

            string xCur = "";
            string yCur = "";
            int waitLoopCount = 0;
            while (DateTime.Now < totPoint)
            {
                waitLoopCount += 1;
                xCur = await RetrieveData(CncDataNameUnity.XOfBasePoint);
                yCur = await RetrieveData(CncDataNameUnity.YOfBasePoint);

                if (false == string.IsNullOrEmpty(xCur) && false == string.IsNullOrEmpty(yCur))
                {
                    double xCurVV = StringUtil.TryParseDouble(xCur, x + 9000);
                    double yCurVV = StringUtil.TryParseDouble(yCur, y + 9000);

                    if (waitLoopCount % 4 == 0) _logger.LogDebug($"Set x y of zero : Des {x:F2},{y:F2} . Ack of set '{ackMsgRes}'. cur x {xCur}, cur y {yCur}. ");

                    if (Math.Abs(x - xCurVV) < 0.01 && Math.Abs(y - yCurVV) < 0.01)
                    {
                        allFit = true;
                        break;
                    }
                }
                else
                {
                    if (waitLoopCount % 4 == 0) _logger.LogDebug($"Set x y of zero : Des {x:F2},{y:F2} . Ack of set '{ackMsgRes}'. Get cur xy failed !  Fit {allFit}.");
                }

                await Task.Delay(500);
            }

            _logger.LogDebug($"Set x y of zero : Des {x:F2},{y:F2} . Ack of set '{ackMsgRes}'. cur x {xCur}, cur y {yCur}. Fit {allFit}.");
            return allFit;
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exp2203005 " + exp.Message);
        }
        return false;
    }

    #region LoadFile

    private async Task<string> GetCurPgmFilePathAsync(string key = "PgmFilePath", CancellationToken cancellationToken = default)
    {
        try
        {
            string sendMsg = GetRequestStr(CNCRequestPacketKind.ACTPROGRAM);
            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            SendMessageToCNC84(sendMsg),
                            cancellationToken);
            return result.ToStr() ?? "NULL";//失败时不应返回 string.Empty 。此时CNC实际的当前程序可能依然在， 会导致业务逻辑误判
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return "NULL";
    }

    private async Task<string> GetCurDiaFilePathAsync(string key = "DiaFilePath", CancellationToken cancellationToken = default)
    {
        try
        {
            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMESTRING, "%S(DiaFileNameWithDialog,0)");
            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
            return result.ToStr() ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private async Task<string> GetCurAtpFilePathAsync(string key = "AtpFilePath", CancellationToken cancellationToken = default)
    {
        try
        {
            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMESTRING, "%S(ATPFileNameWithDialog,0)");
            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
            return result.ToStr() ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return string.Empty;
    }

    private bool AreConditionsMeetForLoadFile(out string failMsg)
    {
        try
        {
            if (string.IsNullOrEmpty(_cncStatusData))
            {
                failMsg = "CNC status is empty";
                return false;
            }
            if (_cncStatusData.Contains("MOWORK"))
            {
                failMsg = "CNC is working";
                return false;
            }

            failMsg = "";
            return true;
        }
        catch (Exception ex)
        {
            failMsg = $"Exp : " + ex.Message;
            _logger.LogError(ex, ex.Message);
        }
        return false;
    }

    public async Task<bool> CncLoadFile(LoadFileType loadFileType, string strFilePath, CancellationToken cancellationToken = default)
    {
        string strLoadResult = string.Empty;
        bool loadCmdSended = false;
        try
        {
            string failMsg = "";
            bool fit = AreConditionsMeetForLoadFile(out failMsg);

            if (fit)
            {
                string msg = GetCommandLoadFileStr(strFilePath);
                string key = "LoadFile";

                _logger.LogDebug($"Load file : pre send msg '{msg}' ");

                //发现 ：正常情况下， @LF 消息发送后，虽然CNC已经正常加载消息了。 但是 收不到@LF对应的回复消息。
                // 如果在这里下个断点， 就能收到 @LF 对应的回复消息了。
                var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                    SendMessageToCNC84(msg, 800),
                                    cancellationToken);
                string strResult = result.ToStr();
                _logger.LogDebug($"Load file : get ack '{strResult}' ");

                _logger.LogDebug($"Load file : CncLoadFile - {msg} Result - {strResult}");

                if (!string.IsNullOrEmpty(strResult))
                {
                    await Task.Delay(4000);
                    loadCmdSended = true;
                }
                else
                {
                    strLoadResult = "NG_Load file Error.";
                }
            }
            else
            {
                strLoadResult = failMsg;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            strLoadResult = "NG_" + ex.Message;
        }

        _logger.LogDebug($"Load file : CncLoadFile - {strFilePath} Result - {strLoadResult}");

        return await TrySetLoadResult(loadFileType, strFilePath, strLoadResult, loadCmdSended);
    }

    private async Task<bool> TrySetLoadResult(LoadFileType loadFileType, string strFilePath, string strLoadResult, bool loadCmdSended)
    {
        string strCurFilePath = "";
        try
        {
            _logger.LogDebug($"Load file : check load effect begin. '{loadFileType}', '{strFilePath}', '{strLoadResult}'. Load Cmd Sended {loadCmdSended}");

            if (loadCmdSended)
            {
                int maxTryCount = 3;
                switch (loadFileType)
                {
                    case LoadFileType.PROGRAM:
                        {
                            maxTryCount = 32;
                        }
                        break;

                    case LoadFileType.DIA:
                        {
                            maxTryCount = 6;
                        }
                        break;

                    case LoadFileType.ATP:
                        {
                            maxTryCount = 8;
                        }
                        break;
                }

                int nTry = 0;
                do
                {
                    await Task.Delay(1000);

                    switch (loadFileType)
                    {
                        case LoadFileType.PROGRAM:
                            {
                                strCurFilePath = await GetCurPgmFilePathAsync();

                                _logger.LogDebug($"Load file : check res. cur pgm : '{strCurFilePath}' ， req '{strFilePath}' .");

                                //if (strCurFilePath.Equals(strFilePath, StringComparison.OrdinalIgnoreCase))//发现路径有汉字时，此方法可能失灵
                                if (strCurFilePath.Trim().ToUpper() == strFilePath.Trim().ToUpper())
                                {
                                    strLoadResult = "OK";
                                    return SetLoadFileResult(loadFileType, strFilePath, strLoadResult);
                                }
                            }
                            break;

                        case LoadFileType.DIA:
                            {
                                strCurFilePath = await GetCurDiaFilePathAsync();
                                _logger.LogDebug($"Load file : check res. cur dia : '{strCurFilePath}', req file '{strFilePath}'. ");

                                //if (strCurFilePath.Equals(strFilePath, StringComparison.OrdinalIgnoreCase))//发现路径有汉字时，此方法可能失灵
                                if (strCurFilePath.Trim().ToUpper() == strFilePath.Trim().ToUpper())
                                {
                                    strLoadResult = "OK";
                                    return SetLoadFileResult(loadFileType, strFilePath, strLoadResult);
                                }
                            }
                            break;

                        case LoadFileType.ATP:
                            {
                                strCurFilePath = await GetCurAtpFilePathAsync();
                                _logger.LogDebug($"Load file : check res. cur atp : '{strCurFilePath}', req '{strFilePath}'.  ");

                                //if (strCurFilePath.Equals(strFilePath, StringComparison.OrdinalIgnoreCase))//发现路径有汉字时，此方法可能失灵
                                if (strCurFilePath.Trim().ToUpper() == strFilePath.Trim().ToUpper())
                                {
                                    strLoadResult = "OK";
                                    return SetLoadFileResult(loadFileType, strFilePath, strLoadResult);
                                }
                            }
                            break;
                    }
                }
                while (nTry++ <= maxTryCount);
            }

            _logger.LogDebug($"Load file : check load effect time out ! '{loadFileType}', '{strFilePath}', '{strLoadResult}'. Load Cmd Sended {loadCmdSended}. Cur file '{strCurFilePath}'.");
            await Task.Delay(1000);
            return SetLoadFileResult(loadFileType, strFilePath, strLoadResult);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"Load file : exp " + ex.Message);
            _logger.LogError(ex, ex.Message);
        }
        return false;
    }

    private bool SetLoadFileResult(LoadFileType loadFileType, string strFilePath, string strResult)
    {
        try
        {
            switch (loadFileType)
            {
                case LoadFileType.PROGRAM:
                    {
                        _asyncTaskWaiter.TrySetResult("LoadPgmFile_" + strFilePath, strResult.GetBytes());
                    }
                    break;

                case LoadFileType.DIA:
                    {
                        _asyncTaskWaiter.TrySetResult("LoadDiaFile_" + strFilePath, strResult.GetBytes());
                    }
                    break;

                case LoadFileType.ATP:
                    {
                        _asyncTaskWaiter.TrySetResult("LoadAtpFile_" + strFilePath, strResult.GetBytes());
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return true;
    }

    public Task<RetMsg> LoadCNCCommand(string cmd) => throw new NotImplementedException();

    #endregion LoadFile

    #region ForAutoList

    private void AutoListReaderLogImp(string line)
    {
        _logger.LogDebug(line);
    }

    private void AutoListReaderOnBatchReadEnd()
    {
        _indexOfCurJob = _cncAutoListReader.GetCurJobIndex();
        _startTimeOfCurJob = _cncAutoListReader.GetJobStartTime(_indexOfCurJob, DateTimeUtil.Future3000);
        _endTimeOfCurJob = _cncAutoListReader.GetJobEndTime(_indexOfCurJob, DateTimeUtil.Future3000);
    }

    private bool IsConnectionOK()
    {
        return this.IsConnected;
    }

    private long _readStartTimeCount = 0;
    private long _readLineCount = 0;

    private async Task<DateTime> ReadAutoListStartTimeImp(int jobIndex)
    {
        string key = CNC8XCmd.JOB_STARTTIME + $"({jobIndex})";
        string resText = await QueryRuntimeValueFrom8X(key);

        _readStartTimeCount += 1;
        if (_readStartTimeCount < 300)
        {
            _logger.LogDebug($"8xAutoList : read start time. {key} , res '{resText}'.");
        }

        if (string.IsNullOrEmpty(resText))
            return DateTime.MinValue;
        else
            return DateTime.Parse(resText);
    }

    private async Task<DateTime> ReadAutoListEndTime(int jobIndex)
    {
        string key = CNC8XCmd.JOB_ENDTIME + $"({jobIndex})";
        string resText = await QueryRuntimeValueFrom8X(key);

        if (string.IsNullOrEmpty(resText))
            return DateTime.MinValue;
        else
            return DateTime.Parse(resText);
    }

    private async Task<AutoListCache.LineData> ReadAutoListLineImp(int jobIndex, bool readStartTime)
    {
        AutoListCache.LineData line = new AutoListCache.LineData(jobIndex);

        try
        {
            if (readStartTime)
            {
                line.StartTime = await ReadAutoListStartTimeImp(jobIndex);
            }

            line.EndTime = await ReadAutoListEndTime(jobIndex);

            line.HoleCnt = StringUtil.TryParseLong(await QueryRuntimeValueFrom8X(CNC8XCmd.JOB_HOLE_CNT + $"({jobIndex})"), 0);

            {//WorkDurMin
                double work_dur_sec = StringUtil.TryParseDouble(await QueryRuntimeValueFrom8X(CNC8XCmd.JOB_TIME + $"({jobIndex})"), 0);
                line.WorkDurMin = (double)work_dur_sec / 60.0;
            }

            _readLineCount += 1;
            if (_readLineCount < 300)
            {
                _logger.LogDebug($"8xAutoList : read auto list end time. Index {jobIndex} : end t '{line.EndTime}'. hole {line.HoleCnt}, work dur min {line.WorkDurMin:F2}.");
            }

            return line;
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exp9001789 exp " + exp.Message);
        }
        return line;
    }

    private async Task<string> QueryRuntimeValueFrom8X(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, key);
            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                SendMessageToCNC84(sendMsg),
                cancellationToken);

            string resText = result.ToStr();
            if (string.IsNullOrEmpty(resText))
                return "";
            else
                return resText;
        }
        catch (Exception exp)
        {
            _logger.LogError($"Exp9001780 exp " + exp.Message);
        }
        return "";
    }

    private void HandleAutoList_JobStartTime(string recvMsg)
    {
        int jobIndex = -1;
        long time_stamp = StringUtil.TryParseLong(GetValueWithIndex(recvMsg, CNC8XCmd.JOB_STARTTIME, out jobIndex));
        string key = CNC8XCmd.JOB_STARTTIME + $"({jobIndex})";

        if (_readStartTimeCount < 300)
        {
            _logger.LogDebug($"8xAutoList : recv job start time. index {jobIndex}, recv msg '{recvMsg}'.");
        }

        var local_t = DateTimeUtil.UtcToLocalTime(DateTimeUtil.UnixTimestampToDateTime(time_stamp, DateTimeUtil.Future3000));
        _asyncTaskWaiter.TrySetResult(key, DateTimeUtil.DateTimeToDBString(local_t).GetBytes());
    }

    private void HandleAutoList_JobEndTime(string recvMsg)
    {
        int jobIndex = -1;
        long time_stamp = StringUtil.TryParseLong(GetValueWithIndex(recvMsg, CNC8XCmd.JOB_ENDTIME, out jobIndex));
        string key = CNC8XCmd.JOB_ENDTIME + $"({jobIndex})";

        var local_t = DateTimeUtil.UtcToLocalTime(DateTimeUtil.UnixTimestampToDateTime(time_stamp, DateTimeUtil.Future3000));
        _asyncTaskWaiter.TrySetResult(key, DateTimeUtil.DateTimeToDBString(local_t).GetBytes());
    }

    private void HandleAutoList_JobTime(string recvMsg)
    {
        int jobIndex = -1;
        GetValueWithIndex(recvMsg, CNC8XCmd.JOB_TIME, out jobIndex);

        string key = CNC8XCmd.JOB_TIME + $"({jobIndex})";
        _asyncTaskWaiter.TrySetResult(key, recvMsg.GetBytes());
    }

    private void HandleAutoList_JobHoleCnt(string recvMsg)
    {
        int jobIndex = -1;
        GetValueWithIndex(recvMsg, CNC8XCmd.JOB_HOLE_CNT, out jobIndex);

        string key = CNC8XCmd.JOB_HOLE_CNT + $"({jobIndex})";
        _asyncTaskWaiter.TrySetResult(key, recvMsg.GetBytes());
    }

    #endregion ForAutoList

    private async Task<bool> GetAutoListRunNumber(string key, CancellationToken cancellationToken = default)
    {
        string strResult = "0";

        string megdata = CNC8XCmd.HSYS55_JobIndex;
        string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, megdata, UseXmldoc: true);
        var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(megdata,
                                SendMessageToCNC84(sendMsg),
                                cancellationToken);
        string sAutoListSeletcIndex = result.ToStr();
        if (int.TryParse(sAutoListSeletcIndex, out int iAutoListSelectIndex))
        {
            iAutoListSelectIndex--;
            if (iAutoListSelectIndex >= 0)
            {
                megdata = $@"{CNC8XCmd.JOB_RUN}({iAutoListSelectIndex})";
                sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, megdata, UseXmldoc: true);
                result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(megdata,
                                    SendMessageToCNC84(sendMsg),
                                    cancellationToken);
                string sAutoListSelectIndexSeq = result.ToStr();
                if (int.TryParse(sAutoListSelectIndexSeq, out int iAutoListSelectIndexSeq))
                {
                    strResult = iAutoListSelectIndexSeq.ToStringEx();
                }
            }
        }
        _asyncTaskWaiter.TrySetResult(key, strResult.GetBytes());
        return true;
    }
}
