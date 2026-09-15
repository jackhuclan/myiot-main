// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SuperSocket.Client;
using SuperSocket.Connection;
using SuperSocket.ProtoBase;
using VgAutoDrill.Fundation.CNC.Packet;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.Status;

namespace VgEAPClient.Common.CNC;

internal class EasyClientCNCConnector : ICNCConnector
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
    private readonly object _lockSendCNC84Socket = new object();
    private readonly ILogger<EasyClientCNCConnector> _logger;
    private readonly IAsyncTaskWaiter _asyncTaskWaiter;
    private static readonly int _refreshRouteCodes = 0;
    private readonly EAPClientOptions _eAPClientOptions;
    private readonly string _oldCncStatusEc = string.Empty;
    private readonly bool _isNewRun = false;
    private readonly string _oldPgmRunStartTime = string.Empty;
    private readonly string _oldPgmRunEndTime = string.Empty;
    private readonly string _oldCncComm = string.Empty;
    private readonly IEasyClient<SMDncPacket> _easyClient;
    private readonly IPackageEncoder<SMDncPacket> _packageEncoder;

    public EasyClientCNCConnector(ILogger<EasyClientCNCConnector> logger,
        IOptions<EAPClientOptions> options,
        IAsyncTaskWaiter asyncTaskWaiter)
    {
        _logger = logger;
        _asyncTaskWaiter = asyncTaskWaiter;
        _eAPClientOptions = options.Value;

        _packageEncoder = new SMDncPacketEncoder();
        _easyClient = new EasyClient<SMDncPacket>(new SMDncPacketFilter(), new ConnectionOptions
        {
            Logger = _logger,
        }).AsClient();
        _easyClient.PackageHandler += Client_PackageHandler;

        //_socketCNC84.ReceiveBufferSize = _receiveBufferSize;
        //_socketCNC84.DataReceived += socketCNC84_DataReceived;
        //_socketCNC84.Connected += socketCNC84_Connected;
        //_socketCNC84.Error += socketCNC84_Error;
        //_socketCNC84.Closed += socketCNC84_Closed;

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
        var connected = await _easyClient.ConnectAsync(new IPEndPoint(IPAddress.Parse(_eAPClientOptions.CNC84Server), _eAPClientOptions.CNC84Port));
        _easyClient.StartReceive();
    }

    private ValueTask Client_PackageHandler(EasyClient<SMDncPacket> sender, SMDncPacket packet)
    {
        switch (packet.CmdType)
        {
            //_request
            case 1:
                break;

            //_execute
            case 2:
                _asyncTaskWaiter.TrySetResult(packet.Value, packet.ToString().GetBytes());
                break;
            //_advice
            case 3:
                _asyncTaskWaiter.TrySetResult(packet.Value, packet.ToString().GetBytes());
                break;
            //_adviceStart
            case 4:
                _asyncTaskWaiter.TrySetResult(packet.Value, packet.ToString().GetBytes());
                break;
            //_adviceStop
            case 5:
                _asyncTaskWaiter.TrySetResult(packet.Value, packet.ToString().GetBytes());
                break;
        }

        return ValueTask.CompletedTask;
    }

    public async Task Disonnect(CancellationToken cancellationToken) => await _easyClient.CloseAsync();

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
                            var packet = new SMDncPacket
                            {
                                CNC = new VgAutoDrill.Fundation.CNC.Packet.CNC
                                {
                                }
                            };

                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParD({GetToolIndexById(strInfo[1])})");
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                _easyClient.SendAsync(_packageEncoder, packet).AsTask(),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "ToolS":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParS({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ToolF":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParF({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ToolR":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParR({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ToolN":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParN({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ToolB":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TParB({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "LoadPgmFile":
                        {
                            var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                                CncLoadFile(LoadFileType.PROGRAM, strValue),
                                cancellationToken);
                            return result.ToStr() ?? string.Empty;
                        }
                    case "LoadDiaFile":
                        {
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    CncLoadFile(LoadFileType.DIA, strValue),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "LoadAtpFile":
                        {
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    CncLoadFile(LoadFileType.ATP, strValue),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "SpindleEnable":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"ZS_ZSEL({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "TMeasureDia":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TMesD({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "TMeasureLen":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TMesL({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "TRunout":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"TMesR({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "Spindle1WorkTimes":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, $"PC_SpinHisTime({GetToolIndexById(strInfo[1])})");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                }
            }
            else
            {
                switch (key)
                {
                    case "CncStatus":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.CNCSTATUS);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "BlockText":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.SCREENSAVER);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "CncError":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.CNCERROR);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
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
                            //if (!string.IsNullOrEmpty(CncStatus.AR) && CncStatus.AR != "00:00:00")
                            //{
                            //    DateTime dtAR = DateTime.ParseExact(CncStatus.AR, "HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                            //    TimeSpan ts = dtAR - DateTime.ParseExact("00:00:00", "HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

                            //    if (ts.TotalSeconds > 0 && ts.TotalSeconds <= 4 && _isNewRun == false)
                            //    {
                            //        _isNewRun = true;
                            //        _oldPgmRunStartTime = DateTime.Now.AddSeconds(-ts.TotalSeconds).ToString("yyyy/MM/dd HH:mm:ss");
                            //        return _oldPgmRunStartTime;
                            //    }
                            //}
                            return _oldPgmRunStartTime;
                        }
                    case "PgmRunEndTime":
                        {
                            //if (_isNewRun)
                            //{
                            //    if (_oldCncStatusEc != CncStatus.EC && CncStatus.EC.Equals("0048"))
                            //    {
                            //        _isNewRun = false;
                            //        _oldCncStatusEc = CncStatus.EC;
                            //        _oldPgmRunEndTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            //        return _oldPgmRunEndTime;
                            //    }
                            //    _oldCncStatusEc = CncStatus.EC;
                            //}
                            //else
                            //{
                            //    return _oldPgmRunEndTime;
                            //}
                            return string.Empty;
                        }
                    case "PgmFilePath":
                        {
                            //return await GetCurPgmFilePathAsync(key);

                            return string.Empty;
                        }
                    case "DiaFilePath":
                        {
                            // return await GetCurDiaFilePathAsync(key);
                            return string.Empty;
                        }
                    case "AtpFilePath":
                        {
                            //return await GetCurAtpFilePathAsync(key);
                            return string.Empty;
                        }
                    case "CurDrillOrRout":
                    case "TotalDrillOrRout":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMESTRING, "%S(HFCNTValue)");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "Duty":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.DUTY);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ShiftOnlineTime":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeTimes(1262)");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ShiftWorkingTime":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeTimes(1363)");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ShiftWaitingTime":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeTimes(1464)");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ShiftErrorTime":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeTimes(1565)");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "XYPosition":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.XYPOSITION);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "CurSpindleStatus":
                        {
                            return CncStatus.ZS ?? string.Empty;
                        }
                    case "CurToolId":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.CNCTOOLS);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "DrillH":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComH");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "DrillQ":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComQUIK");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "DrillK":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComK");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "DrillZ":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComZ");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "Block":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "HSYS55_Block");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "Step":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "HSYS55_Step");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "ShiftTotalDrillOrRout":
                        {
                            return string.Empty;
                        }
                    case "ShiftRunCount":
                        {
                            return string.Empty;
                        }
                    case "PreDuty":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "PC_HisBdeData(1868)");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "OPID":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.OPID);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "SAX":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ViewComSAX");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "SAY":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ViewComSAY");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "SAZX":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComSAZX");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "SAZY":
                        {
                            //string sendMsg = GetExecuteStr(CNCExecutePacketKind.RUNTIMEVALUE, "ComSAZY");
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "UserName":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.USERNAME);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
                        }
                    case "UserLevel":
                        {
                            //string sendMsg = GetRequestStr(CNCRequestPacketKind.USERLEVEL);
                            //var result = await _asyncTaskWaiter.ExecuteTaskAsWaiter(key,
                            //    SendMessageToCNC84(sendMsg),
                            //    cancellationToken);
                            //return result.ToStr() ?? string.Empty;
                            return string.Empty;
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

    public BrokenToolData ParseBrokenToolData(BrokenToolData brokenToolData, string blockText, CancellationToken cancellationToken = default) => throw new NotImplementedException();


    public Task<bool> CncSetXYOfProgramZero(double x, double y, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task<bool> CncLoadFile(LoadFileType loadFileType, string strFilePath, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task<RetMsg> LoadCNCCommand(string cmd) => throw new NotImplementedException();
}
