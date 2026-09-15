// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz.Util;
using SqlSugar;
using VegaUI;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgCNCServer.DAO;
using VgCNCServer.DAO.Models;
using VgEAPClient.Common;
using VgEAPClient.Common.CNC;
using VgEAPClient.Common.CNC.ATP;
using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.Status;
using VgEAPClient.Common.Communication;
using VgEAPClient.Common.Communication.Inbound;
using VgEAPClient.Common.Communication.Outbound;
using VgEAPClient.Common.Configuration;
using VgEAPClient.Common.OpcUaServer;
using VgEAPClient.Model;
using WindowsFormsLifetime;
using Parameter = VgEAPClient.Common.Communication.Parameter;

namespace VgEAPClient;

public partial class FrmMain : Form
{
    private readonly ILogger<FrmMain> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IObjectFactory _objectFactory;
    private readonly IEQPDataReporter _httpDataReporter;
    private readonly IEQPDataReceiver _httpDataReceiver;
    private readonly IEAPHeartbeater _eAPHeartbeater;
    private readonly IConfigurationFileReplacer _configurationFileReplacer;
    private readonly ICncFileLoader _cncFileLoader;
    private readonly IOpcUaServerStartup _opcUaServerStartup;
    private readonly IAmmeterFactory _ammeterFactory;
    private readonly IWorkOrderRecipeLoader _workOrderRecipeLoader;
    private readonly IGuiContext _guiContext;
    private readonly IDbContextFactory<VegaContext> _contextFactory;
    private readonly SetuoWindowOpenRun _setuoWindowOpenRun;
    private readonly IFormProvider _formProvider;
    public readonly EAPClientOptions _eAPClientOptions;
    public readonly AOIDbOptions _aOIDbOptions;
    private readonly HttpDataReporterOptions _reporterOptions;
    private readonly HttpDataReceiverOptions _receiverOptions;
    private readonly LotInfo _curLotInfo;
    private readonly List<CimMsgInfo> _listCimMsgInfo = new List<CimMsgInfo>();
    private readonly SettingsInfo _settingsInfo;
    private readonly ICNCConnector _cNCConnector;
    private readonly DrillCommonDataA _drillCommonDataA;
    private readonly DrillCommonDataB _drillCommonDataB;
    private readonly DrillCommonDataC _drillCommonDataC;
    private readonly DrillStatusData _drillStatusData;

    private bool _isAlarm = false;
    private string _lastAlarmEc = string.Empty;
    private string _lastAlarmText = string.Empty;
    private bool _isFrmMainLoaded = false;
    private bool _isPgmBrokenTool = false;
    private int _nChangePanelTimes = 0;
    private int _nChangeDrillTimes = 0;
    private int _nShiftBrokenToolTimes = 0;
    private List<WorkOrderRecipe> _filelistPgm = new List<WorkOrderRecipe>();
    private List<WorkOrderRecipe> _filelistDia = new List<WorkOrderRecipe>();
    private List<WorkOrderRecipe> _filelistAtp = new List<WorkOrderRecipe>();
    private bool _isDirectLoadedFile = false;
    private readonly CancellationTokenSource cts;
    private IAmmeterIO? _ammeterIO;

    FrmBrokenKnifeReport _frmBrokenKnifeReport;

    private string _CurShift = "";
    private string _LastShift = "";
    private int _iShiftHits = 0;
    private List<FileInfo> _SearchPathList = [];

    private string _0ldPgmFile = string.Empty;
    private string _NewPgmFile = string.Empty;

    public FrmMain(ILogger<FrmMain> logger,
        ILoggerFactory loggerFactory,
        IObjectFactory objectFactory,
        IOptions<EAPClientOptions> options,
        IOptions<AOIDbOptions> aoidboptions,
        IOptions<HttpDataReporterOptions> reporterOptions,
        IOptions<HttpDataReceiverOptions> receiverOptions,
        IEQPDataReporter httpDataReporter,
        IEQPDataReceiver httpDataReceiver,
        IEAPHeartbeater eAPHeartbeater,
        ICNCOperatorProvider cNCOperatorProvider,
        IOpcUaServerStartup opcUaServerStartup,
        IAmmeterFactory ammeterFactory,
        IWorkOrderRecipeLoader workOrderRecipeLoader,
        IConfigurationFileReplacer configurationFileReplacer,
        IDataCollector<DrillCommonDataA> commonDataCollectorA,
        IDataCollector<DrillCommonDataB> commonDataCollectorB,
        IDataCollector<DrillCommonDataC> commonDataCollectorC,
        IDataCollector<DrillStatusData> statusDataCollector,
        ICncFileLoader cncFileLoader,
        IGuiContext guiContext,
        IGuiLogger guiLogger,
        IDbContextFactory<VegaContext> contextFactory,
        SetuoWindowOpenRun setuoWindowOpenRun,
        IFormProvider formProvider)
    {
        InitializeComponent();
        PageTran();
        _logger = logger;

        _eAPClientOptions = options.Value;
        _aOIDbOptions = aoidboptions.Value;
        _reporterOptions = reporterOptions.Value;
        _receiverOptions = receiverOptions.Value;
        GetLocalWebIpEndPoint();

        _loggerFactory = loggerFactory;
        _objectFactory = objectFactory;
        _httpDataReporter = httpDataReporter;
        _httpDataReceiver = httpDataReceiver;
        _eAPHeartbeater = eAPHeartbeater;

        _opcUaServerStartup = opcUaServerStartup;

        _ammeterFactory = ammeterFactory;
        _workOrderRecipeLoader = workOrderRecipeLoader;
        _configurationFileReplacer = configurationFileReplacer;
        _cncFileLoader = cncFileLoader;
        _drillCommonDataA = commonDataCollectorA.Data;
        _drillCommonDataB = commonDataCollectorB.Data;
        _drillCommonDataC = commonDataCollectorC.Data;
        _drillStatusData = statusDataCollector.Data;
        _guiContext = guiContext;
        _contextFactory = contextFactory;
        _setuoWindowOpenRun = setuoWindowOpenRun;
        _formProvider = formProvider;
        if (_eAPClientOptions.Language == "en")
        {
            _settingsInfo = new SettingsInfoEn(_eAPClientOptions, _reporterOptions);
            _curLotInfo = new LotInfoEn();
        }
        else
        {
            _settingsInfo = new SettingsInfo(_eAPClientOptions, _reporterOptions);
            _curLotInfo = new LotInfo();
        }

        _frmBrokenKnifeReport = new FrmBrokenKnifeReport(_httpDataReporter, _guiContext, _formProvider, options, this);

        _cNCConnector = cNCOperatorProvider.GetCNCConnector();
        _cNCConnector._drillStatusData = _drillStatusData;
        _cNCConnector._drillCommonDataA = _drillCommonDataA;
        _opcUaServerStartup.Start();

        _cNCConnector.OnCNCConnected += OnCNCConnected;
        _cNCConnector.OnCNCDisconnected += OnCNCDisconnected;
        _cNCConnector.OnCNCConnectFailed += OnCNCConnectFailed;
        _cNCConnector.OnCNCConnectException += OnCNCConnectException;
        _cNCConnector.OnCNCDisonnectException += OnCNCDisonnectException;
        _cNCConnector.OnCNCClosed += OnCNCClosed;
        _cNCConnector.OnCNCError += OnCNCError;

        _drillStatusData.OnCncStatusTextChanged += OnCncStatusTextChangedFunc;
        _drillStatusData.OnBrokenToolChanged += OnBrokenToolChangedFunc;
        _drillStatusData.OnCncProgramStartEvent += OnCncProgramStartEventFunc;
        _drillStatusData.OnCncProgramFinishedEvent += OnCncProgramFinishedEventFunc;
        _drillStatusData.OnCncStatusMoChanged += OnCncStatusMoChangedFunc;
        _drillStatusData.OnCncLoadFileFinished += OnCncLoadFileFinished;
        _drillCommonDataA.OnCncPgmFilePathChanged += OnCncPgmFilePathChangedFunc;

        _httpDataReceiver.OnLotInfoDownloadCommandReceived += OnLotInfoDownloadCommandReceivedFunc;
        _httpDataReceiver.OnDateTimeCommandReceived += OnDateTimeCommandReceivedFunc;
        _httpDataReceiver.OnCIMMessageCommandReceived += OnCIMMessageCommandReceivedFunc;
        _httpDataReceiver.OnRecipeValidationResultCommandReceived += OnRecipeValidationResultCommandReceivedFunc;
        _httpDataReceiver.OnAreYouThereReplyReceived += OnAreYouThereReplyReceivedFunc;

        _httpDataReporter.OnTick += OnHttpDataReporterTick;
        _httpDataReporter.OnTick += SaveUsrShiftDuty;
        _httpDataReporter.OnTick += SaveUsrShiftFinalDuty;
        _httpDataReporter.OnTick += SavesysDrillInformation;
        _httpDataReporter.OnTick += SaveusrDuty;
        _httpDataReporter.OnTick += SaveusrRealTimeDuty;
        _httpDataReporter.OnTick += Savesysloginlog;
        _httpDataReporter.OnTick += Savesysstaff;
        _httpDataReporter.OnTick += Saveusranalysisduty;
        _httpDataReporter.OnTick += LogCollectionDataOnUI;

        _drillStatusData.OnCncProgramFinishedEvent += Saveusrworkingcondition;
        _drillStatusData.OnCncProgramFinishedEvent += Saveusrtoolsbrokenend;
        _drillStatusData.OnBrokenToolChanged += Saveusrtoolsbroken;
        _drillStatusData.OnCncAlarmEndEvent += Saveusralarm;
        _drillStatusData.OnCncCommChanged += Saveusrcomm;
        _drillStatusData.OnCncM52StartEvent += Saveusrm52event;
        _drillStatusData.OnCncErrorChanged += SaveusrEvent;

        _httpDataReporter.OnLotInfoRequestReturn += OnLotInfoRequestReturn;

        _eAPHeartbeater.OnEAPConnectedChanged += OnEAPConnectedChanged;

        guiLogger.OnShowResult += ShowResult;
        guiLogger.OnShowWarn += ShowWarn;
        guiLogger.OnShowError += ShowError;
        guiLogger.OnShowRecv += ShowRecv;
        guiLogger.OnShowSend += ShowSend;

        cts = new CancellationTokenSource();
        Task.Run(() => OnTimerForRefresh(cts.Token));

        if (_eAPClientOptions.IsCollaborative)
        {
            Task.Run(ReaderDbLoadFileLogFun);
        }

        if (_aOIDbOptions.IsEnable)
        {
            Task.Run(GetAOIDbCheck);
        }

        if (_eAPClientOptions.RecipeSearchPathTimeFresh > 0)
        {
            Task.Run(GetSearchPathList);
        }
    }

    #region AOI 
    private async Task GetAOIDbCheck()
    {
        while (true)
        {
            try
            {
                if (_isFrmMainLoaded)
                {
                    await _cNCConnector.RetrieveData("GetAOIBDCheck");
                }
            }
            catch (Exception ex)
            {
                ShowError($@"GetAOIDbCheck - {ex.Message}");
                _logger.LogError(ex, $@"GetAOIDbCheck - {ex.Message}");
            }
            await Task.Delay(1000);
        }
    }
    #endregion

    #region CNC ConnectFun

    private void OnCNCError()
    {
        try
        {
            if (_eAPClientOptions.DeviceDescriptor.DeviceKind == DeviceKind.CNC84Drill)
            {
                cnc84Swith.Active = false;
            }
            else
            {
                cnc95Switch.Active = false;
            }
            ShowResult("CNC 连接错误".VgTs());
        }
        catch (Exception ex)
        {
            ShowError($@"OnCNCError - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCNCDisonnectException(Exception ex)
    {
        try
        {
            ShowResult("CNC 断开错误".VgTs() + " - " + ex.Message);
        }
        catch (Exception ex2)
        {
            ShowError($@"OnCNCDisonnectException - {ex2.Message}");
            _logger.LogError(ex2, ex2.Message);
        }
    }

    private void OnCNCClosed()
    {
        try
        {
            if (_eAPClientOptions.DeviceDescriptor.DeviceKind == DeviceKind.CNC84Drill)
            {
                cnc84Swith.Active = false;
            }
            else
            {
                cnc95Switch.Active = false;
            }
            ShowResult("CNC 连接关闭".VgTs());
        }
        catch (Exception ex)
        {
            ShowError($@"OnCNCClosed - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCNCConnectException(Exception ex)
    {
        try
        {
            if (_eAPClientOptions.DeviceDescriptor.DeviceKind == DeviceKind.CNC84Drill)
            {
                cnc84Swith.Active = false;
            }
            else
            {
                cnc95Switch.Active = false;
            }
            ShowResult("CNC 连接异常".VgTs() + " - " + ex.Message);
        }
        catch (Exception ex2)
        {
            ShowError($@"OnCNCConnectException - {ex2.Message}");
            _logger.LogError(ex2, ex2.Message);
        }
    }

    private void OnCNCConnectFailed()
    {
        try
        {
            if (_eAPClientOptions.DeviceDescriptor.DeviceKind == DeviceKind.CNC84Drill)
            {
                cnc84Swith.Active = false;
            }
            else
            {
                cnc95Switch.Active = false;
            }
            ShowResult("CNC 连接失败".VgTs());
        }
        catch (Exception ex)
        {
            ShowError($@"OnCNCConnectFailed - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCNCDisconnected()
    {
        try
        {
            if (_eAPClientOptions.DeviceDescriptor.DeviceKind == DeviceKind.CNC84Drill)
            {
                cnc84Swith.Active = false;
            }
            else
            {
                cnc95Switch.Active = false;
            }
            ShowResult("CNC 已断开".VgTs());
        }
        catch (Exception ex)
        {
            ShowError($@"OnCNCDisconnected - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCNCConnected()
    {
        try
        {
            if (_eAPClientOptions.DeviceDescriptor.DeviceKind == DeviceKind.CNC84Drill)
            {
                cnc84Swith.Active = true;
            }
            else
            {
                cnc95Switch.Active = true;
            }
            ShowResult("CNC 连接成功".VgTs());
        }
        catch (Exception ex)
        {
            ShowError($@"OnCNCConnected - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    #endregion

    #region CommonData Fun

    private void OnCncPgmFilePathChangedFunc()
    {
        try
        {
            //程序文件变动触发时 _drillCommonDataA.ProgramPath 会出现 NULL 和 空等情况  要把这两个异动排除

            if (!_drillCommonDataA.ProgramPath.IsNullOrEmpty() && !_drillCommonDataA.ProgramPath.Trim().Equals("NULL", StringComparison.OrdinalIgnoreCase))
            {
                _NewPgmFile = _drillCommonDataA.ProgramPath;
                if (_0ldPgmFile.IsNullOrEmpty())
                {
                    _0ldPgmFile = _NewPgmFile;
                    ShowResult($@"初始程序载入[{_drillCommonDataA.ProgramPath}]", "PgmFilePathChange");
                }
                else
                {
                    if (_0ldPgmFile != _NewPgmFile)
                    {
                        _nChangeDrillTimes++;
                        _0ldPgmFile = _NewPgmFile;
                        ShowResult($@"当前程序以变更为[{_NewPgmFile}]", "PgmFilePathChange");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowError($@"OnCncPgmFilePathChangedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCncLoadFileFinished()
    {
        try
        {
            string strLoadedFilePathResult = _drillStatusData.CncLoadFileResult.GetAllLoadResult();

            Task.Run(() =>
            {
                ShowWarnBox(strLoadedFilePathResult, _eAPClientOptions.IsLoadFileMsgBoxShow);
            });

            _logger.LogInformation($"OnCncLoadFileFinished - {strLoadedFilePathResult}");
        }
        catch (Exception ex)
        {
            ShowError($@"OnCncLoadFileFinished - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void SaveusrEvent()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrEvent info = new()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    dtDate = DateOnly.FromDateTime(dateTime),
                    sTime = dateTime.ToString("HH:mm:ss"),
                    sEventID = _drillStatusData.CncStatusEc,
                    sActProgram = _drillCommonDataA.ProgramPath,
                    sEventDesc = _drillStatusData.CncError,
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                };
                _context.usrEvents.Add(info);
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
        }
    }

    private void Saveusralarm()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrAlarm info = new usrAlarm()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    dtDate = DateOnly.FromDateTime(dateTime),
                    sStartTime = _drillStatusData.CncAlarmData.AlarmStartTime,
                    sEndTime = _drillStatusData.CncAlarmData.AlarmEndTime,
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sAlarmID = _drillStatusData.CncAlarmData.AlarmId,
                    sAlarmDesc = _drillStatusData.CncAlarmData.AlarmDesc,
                    sActProgram = _drillCommonDataA.ProgramPath,
                };

                _context.usrAlarms.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给usrAlarms完成");
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给usrAlarms 时报错: {ex.Message}");
            return;
        }
    }

    private void Saveusrcomm()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrCOMM info = new usrCOMM()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    dtDate = DateOnly.FromDateTime(dateTime),
                    sTime = dateTime.ToString("HH:mm:ss"),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sCOMM = _drillStatusData.CncComm,
                    sActProgram = _drillCommonDataA.ProgramPath,
                    sEventCode = _drillStatusData.CncComm.EndsWith(";3398") ? "3398" : "537067521"
                };

                _context.usrCOMMs.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给usrCOMM完成");
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给usrCOMM 时报错: {ex.Message}");
        }
    }

    private void Saveusrm52event()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrM54Event info = new usrM54Event()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    dtDate = DateOnly.FromDateTime(dateTime),
                    sTime = dateTime.ToString("HH:mm:ss"),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sEventID = "3398",
                    sEventDesc = "COMM M52",
                    sActProgram = _drillCommonDataA.ProgramPath,
                };

                _context.usrM54Events.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给usrM54Event完成");
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给usrM54Event 时报错: {ex.Message}");
        }
    }

    private void Saveusrtoolsbroken()
    {
        try
        {


            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrToolsBroken info = new usrToolsBroken()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    dtDate = DateOnly.FromDateTime(dateTime),
                    sTime = dateTime.ToString("HH:mm:ss"),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sActProgram = _drillCommonDataA.ProgramPath,
                    sBrokens = _nShiftBrokenToolTimes.ToString(),
                    sSpindle = _drillStatusData.BrokenToolData.BrkToolSpindle,
                    sToolID = _drillStatusData.BrokenToolData.BrkToolId,
                    sToolDIA = _drillStatusData.BrokenToolData.BrkToolDia,
                    sHoleID = _drillStatusData.CurDrillOrRout,
                    sBrokenLife = _drillStatusData.CncToolData.ToolB,
                    sX = _drillCommonDataA.XPosition,
                    sY = _drillCommonDataA.YPosition,
                    sProgramBlock = _drillCommonDataA.Block,
                    sProgramStep = _drillCommonDataA.Step
                };

                _context.usrToolsBrokens.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给usrToolsBroken完成");
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给usrToolsBroken 时报错: {ex.Message}");
            return;
        }
    }

    private void Saveusrtoolsbrokenend()
    {
        try
        {
            if (_isPgmBrokenTool)
            {
                using (var _context = _contextFactory.CreateDbContext())
                {
                    DateTime dateTime = DateTime.Now;
                    usrToolsBrokenEnd info = new usrToolsBrokenEnd()
                    {
                        sInnerID = Guid.NewGuid().ToString(),
                        dtDate = DateOnly.FromDateTime(dateTime),
                        sTime = dateTime.ToString("HH:mm:ss"),
                        sEquipmentID = _eAPClientOptions.EquipmentID,
                        sEqpType = _eAPClientOptions.EqpType,
                        sActProgram = _drillCommonDataA.ProgramPath,
                        sBrokens = _nShiftBrokenToolTimes.ToString(),
                        sSpindle = _drillStatusData.BrokenToolData.BrkToolSpindle,
                        sToolID = _drillStatusData.BrokenToolData.BrkToolId,
                        sToolDIA = _drillStatusData.BrokenToolData.BrkToolDia,
                        sHoleID = _drillStatusData.CurDrillOrRout,
                        sX = _drillCommonDataA.XPosition,
                        sY = _drillCommonDataA.YPosition,
                        sProgramBlock = _drillCommonDataA.Block,
                        sProgramStep = _drillCommonDataA.Step
                    };

                    _context.usrToolsBrokenEnds.Add(info);
                    _context.SaveChanges();
                }
                _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给usrToolsBrokenEnd完成");
            }
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给usrToolsBrokenEnd 时报错: {ex.Message}");
            return;
        }
    }

    private void Saveusrworkingcondition()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;

                usrWorkingCondition info = new usrWorkingCondition()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    dtDate = DateOnly.FromDateTime(dateTime),
                    sTime = dateTime.ToString("HH:mm:ss"),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sT = _drillStatusData.CncStatusZs,
                    sActProgram = _drillCommonDataA.ProgramPath,
                    sNeeded = _drillCommonDataA.TotalDrillOrRout,
                    sStartTime = _drillStatusData.PgmRunStartTime,
                    sEndTime = _drillStatusData.PgmRunEndTime,
                    sWorkTime = _drillStatusData.PgmRunTotalTime,
                    sWaitTime = "0"
                };
                _context.usrWorkingConditions.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给usrWorkingCondition完成");
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给usrWorkingCondition 时报错: {ex.Message}");
            return;
        }
    }

    private void OnCncProgramFinishedEventFunc()
    {
        try
        {
            if (!_drillStatusData.IsCanSendPgmEnd)
            {
                return;
            }
            ShowResult("OnCncProgramFinishedEventFunc");
            _nChangePanelTimes++;
            _iShiftHits += _drillCommonDataA.CurDrillOrRout.ToInt();

            var body = new EQPSendOutJobReportBody()
            {
                EquipmentID = _eAPClientOptions.EquipmentID,
                LotID = _curLotInfo.LotID,
                ItemNum = _curLotInfo.ItemNum,
                RecipeID = _drillCommonDataA.ProgramPath,
                PanelID = _drillStatusData.sBarCode,
                Result = _drillStatusData.sResult,
            };

            _httpDataReporter.SendEQPSendOutJobReport(body);
            ShowSend($@"OnCncProgramFinishedEventFunc - SendEQPSendOutJobReport - {body.ToJson()}");
        }
        catch (Exception ex)
        {
            ShowError($@"OnCncProgramFinishedEventFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCncProgramStartEventFunc()
    {
        try
        {
            if (!_drillStatusData.IsCanSendPgmEnd)
            {
                return;
            }
            ShowResult("OnCncProgramStartEventFunc");
            _isPgmBrokenTool = false;

            var body = new EQPReceiveJobReportBody()
            {
                EquipmentID = _eAPClientOptions.EquipmentID,
                LotID = _curLotInfo.LotID,
                ItemNum = _curLotInfo.ItemNum,
            };
            _httpDataReporter.SendEQPReceiveJobReport(body);
            ShowSend($@"OnCncProgramStartEventFunc - SendEQPReceiveJobReport - {body.ToJson()}");
        }
        catch (Exception ex)
        {
            ShowError($@"OnCncProgramStartEventFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnBrokenToolChangedFunc()
    {
        try
        {
            ShowResult($@"OnBrokenToolChangedFunc - {_drillStatusData.CncStatusText}");
            _isPgmBrokenTool = true;

            if (!_isFrmMainLoaded)
            {
                ShowResult("OnBrokenToolChangedFunc FrmMain " + "未加载".VgTs());
                return;
            }
            if (!_drillStatusData.BrokenToolData.BrkToolId.IsNullOrWhiteSpace()
                || !_drillStatusData.BrokenToolData.BrkToolDia.IsNullOrWhiteSpace()
                || !_drillStatusData.BrokenToolData.BrkToolSpindle.IsNullOrWhiteSpace())
            {
                _nShiftBrokenToolTimes++;
                if (_eAPClientOptions.IsMsgBoxBrokenToolInfo)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(4000);

                        Invoke(() =>
                        {
                            _frmBrokenKnifeReport.InitFrmData(_curLotInfo.LotID, _curLotInfo.ItemNum, _drillStatusData);
                            _frmBrokenKnifeReport.StartPosition = FormStartPosition.CenterScreen;
                            _frmBrokenKnifeReport.Show();
                        });
                    });
                }
                else
                {
                    var body = new BrokenKnifeAlarmReportBody()
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        LotID = _curLotInfo.LotID,
                        ItemNum = _curLotInfo.ItemNum,
                        FilePath = _drillStatusData.CurProgramData.DiaFilePath,
                        DrillPath = _drillStatusData.CurProgramData.PgmFilePath,
                        KnifeSeq = _drillStatusData.BrokenToolData.BrkToolId,
                        AxisNum = _drillStatusData.BrokenToolData.BrkToolSpindle,
                        KnifeDia = _drillStatusData.BrokenToolData.BrkToolDia,
                        SetLife = _drillStatusData.CncToolData.ToolN,
                        UsedLife = _drillStatusData.CncToolData.ToolB,
                        BrokenTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        HolesNum = _drillStatusData.CurDrillOrRout,
                        PanelSite = "",
                    };
                    _httpDataReporter.SendBrokenKnifeAlarmReport(body);
                    ShowSend($@"OnBrokenToolChangedFunc - SendBrokenKnifeAlarmReport - {body.ToJson()}");
                }
            }
            else
            {
                ShowResult($@"OnBrokenToolChangedFunc - _drillStatusData.BrokenToolData " + "为空".VgTs());
            }
        }
        catch (Exception ex)
        {
            ShowError($@"OnBrokenToolChangedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCncStatusMoChangedFunc()
    {
        try
        {
            var body = new EQPStatusChangeReportBody
            {
                EquipmentID = _eAPClientOptions.EquipmentID,
                Status = GetMO(_drillStatusData.CncStatusMo),
            };
            _httpDataReporter.SendEQPStatusChangeReport(body);
            ShowSend($@"OnCncStatusMoChangedFunc - SendEQPStatusChangeReport - {body.ToJson()}");
        }
        catch (Exception ex)
        {
            ShowError($@"OnCncStatusMoChangedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCncStatusTextChangedFunc()
    {
        try
        {
            ShowResult($@"OnCncStatusTextChangedFunc - {_drillStatusData.CncStatusText}");
            ShowResult($@"OnCncStatusTextChangedFunc - {_drillStatusData.CncStatusMo} - EC - {_drillStatusData.CncStatusEc}");
            ShowResult($@"OnCncStatusTextChangedFunc - {_drillStatusData.BlockText}");

            if ((_drillStatusData.CncStatusMo.Equals("ALAM") || _cNCConnector.EcList.Contains(_drillStatusData.CncStatusEc)) && !_isAlarm)
            {
                _isAlarm = true;
                _lastAlarmEc = _drillStatusData.CncStatusEc;
                _lastAlarmText = _drillStatusData.BlockText;
                Task.Run(() =>
                {
                    var body = new EQPAlarmReportBody()
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        AlarmID = _drillStatusData.CncStatusEc,
                        AlarmLevel = "2",
                        AlarmStatus = "1",
                        AlarmText = _drillStatusData.BlockText,
                    };
                    _httpDataReporter.SendEQPAlarmReport(body);
                    ShowSend($@"OnCncStatusTextChangedFunc - SendEQPAlarmReport - {body.ToJson()}");
                });
            }
            else if (_drillStatusData.CncStatusMo.Equals("WORK") && _isAlarm)
            {
                _isAlarm = false;
                Task.Run(() =>
                {
                    var body = new EQPAlarmReportBody()
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        AlarmID = _lastAlarmEc,
                        AlarmLevel = "2",
                        AlarmStatus = "2",
                        AlarmText = _lastAlarmText,
                    };
                    _httpDataReporter.SendEQPAlarmReport(body);
                    ShowSend($@"OnCncStatusTextChangedFunc - SendEQPAlarmReport - {body.ToJson()}");
                });
            }
        }
        catch (Exception ex)
        {
            ShowError($@"OnCncStatusTextChangedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    #endregion

    #region HttpFun

    private async Task OnEAPConnectedChanged(bool connected)
    {
        try
        {
            if (connected)
            {
                cimSwitch.Active = true;

                var eQPCommunicationStatusReportBody = new EQPCommunicationStatusReportBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                    CommunicationStatus = _eAPClientOptions.CIMMode,
                };
                await _httpDataReporter.SendEQPCommunicationStatusReport(eQPCommunicationStatusReportBody);
                ShowSend($@"OnEAPConnectedChanged(ON) - SendEQPCommunicationStatusReport - {eQPCommunicationStatusReportBody.ToJson()}");

                var eQPDateTimeRequestBody = new EQPDateTimeRequestBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                    DateTime = "",
                };
                await _httpDataReporter.SendEQPDateTimeRequest(eQPDateTimeRequestBody);
                ShowSend($@"OnEAPConnectedChanged(ON) - SendEQPDateTimeRequest - {eQPDateTimeRequestBody.ToJson()}");

                var eQPStatusChangeReportBody = new EQPStatusChangeReportBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                    Status = GetMO(_drillStatusData.CncStatusMo),
                };
                await _httpDataReporter.SendEQPStatusChangeReport(eQPStatusChangeReportBody);
                ShowSend($@"OnEAPConnectedChanged(ON) - SendEQPStatusChangeReport - {eQPStatusChangeReportBody.ToJson()}");

                var eQPRunningModeReportBody = new EQPRunningModeReportBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                    Status = _eAPClientOptions.OperationMode,
                };
                await _httpDataReporter.SendEQPRunningModeReport(eQPRunningModeReportBody);
                ShowSend($@"OnEAPConnectedChanged(ON) - SendEQPRunningModeReport - {eQPRunningModeReportBody.ToJson()}");
            }
            else
            {
                cimSwitch.Active = false;
                var eQPCommunicationStatusReportBody = new EQPCommunicationStatusReportBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                };
                await _httpDataReporter.SendEQPCommunicationStatusReport(eQPCommunicationStatusReportBody);
                ShowSend($@"OnEAPConnectedChanged(OFF) - SendEQPCommunicationStatusReport - {eQPCommunicationStatusReportBody.ToJson()}");
            }
        }
        catch (Exception ex)
        {
            ShowError($@"OnEAPConnectedChanged - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private Task<LotInfoRequestModel> OnLotInfoRequestReturn(LotInfoRequestModel model)
    {
        try
        {
            ShowRecv($@"OnLotInfoRequestReturn - {model.ToJson()}");
            if (model != null)
            {
                if (!"1".Equals(model.Result.Code))
                {
                    Task.Run(() =>
                    {
                        ShowWarnBox(model.Result.MessageCH, true);
                    });
                }
            }
            else
            {
                model = new LotInfoRequestModel(
                    header: new EQPReportHeader(),
                    body: new LotInfoRequestBody(),
                    result: new EQPReportResult()
                );
            }
        }
        catch (Exception ex)
        {
            ShowError($@"OnLotInfoRequestReturn - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }

        return Task.FromResult(model);
    }

    private Task<AreYouThereReplyModel> OnAreYouThereReplyReceivedFunc(AreYouThereReplyModel model)
    {
        try
        {
            ShowRecv($@"OnAreYouThereReplyReceivedFunc - {model.ToJson()}");
            if (model != null)
            {
                if (_eAPClientOptions.bIsFirstSendIPPort)
                {
                    _eAPClientOptions.bIsFirstSendIPPort = false;
                    Task.Run(async () =>
                    {
                        var body = new IPPortReportBody
                        {
                            EquipmentID = _eAPClientOptions.EquipmentID,
                            IP = _eAPClientOptions.iPLocal?.Address.ToString(),
                            Port = _eAPClientOptions.iPLocal?.Port.ToString(),
                        };
                        await _httpDataReporter.SendIPPortReport(body);
                        ShowSend($@"OnAreYouThereReplyReceivedFunc - SendIPPortReport - {body.ToJson()}");
                    });
                }
            }
            else
            {
                model = new AreYouThereReplyModel();
            }
        }
        catch (Exception ex)
        {
            ShowError($@"OnAreYouThereReplyReceivedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
        return Task.FromResult(model);
    }

    private Task<RecipeValidationResultCommandModel> OnRecipeValidationResultCommandReceivedFunc(RecipeValidationResultCommandModel model)
    {
        try
        {
            ShowRecv($@"OnRecipeValidationResultCommandReceivedFunc - {model.ToJson()}");
            if (model.Body.Result == 1)
            {
                _guiContext.Invoke(() =>
                {
                    List<LotRecipe> listRecipe = new List<LotRecipe>();
                    listRecipe.Add(new LotRecipe() { LotInfo = _curLotInfo.LotID, Path = _curLotInfo.PgmFilePath ?? "" });
                    BindDataGrid(listRecipe);
                });
            }
        }
        catch (Exception ex)
        {
            ShowError($@"OnRecipeValidationResultCommandReceivedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }

        return Task.FromResult(model);
    }

    private Task<CIMMessageCommandModel> OnCIMMessageCommandReceivedFunc(CIMMessageCommandModel model)
    {
        try
        {
            ShowRecv($@"OnCIMMessageCommandReceivedFunc - {model.ToJson()}");
            Task.Run(() =>
            {
                ShowWarnBox(model.Body.Message, true);
            });

            _guiContext.Invoke(() =>
            {
                _listCimMsgInfo.Add(new CimMsgInfo { MsgTime = DateTime.Now.ToString(), Msg = model.Body.Message });
                BindDataGridCimMsg(_listCimMsgInfo);
            });
        }
        catch (Exception ex)
        {
            ShowError($@"OnCIMMessageCommandReceivedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
        return Task.FromResult(model);
    }

    private Task<DateTimeCommandModel> OnDateTimeCommandReceivedFunc(DateTimeCommandModel model)
    {
        try
        {
            ShowRecv($@"OnDateTimeCommandReceivedFunc - {model.ToJson()}");

            bool res = SysTimeUtil.SetSystemDateTime.SetLocalTimeByStr(model.Body.DateTime);

            model.Result.Code = res ? 1 : 0;
        }
        catch (Exception ex)
        {
            ShowError($@"OnDateTimeCommandReceivedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
        return Task.FromResult(model);
    }

    private Task<LotInfoDownloadCommandModel> OnLotInfoDownloadCommandReceivedFunc(LotInfoDownloadCommandModel model)
    {
        try
        {
            ShowRecv($@"OnLotInfoDownloadCommandReceivedFunc - {model.ToJson()}");

            _curLotInfo.ProductNo = model.Body.ProductNo;
            _curLotInfo.LotID = model.Body.LotID;
            _curLotInfo.ItemNum = model.Body.ItemNum;
            _curLotInfo.PnlWidth = model.Body.PnlWidth;
            _curLotInfo.PnlLength = model.Body.PnlLength;
            _curLotInfo.PnlThick = model.Body.PnlThick;
            _curLotInfo.PanelQTY = model.Body.PanelQTY;
            _curLotInfo.RecipeID = model.Body.RecipeID;

            if (model.Body.RecipeList.Count > 0)
            {
                Task.Run(() =>
                {
                    Invoke(() =>
                    {
                        List<string> pgmlist = new List<string>();
                        List<string> dialist = new List<string>();
                        List<string> atplist = new List<string>();
                        if (!Directory.Exists(_eAPClientOptions.RecipeSearchPath))
                        {
                            ShowErrorBox(string.Format("搜索程序文件夹路径{0}不存在".VgTs(), _eAPClientOptions.RecipeSearchPath), true);
                            return;
                        }
                        List<string> nopgmlist = new List<string>();

                        foreach (var Recipe in model.Body.RecipeList)
                        {
                            if (Recipe.Name.IsMatch(_eAPClientOptions.RecipeListDiaRegex))
                            {
                                if (_eAPClientOptions.RecipeListDiaGetType == 0)
                                {
                                    dialist.Add(Recipe.Value);
                                }
                                else
                                {
                                    string FileName = Path.GetFileName(Recipe.Value);
                                    dialist.AddRange(SearchFile(_eAPClientOptions.RecipeSearchPath, FileName, _eAPClientOptions.IsBlurSerachRecipeListDia).Select(o => o.FullName));
                                }

                            }
                            else if (Recipe.Name.IsMatch(_eAPClientOptions.RecipeListPgmRegex))
                            {
                                if (_eAPClientOptions.RecipeListPgmGetType == 0)
                                {
                                    pgmlist.Add(Recipe.Value);
                                }
                                else
                                {
                                    string FileName = Path.GetFileName(Recipe.Value);
                                    pgmlist.AddRange(SearchFile(_eAPClientOptions.RecipeSearchPath, FileName, _eAPClientOptions.IsBlurSerachRecipeListPgm).Select(o => o.FullName));
                                    nopgmlist.Add(FileName);
                                }
                            }
                            else if (Recipe.Name.IsMatch(_eAPClientOptions.RecipeListAtpRegex))
                            {
                                if (_eAPClientOptions.RecipeListAtpGetType == 0)
                                {
                                    atplist.Add(Recipe.Value);
                                }
                                else
                                {
                                    string FileName = Path.GetFileName(Recipe.Value);
                                    atplist.AddRange(SearchFile(_eAPClientOptions.RecipeSearchPath, FileName, _eAPClientOptions.IsBlurSerachRecipeListAtp).Select(o => o.FullName));
                                }
                            }
                        }

                        if (pgmlist.Count > 0)
                        {
                            string PgmFilePath = "";
                            string DiaFilePath = "";
                            string AtpFilePath = "";

                            FileLoadFrom pgmLoad = new FileLoadFrom(pgmlist.Distinct().ToList(), this, 0);
                            if (pgmLoad.ShowDialog() == DialogResult.OK)
                            {
                                PgmFilePath = pgmLoad.SelectFilePath;
                            }
                            else
                            {
                                return;
                            }

                            if (dialist.Count > 0)
                            {
                                FileLoadFrom diaLoad = new FileLoadFrom(dialist.Distinct().ToList(), this, 1);
                                if (diaLoad.ShowDialog() == DialogResult.OK)
                                {
                                    DiaFilePath = diaLoad.SelectFilePath;
                                }
                                else
                                {
                                    return;
                                }
                            }

                            if (atplist.Count > 0)
                            {
                                FileLoadFrom atpLoad = new FileLoadFrom(atplist.Distinct().ToList(), this, 2);
                                if (atpLoad.ShowDialog() == DialogResult.OK)
                                {
                                    AtpFilePath = atpLoad.SelectFilePath;
                                }
                                else
                                {
                                    return;
                                }
                            }

                            _curLotInfo.PgmFilePath = PgmFilePath;
                            if (!_drillStatusData.CurProgramData.PgmFilePath.Equals(_curLotInfo.PgmFilePath))
                            {
                                var body = new EQPCurrentChangeRecipeReportBody
                                {
                                    EquipmentID = _eAPClientOptions.EquipmentID,
                                    LotID = model.Body.LotID,
                                    ItemNum = model.Body.ItemNum,
                                    RecipeID = model.Body.RecipeID,
                                };
                                _httpDataReporter.SendEQPCurrentChangeRecipeReport(body);
                                ShowSend($@"OnLotInfoDownloadCommandReceivedFunc - SendEQPCurrentChangeRecipeReport - {body.ToJson()}");
                            }

                            CncLoadFile(PgmFilePath, DiaFilePath, AtpFilePath);
                            pgLotInfo.SelectedObject = _curLotInfo;
                        }
                        else
                        {
                            if (nopgmlist.Count > 0)
                            {
                                ShowWarnBox(string.Format("在路径{0}未找到符合的文件名[{1}]".VgTs(), _eAPClientOptions.RecipeSearchPath, string.Join("],[", nopgmlist)), true);
                            }
                        }
                    });
                });
            }

        }
        catch (Exception ex)
        {
            ShowError($@"OnLotInfoDownloadCommandReceivedFunc - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
        return Task.FromResult(model);
    }

    private async Task LogCollectionDataOnUI()
    {
        if (_eAPClientOptions.EnableLogCollectionDataOnUI)
        {
            await Task.Run(() =>
            {
                ShowResult($@"{_drillCommonDataA.LastGetTime:yyyy-MM-dd HH:mm:ss} --  {_drillCommonDataA.LastName}", "LastGetTime");
                ShowResult(_drillStatusData.ToString());
                ShowResult(_drillCommonDataA.ToString());
                ShowResult(_drillCommonDataB.ToString());
                ShowResult(_drillCommonDataC.ToString());
            });
        }
    }

    private Task OnHttpDataReporterTick()
    {
        try
        {
            var body = new EQPDataCollectionReportBody
            {
                EquipmentID = _eAPClientOptions.EquipmentID,
                TotalOnTime = _drillCommonDataA.ShiftOnlineTime,
                TotalProTime = _drillCommonDataA.ShiftWorkingTime,
                ProductNO = _curLotInfo.ProductNo,
                LotID = _curLotInfo.LotID,
                ParameterList = [
                    new Parameter()
                    {
                        Name = "Duty",
                        Value = _drillCommonDataA.Duty,
                    },
                    new Parameter()
                    {
                        Name = "ShiftOnlineTime",
                        Value = _drillCommonDataA.ShiftOnlineTime,
                    },
                    new Parameter()
                    {
                        Name = "ShiftWorkingTime",
                        Value = _drillCommonDataA.ShiftWorkingTime,
                    },
                    new Parameter()
                    {
                        Name = "ShiftWaitingTime",
                        Value = _drillCommonDataA.ShiftWaitingTime,
                    },
                    new Parameter()
                    {
                        Name = "ShiftErrorTime",
                        Value = _drillCommonDataA.ShiftErrorTime,
                    },
                    new Parameter()
                    {
                        Name = "XYPosition",
                        Value = _drillCommonDataA.XYPosition,
                    },
                    new Parameter()
                    {
                        Name = "CurDrillOrRout",
                        Value = _drillCommonDataA.CurDrillOrRout,
                    },
                    new Parameter()
                    {
                        Name = "TotalDrillOrRout",
                        Value = _drillCommonDataA.TotalDrillOrRout,
                    },
                    new Parameter()
                    {
                        Name = "CurToolId",
                        Value = _drillCommonDataA.CurToolId,
                    },
                    new Parameter()
                    {
                        Name = "CurToolD",
                        Value = _drillCommonDataA.CurToolD,
                    },
                    new Parameter()
                    {
                        Name = "CurToolS",
                        Value = _drillCommonDataA.CurToolS,
                    },
                    new Parameter()
                    {
                        Name = "CurToolF",
                        Value = _drillCommonDataA.CurToolF,
                    },
                    new Parameter()
                    {
                        Name = "CurToolChipl",
                        Value = _drillCommonDataA.CurToolChipl,
                    },
                    new Parameter()
                    {
                        Name = "CurToolR",
                        Value = _drillCommonDataA.CurToolR,
                    },
                    new Parameter()
                    {
                        Name = "CurToolA",
                        Value = _drillCommonDataA.CurToolA,
                    },
                    new Parameter()
                    {
                        Name = "CurToolZ",
                        Value = _drillCommonDataA.CurToolZ,
                    },
                    new Parameter()
                    {
                        Name = "CurToolN",
                        Value = _drillCommonDataA.CurToolN,
                    },
                    new Parameter()
                    {
                        Name = "CurToolSegM",
                        Value = _drillCommonDataA.CurToolSegM,
                    },
                    new Parameter()
                    {
                        Name = "RunTime",
                        Value = _drillCommonDataA.ShiftWorkingTime,
                    },
                    new Parameter()
                    {
                        Name = "StopTime",
                        Value = _drillCommonDataA.ShiftWaitingTime,
                    },
                    new Parameter()
                    {
                        Name = "AlermTime",
                        Value = _drillCommonDataA.ShiftErrorTime,
                    },
                    new Parameter()
                    {
                        Name = "ProgramPath",
                        Value = _drillCommonDataA.ProgramPath,
                    },
                ],
            };
            _httpDataReporter.SendEQPDataCollectionReport(body);
            ShowSend($@"OnHttpDataReporterTick - EQPDataCollectionReportBody - {body.ToJson()}");
        }
        catch (Exception ex)
        {
            ShowError($@"OnHttpDataReporterTick - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
        return Task.CompletedTask;
    }

    private Task SaveUsrShiftDuty()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrShiftDuty shift = new()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    sZ = _drillCommonDataA.DrillZ == string.Empty ? "" : (double.Parse(_drillCommonDataA.DrillZ) >= 10.0f ? "台板" : "夹PIN"),
                    dtDate = dateTime.Hour >= 8 && dateTime.Hour < 24 ? DateOnly.FromDateTime(dateTime) : DateOnly.FromDateTime(dateTime).AddDays(-1),
                    sTime = dateTime.ToString("HH:mm:ss"),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sDuty = _drillCommonDataA.Duty,
                    sT = _drillStatusData.CncStatusZs,
                    sWorkTime = _drillCommonDataA.ShiftWorkingTime,
                    sWaitTime = _drillCommonDataA.ShiftWaitingTime,
                    sStopTime = _drillCommonDataA.ShiftErrorTime,
                    sTotalTime = _drillCommonDataA.ShiftOnlineTime,
                    sHits = _drillStatusData.CurDrillOrRout,
                    sShiftHits = _iShiftHits >= _drillStatusData.CurDrillOrRout.ToInt() ? _iShiftHits.ToString() : _drillStatusData.CurDrillOrRout,
                    sTools = _drillCommonDataA.CurToolId,
                    sBrokens = _nShiftBrokenToolTimes.ToString(),
                    sChangePanels = _nChangePanelTimes.ToString(),
                    sChangeDrills = _nChangeDrillTimes.ToString(),
                    sChangeDrillsTime = "",
                    sShift = dateTime.Hour >= 8 && dateTime.Hour < 20 ? "白班" : "夜班"
                };

                _context.usrShiftDutys.Add(shift);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 更新表usrShiftDuty数据完成");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
            return Task.FromException(ex);
        }
    }

    private Task SaveUsrShiftFinalDuty()
    {
        try
        {
            string sDayShift = _CurShift;
            string sZ = _drillCommonDataA.DrillZ == string.Empty ? "" : (double.Parse(_drillCommonDataA.DrillZ) >= 10.0f ? "台板" : "夹PIN");
            string sDuty = _drillCommonDataA.Duty;
            string sT = _drillStatusData.CncStatusZs;
            string sWorkTime = _drillCommonDataA.ShiftWorkingTime;
            string sWaitTime = _drillCommonDataA.ShiftWaitingTime;
            string sStopTime = _drillCommonDataA.ShiftErrorTime;
            string sTotalTime = _drillCommonDataA.ShiftOnlineTime;
            string sHits = _drillStatusData.CurDrillOrRout;
            string sTools = _drillCommonDataA.CurToolId;
            if (sDayShift.IsNullOrEmpty() || sZ.IsNullOrEmpty() || sDuty.IsNullOrEmpty()
                || sT.IsNullOrEmpty() || sWorkTime.IsNullOrEmpty() || sWaitTime.IsNullOrEmpty()
                || sStopTime.IsNullOrEmpty() || sTotalTime.IsNullOrEmpty() || sHits.IsNullOrEmpty()
                || sTools.IsNullOrEmpty())
            {
                return Task.CompletedTask;
            }
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                var info = _context.usrShiftFinalDutys.FirstOrDefault(x => x.sEquipmentID == _eAPClientOptions.EquipmentID && x.sDayShift == sDayShift);
                if (info == null)
                {
                    info = new();
                    info.sInnerID = Guid.NewGuid().ToString();
                }
                info.sZ = _drillCommonDataA.DrillZ == string.Empty ? "" : (double.Parse(_drillCommonDataA.DrillZ) >= 10.0f ? "台板" : "夹PIN");
                info.dtDate = DateOnly.FromDateTime(dateTime);
                info.sTime = dateTime.ToString("HH:mm:ss");
                info.sEquipmentID = _eAPClientOptions.EquipmentID;
                info.sEqpType = _eAPClientOptions.EqpType;
                info.sDuty = _drillCommonDataA.Duty;
                info.sT = _drillStatusData.CncStatusZs;
                info.sWorkTime = _drillCommonDataA.ShiftWorkingTime;
                info.sWaitTime = _drillCommonDataA.ShiftWaitingTime;
                info.sStopTime = _drillCommonDataA.ShiftErrorTime;
                info.sTotalTime = _drillCommonDataA.ShiftOnlineTime;
                info.sHits = _drillStatusData.CurDrillOrRout;
                info.sShiftHits = _iShiftHits >= _drillStatusData.CurDrillOrRout.ToInt() ? _iShiftHits.ToString() : _drillStatusData.CurDrillOrRout;
                info.sTools = _drillCommonDataA.CurToolId;
                info.sBrokens = _nShiftBrokenToolTimes.ToString();
                info.sChangePanels = _nChangePanelTimes.ToString();
                info.sChangeDrills = _nChangeDrillTimes.ToString();
                info.sChangeDrillsTime = "";
                info.sShift = dateTime.Hour >= 8 && dateTime.Hour < 20 ? "白班" : "夜班";
                info.sDayShift = sDayShift;


                if (_context.Entry(info).State == EntityState.Detached)
                {
                    _context.usrShiftFinalDutys.Add(info);
                }

                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 更新表UsrShiftFinalDuty数据完成");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
            return Task.FromException(ex);
        }
    }

    private Task SavesysDrillInformation()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var info = _context.sysDrillInformations.FirstOrDefault(x => x.sEquipmentID == _eAPClientOptions.EquipmentID);
                if (info == null)
                {
                    info = new sysDrillInformation();
                    info.sInnerID = Guid.NewGuid().ToString();
                }

                info.sEquipmentID = _eAPClientOptions.EquipmentID;
                info.sEqpType = _eAPClientOptions.EqpType;
                info.sDuty = _drillCommonDataA.Duty;
                info.sWorkMode = _drillStatusData.CncStatusMo;
                info.sPersent = _drillCommonDataA.CncRunProgress;
                info.sDrilled = _drillCommonDataA.CurDrillOrRout == null ? "" : _drillCommonDataA.CurDrillOrRout;
                info.sNeeded = _drillCommonDataA.TotalDrillOrRout == null ? "" : _drillCommonDataA.TotalDrillOrRout;
                info.sActProgram = _drillCommonDataA.ProgramPath;
                info.sDiaFileName = _drillCommonDataB.DiaFilePath;
                info.sRegistrationDate = DateTime.Now.ToString("yyyy-MM-dd");
                info.sStatus = _drillStatusData.CncStatusMo;
                info.dtDateTime = DateTime.Now;


                if (_context.Entry(info).State == EntityState.Detached)
                {
                    _context.sysDrillInformations.Add(info);
                }

                _context.SaveChanges();
                _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 更新表sysDrillInformation数据完成");
            }
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
            return Task.FromException(ex);
        }
    }

    private Task SaveusrDuty()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrDuty info = new()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    dtDate = dateTime.Hour >= 8 && dateTime.Hour < 24 ? DateOnly.FromDateTime(dateTime) : DateOnly.FromDateTime(dateTime).AddDays(-1),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sDutyShiftA = _drillCommonDataA.Duty,
                    sDutyShiftB = _drillCommonDataA.Duty,
                    sDutyShiftC = ""
                };
                if (dateTime.Hour >= 8 && dateTime.Hour < 20)
                {
                    info.sDutyShiftC = "A";
                    info.sDutyShiftB = "";
                }
                else
                {
                    info.sDutyShiftC = "B";
                }

                _context.usrDutys.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 更新表usrDuty数据完成");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
            return Task.FromException(ex);
        }
    }

    private Task SaveusrRealTimeDuty()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrRealTimeDuty Real = new()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    sZ = _drillCommonDataA.DrillZ == string.Empty ? "" : (double.Parse(_drillCommonDataA.DrillZ) >= 10.0f ? "台板" : "夹PIN"),
                    dtDate = DateOnly.FromDateTime(dateTime),
                    sTime = dateTime.ToString("HH:mm:ss"),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sDuty = _drillCommonDataA.Duty,
                    sWorkTime = _drillCommonDataA.ShiftWorkingTime,
                    sWaitTime = _drillCommonDataA.ShiftWaitingTime,
                    sStopTime = _drillCommonDataA.ShiftErrorTime,
                    sTotalTime = _drillCommonDataA.ShiftOnlineTime,
                    sHits = _drillStatusData.CurDrillOrRout,
                    sRoutPath = "0",
                    sChangePanels = _nChangePanelTimes.ToString(),
                    sChangeDrills = _nChangeDrillTimes.ToString(),
                    sRegistrationDate = dateTime.ToString("yyyy-MM-dd"),
                    sShift = dateTime.Hour >= 8 && dateTime.Hour < 20 ? "A" : "B"
                };

                _context.usrRealTimeDutys.Add(Real);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 更新表usrRealTimeDuty数据完成");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
            return Task.FromException(ex);
        }
    }

    private Task Savesysloginlog()
    {
        try
        {
            DateTime dateTime = DateTime.Now;
            using (var _context = _contextFactory.CreateDbContext())
            {
                sysLoginLog info = new sysLoginLog()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sLoginTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    sAuthorizationCode = "",
                    sMemo = ""
                };

                _context.sysLoginLogs.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{dateTime.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给sysLoginLog完成");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给 sysLoginLog 时报错: {ex.Message}");
            return Task.FromException(ex);
        }
    }

    private Task Savesysstaff()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                sysStaff info = new sysStaff()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    sLoginName = _drillCommonDataC.UserName,
                    sEName = "",
                    sCName = "",
                    sPWD = "",
                    sRole = "",
                    sAddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    sAddUser = _drillCommonDataC.UserName,
                    sMemo = "",
                    iStatus = 0
                };

                _context.sysStaffs.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给sysStaffs完成");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给sysStaffs 时报错: {ex.Message}");
            return Task.FromException(ex);
        }
    }

    private Task Saveusranalysisduty()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dateTime = DateTime.Now;
                usrAnalysisDuty info = new usrAnalysisDuty()
                {
                    sInnerID = Guid.NewGuid().ToString(),
                    sEquipmentID = _eAPClientOptions.EquipmentID,
                    sEqpType = _eAPClientOptions.EqpType,
                    sZ = _drillCommonDataA.DrillZ == string.Empty ? "" : (double.Parse(_drillCommonDataA.DrillZ) >= 10.0f ? "台板" : "夹PIN"),
                    dtDate = DateOnly.FromDateTime(dateTime),
                    sTime = dateTime.ToString("HH:mm:ss"),
                    sChangePanels = _nChangePanelTimes.ToString(),
                    sStandardChangePanelsTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    sChangeDrills = _nChangeDrillTimes.ToString(),
                    sStandardChangeDrillsTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    sExceptionHandleCounts = "",
                    sExceptionHandleTime = "",
                    sMaintainTime = "",
                    sTheoryDuty = _drillCommonDataA.Duty,
                    sActualDuty = "",
                    sDifference = "",
                    sShift = dateTime.Hour >= 8 && dateTime.Hour < 20 ? "A" : "B"
                };

                _context.usrAnalysisDutys.Add(info);
                _context.SaveChanges();
            }
            _logger.LogDebug($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} 上传数据给usrAnalysisDuty完成");
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), $"上传数据给usrAnalysisDuty 时报错: {ex.Message}");
            return Task.FromException(ex);
        }
    }

    #endregion

    #region 自定义方法

    /// <summary>
    /// 部分界面文字翻译
    /// </summary>
    private void PageTran()
    {
        uiSwPM.ActiveText = "开始保养".VgTs();
        uiSwPM.InActiveText = "保养结束".VgTs();

        autoSwitch.ActiveText = "自动模式".VgTs();
        autoSwitch.InActiveText = "手动模式".VgTs();

        cimSwitch.ActiveText = "已连接".VgTs();
        cimSwitch.InActiveText = "断开连接".VgTs();

        cnc84Swith.ActiveText = "已连接".VgTs();
        cnc84Swith.InActiveText = "断开连接".VgTs();

        cnc95Switch.ActiveText = "已连接".VgTs();
        cnc95Switch.InActiveText = "断开连接".VgTs();

        opcuaSwitch.ActiveText = "已连接".VgTs();
        opcuaSwitch.InActiveText = "断开连接".VgTs();
    }

    private string GetCompileVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? System.Version.Parse("1.0.0.0").ToString();
    }

    /// <summary>
    /// 筛选获取设备状态
    /// </summary>
    private string GetMO(string val)
    {
        try
        {
            if (_eAPClientOptions.IsPM)
            {
                return "4";
            }
            switch (val)
            {
                case "IDLE":
                case "WAIT":
                case "SERV":
                    return "1";//IDLE
                case "WORK":
                    return "2";//RUN
                case "DOWN":
                case "STOP":
                case "ALARM":
                case "ALAM":
                    return "3";//STOP /DOWN
            }
        }
        catch (Exception ex)
        {
            ShowError($@"GetMO - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
        return val;
    }

    private void BindSettings()
    {
        pgSettings.SelectedObject = _settingsInfo;
        pgSettings.Enabled = false;
        btnSave.Enabled = false;
        btnUpdate.Enabled = true;
    }

    private void BindDataGridCimMsg(List<CimMsgInfo> listCimMsg)
    {
        if (listCimMsg.Count > 20)
        {
            listCimMsg.RemoveRange(0, listCimMsg.Count - 20);
        }

        //dataGrid = new DataGridView();
        //dataGrid.Rows.Clear();
        dataGridCimMsg.Columns.Clear();
        dataGridCimMsg.DataSource = null;
        //关闭自动创建列
        dataGridCimMsg.AutoGenerateColumns = false;
        dataGridCimMsg.AutoSize = true;

        //取消最后一行空白列
        dataGridCimMsg.AllowUserToAddRows = false;
        // 列头隐藏
        //dataGrid.ColumnHeadersVisible = false;
        // 行头隐藏
        //dataGrid.RowHeadersVisible = false;
        // 禁止用户改变DataGridView1的所有列的列宽
        //dataGrid.AllowUserToResizeColumns = false;
        //禁止用户改变DataGridView1の所有行的行高
        //dataGrid.AllowUserToResizeRows = false;
        // Initialize and add a text box column.

        DataGridViewColumn tagCode = new DataGridViewTextBoxColumn();
        tagCode.DataPropertyName = "MsgTime";
        tagCode.Name = "MsgTime";
        tagCode.Width = 120;
        tagCode.HeaderText = "MsgTime";
        dataGridCimMsg.Columns.Add(tagCode);

        /*DataGridViewButtonColumn Operate = new DataGridViewButtonColumn();
        Operate.Name = "Operate";
        Operate.Width = 60;
        Operate.HeaderText = "操作";
        Operate.UseColumnTextForButtonValue = true;
        Operate.Text = "加载";
        dataGrid.Columns.Add(Operate);*/

        DataGridViewColumn tagPath = new DataGridViewTextBoxColumn();
        tagPath.DataPropertyName = "Msg";
        tagPath.Name = "Msg";
        tagPath.Width = 595;
        tagPath.HeaderText = "Msg";
        dataGridCimMsg.Columns.Add(tagPath);
        //禁止改变DataGridView的行高与列宽
        dataGridCimMsg.AllowUserToResizeRows = false;
        dataGridCimMsg.AllowUserToResizeColumns = true;

        dataGridCimMsg.DataSource = listCimMsg;
        dataGridCimMsg.Tag = listCimMsg;
        dataGridCimMsg.ReadOnly = true;
    }

    private void BindDataGrid(List<LotRecipe> listData)
    {
        //dataGrid = new DataGridView();
        //dataGrid.Rows.Clear();
        dataGrid.Columns.Clear();
        dataGrid.DataSource = null;
        //关闭自动创建列
        dataGrid.AutoGenerateColumns = false;
        dataGrid.AutoSize = true;

        //取消最后一行空白列
        dataGrid.AllowUserToAddRows = false;
        // 列头隐藏
        //dataGrid.ColumnHeadersVisible = false;
        // 行头隐藏
        //dataGrid.RowHeadersVisible = false;
        // 禁止用户改变DataGridView1的所有列的列宽
        //dataGrid.AllowUserToResizeColumns = false;
        //禁止用户改变DataGridView1の所有行的行高
        //dataGrid.AllowUserToResizeRows = false;
        // Initialize and add a text box column.

        DataGridViewColumn tagCode = new DataGridViewTextBoxColumn();
        tagCode.DataPropertyName = "LotInfo";
        tagCode.Name = "LotInfo";
        tagCode.Width = 90;
        tagCode.HeaderText = "LotInfo";
        dataGrid.Columns.Add(tagCode);

        /*DataGridViewButtonColumn Operate = new DataGridViewButtonColumn();
        Operate.Name = "Operate";
        Operate.Width = 60;
        Operate.HeaderText = "操作";
        Operate.UseColumnTextForButtonValue = true;
        Operate.Text = "加载";
        dataGrid.Columns.Add(Operate);*/

        DataGridViewColumn tagPath = new DataGridViewTextBoxColumn();
        tagPath.DataPropertyName = "Path";
        tagPath.Name = "Path";
        tagPath.Width = 595;
        tagPath.HeaderText = "详细路径".VgTs();
        dataGrid.Columns.Add(tagPath);
        //禁止改变DataGridView的行高与列宽
        dataGrid.AllowUserToResizeRows = false;
        dataGrid.AllowUserToResizeColumns = true;

        dataGrid.DataSource = listData;
        dataGrid.Tag = listData;
        dataGrid.ReadOnly = true;
    }

    private void BindGridViewWorkOrderRecipe(DataGridView datagvPgm, DataGridView datagvPara, DataGridView datagvAtp, bool IsAddCheckBox)
    {
        try
        {
            BindGridViewList(datagvAtp, _filelistAtp, IsAddCheckBox);
            BindGridViewList(datagvPara, _filelistDia, IsAddCheckBox);
            BindGridViewList(datagvPgm, _filelistPgm, IsAddCheckBox);
        }
        catch (Exception ex)
        {
            ShowError($@"BindGridViewWorkOrderRecipe - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void ClearGridViewList(DataGridView datagvPgm, DataGridView datagvPara, DataGridView datagvAtp)
    {
        try
        {
            datagvPgm.Columns.Clear();
            datagvPgm.DataSource = null;
            datagvPara.Columns.Clear();
            datagvPara.DataSource = null;
            datagvAtp.Columns.Clear();
            datagvAtp.DataSource = null;
        }
        catch (Exception ex)
        {
            ShowError($@"ClearGridViewList - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void ClearUpdateFileList()
    {
        try
        {
            _guiContext.Invoke(() =>
            {
                ClearGridViewList(datagvLocalPgmFilePath, datagvLocalParaFilePath, datagvLocalAtpFilePath);
            });

            _filelistPgm.Clear();
            _filelistDia.Clear();
            _filelistAtp.Clear();
        }
        catch (Exception ex)
        {
            ShowError($@"ClearUpdateFileList - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void BindGridViewList(DataGridView dataGridView, List<WorkOrderRecipe> fileList, bool IsAddCheckBox)
    {
        try
        {
            if (fileList.Count < 1)
            {
                return;
            }

            //dataGrid = new DataGridView();
            //dataGrid.Rows.Clear();
            dataGridView.Columns.Clear();
            dataGridView.DataSource = null;
            //关闭自动创建列
            dataGridView.AutoGenerateColumns = false;
            dataGridView.AutoSize = true;

            //取消最后一行空白列
            dataGridView.AllowUserToAddRows = false;
            // 列头隐藏
            //dataGrid.ColumnHeadersVisible = false;
            // 行头隐藏
            //dataGrid.RowHeadersVisible = false;
            // 禁止用户改变DataGridView1的所有列的列宽
            //dataGrid.AllowUserToResizeColumns = false;
            //禁止用户改变DataGridView1の所有行的行高
            //dataGrid.AllowUserToResizeRows = false;
            // Initialize and add a text box column.

            if (IsAddCheckBox)
            {
                DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                dataGridViewCheckBoxColumn.Name = "IsLoadFile";
                dataGridViewCheckBoxColumn.Width = 60;
                dataGridViewCheckBoxColumn.HeaderText = "加载".VgTs();
                dataGridViewCheckBoxColumn.ReadOnly = false;
                dataGridViewCheckBoxColumn.TrueValue = true;
                dataGridViewCheckBoxColumn.FalseValue = false;
                dataGridView.Columns.Add(dataGridViewCheckBoxColumn);
            }

            DataGridViewColumn tagCode = new DataGridViewTextBoxColumn();
            tagCode.DataPropertyName = "tagCode";
            tagCode.Name = "tagCode";
            tagCode.Width = 90;
            tagCode.HeaderText = "工单".VgTs();
            dataGridView.Columns.Add(tagCode);

            DataGridViewColumn tagType = new DataGridViewTextBoxColumn();
            tagType.DataPropertyName = "tagType";
            tagType.Name = "tagType";
            tagType.Width = 90;
            tagType.HeaderText = "类型".VgTs();
            dataGridView.Columns.Add(tagType);

            DataGridViewColumn tagPath = new DataGridViewTextBoxColumn();
            tagPath.DataPropertyName = "tagPath";
            tagPath.Name = "tagPath";
            tagPath.Width = 595;
            tagPath.HeaderText = "详细路径".VgTs();
            dataGridView.Columns.Add(tagPath);
            //禁止改变DataGridView的行高与列宽
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.AllowUserToResizeColumns = true;

            dataGridView.DataSource = fileList;
            dataGridView.Tag = fileList;

            if (IsAddCheckBox)
            {
                dataGridView.ReadOnly = false;
                dataGridView.Columns[1].ReadOnly = true;
                dataGridView.Columns[2].ReadOnly = true;
                dataGridView.Columns[3].ReadOnly = true;
                dataGridView.Columns[4].ReadOnly = true;
            }
            else
            {
                dataGridView.ReadOnly = true;
            }
        }
        catch (Exception ex)
        {
            ShowError($@"BindGridViewList - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void CncLoadFile(string PgmFilePath, string DiaFilePath = "", string AtpFilePath = "")
    {
        if (!_eAPClientOptions.IsAlarmLoadFile)
        {
            if ((_drillStatusData.CncStatusMo.Equals("ALAM") || _cNCConnector.EcList.Contains(_drillStatusData.CncStatusEc)))
            {
                ShowWarnBox($@"已禁止在机器报警状态下加载文件".VgTs(), true);
                return;
            }
        }

        if (_eAPClientOptions.IsCollaborative && _eAPClientOptions.OthEquipmentIDs.Count > 0)
        {
            string filestrmsg = $@"加工程序:".VgTs() + $"{PgmFilePath}";
            if (!DiaFilePath.IsNullOrEmpty())
            {
                filestrmsg += $@" " + "直径文件:".VgTs() + $"{DiaFilePath}";
            }
            if (!AtpFilePath.IsNullOrEmpty())
            {
                filestrmsg += $@" " + "刀盘文件:".VgTs() + $"{AtpFilePath}";
            }

            List<string> oths = _eAPClientOptions.OthEquipmentIDs.Where(o => o != _eAPClientOptions.EquipmentID).Distinct().ToList();
            if (oths.Count > 0)
            {
                var dig = MessageBox.Show(string.Format("是否将 {0} 同时加载进机台[{1}]".VgTs(), filestrmsg, string.Join(",", oths)), "", MessageBoxButtons.YesNo);
                if (dig == DialogResult.Yes)
                {
                    foreach (string sEquipmentID in oths)
                    {
                        SaveusrEquLoadFileLog(
                            sEquipmentID: sEquipmentID,
                            PgmFilePath: PgmFilePath,
                            DiaFilePath: DiaFilePath,
                            AtpFilePath: AtpFilePath,
                            sEquipmentIDSrc: _eAPClientOptions.EquipmentID,
                            sOpType: "CncLoadFile"
                        );
                    }
                }
            }
        }
        SaveusrEquLoadFileLog(
            sEquipmentID: _eAPClientOptions.EquipmentID,
            PgmFilePath: PgmFilePath,
            DiaFilePath: DiaFilePath,
            AtpFilePath: AtpFilePath,
            sEquipmentIDSrc: _eAPClientOptions.EquipmentID,
            sOpType: "CncLoadFile",
            iLoadStatus: 1
        );
        _drillStatusData.CncLoadFile(PgmFilePath, DiaFilePath, AtpFilePath);
    }

    /// <summary>
    /// 数据库插入程式切换记录
    /// </summary>
    /// <param name="sEquipmentID">机台编码</param>
    /// <param name="PgmFilePath">加工程序</param>
    /// <param name="DiaFilePath">直径文件</param>
    /// <param name="AtpFilePath">刀盘文件</param>
    /// <param name="sEquipmentIDSrc">来源机台编码</param>
    /// <param name="sOpType">操作类型(CncLoadFile;CncStart;CncStop)</param>
    /// <param name="iLoadStatus">加载状态(0=未加载;1=已加载)</param>
    private void SaveusrEquLoadFileLog(string sEquipmentID, string PgmFilePath, string DiaFilePath, string AtpFilePath, string sEquipmentIDSrc, string sOpType, int iLoadStatus = 0)
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime dbtime = _context.GetDbTime();

                var infor = _context.sysDrillInformations.FirstOrDefault(o => o.sEquipmentID == sEquipmentID);
                if (infor != null)
                {
                    var log = new usrEquLoadFileLog();

                    log.sInnerID = Guid.NewGuid().ToString();
                    log.sEquipmentID = sEquipmentID;
                    log.sDiaFile = DiaFilePath;
                    log.sAtpFile = AtpFilePath;
                    log.sProgramFile = PgmFilePath;
                    log.iLoadStatus = iLoadStatus;
                    log.dtTime = dbtime;
                    log.sEquipmentIDSrc = sEquipmentIDSrc;
                    log.sOpType = sOpType;

                    _context.usrEquLoadFileLogs.Add(log);

                    _context.SaveChanges();
                    _logger.LogDebug($"{dbtime.ToString("yyyy-MM-dd HH:mm:ss")} 新增表usrEquLoadFileLog数据完成");
                }
                else
                {
                    _logger.LogDebug($"{dbtime.ToString("yyyy-MM-dd HH:mm:ss")} 新增表usrEquLoadFileLog数据完成,机台编码[{sEquipmentID}]不存在");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
        }
    }

    private void GetLocalWebIpEndPoint()
    {
        try
        {
            if (_receiverOptions.Enabled)
            {
                string sPort = _receiverOptions.UrlPrefix.Split(':').Last().Trim('/');
                int iPort = sPort.ToInt();
                if (iPort <= 0)
                {
                    iPort = 5001;
                }

                try
                {
                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                    {
                        socket.Connect("8.8.8.8", iPort);
                        IPEndPoint? iPEnd = socket.LocalEndPoint as IPEndPoint;
                        if (iPEnd != null)
                        {
                            _eAPClientOptions.iPLocal = iPEnd;
                            _eAPClientOptions.iPLocal.Port = iPort;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    string hostName = Dns.GetHostName();
                    IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                    string IPv4 = addresses.FirstOrDefault(o => o.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "";
                    if (IPv4.IsNullOrEmpty())
                    {
                        _eAPClientOptions.iPLocal = new IPEndPoint(IPAddress.Any, iPort);
                    }
                    else
                    {
                        _eAPClientOptions.iPLocal = new IPEndPoint(IPAddress.Parse(IPv4), iPort);
                    }
                }
                ShowResult(string.Format("HTTP监听已开启,IP:{0},Port:{1}".VgTs(), _eAPClientOptions.iPLocal.Address, _eAPClientOptions.iPLocal.Port));
            }
            else
            {
                _logger.LogInformation("HTTP监听未开启".VgTs());
                _eAPClientOptions.iPLocal = new IPEndPoint(IPAddress.Any, 5001);
            }
        }
        catch (Exception ex)
        {
            ShowError($@"GetLocalWebIpEndPoint - {ex.Message}");
            _logger.LogError(ex, $@"GetLocalWebIpEndPoint - {ex.Message}");
        }
    }

    private List<FileInfo> SearchFile(string DirPath, string FileName, bool IsBlur = false)
    {
        List<FileInfo> fileInfos = new List<FileInfo>();
        if (DirPath == _eAPClientOptions.RecipeSearchPath && _eAPClientOptions.RecipeSearchPathTimeFresh > 0)
        {
            if (IsBlur)
            {
                fileInfos = _SearchPathList.Where(o => o.Name.Contains(FileName) && o.Name.IsMatch(_eAPClientOptions.RecipeSearchNameRegex)).ToList();
            }
            else
            {
                fileInfos = _SearchPathList.Where(o => o.Name == FileName).ToList();
            }
        }
        else
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
            if (directoryInfo.Exists)
            {
                if (IsBlur)
                {
                    fileInfos = directoryInfo.GetFiles($@"*{FileName}*", SearchOption.AllDirectories).ToList();
                    fileInfos = fileInfos.Where(o => o.Name.IsMatch(_eAPClientOptions.RecipeSearchNameRegex)).ToList();
                }
                else
                {
                    fileInfos = directoryInfo.GetFiles(FileName, SearchOption.AllDirectories).ToList();
                }

            }
        }


        return fileInfos;
    }

    /// <summary>
    /// 更新班次
    /// </summary>
    /// <returns></returns>
    private Task UpdateShiftData()
    {
        try
        {
            if (!_isFrmMainLoaded)
            {
                return Task.CompletedTask;
            }
            DateTime dateTime = DateTime.Now;
            _CurShift = GetShift(dateTime);
            if (_CurShift != _LastShift)
            {
                _LastShift = _CurShift;
                _nChangePanelTimes = 0;
                _nChangeDrillTimes = 0;
                _nShiftBrokenToolTimes = 0;
                _iShiftHits = 0;
                ShowResult(string.Format("班次切换完成,当前班次:{0}".VgTs(), _CurShift));
            }
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            ShowError($@"UpdateShiftData - {ex.Message}");
            _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message);
            return Task.FromException(ex);
        }
    }

    private Task ammeterIOPopImportantLog()
    {
        try
        {
            if (null != _ammeterIO)
            {
                string line = _ammeterIO.PopImportantLog();
                if (false == string.IsNullOrEmpty(line))
                {
                    ShowResult($@"OnTimerForRefresh - {line}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"ammeterIOPopImportantLog - {ex.Message}");
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取班次
    /// </summary>
    /// <param name="dateTime">日期时间</param>
    /// <returns>日期_D=白班;日期_N=夜班</returns>
    private string GetShift(DateTime dateTime)
    {
        if (dateTime.Hour >= 8 && dateTime.Hour < 20)
        {
            return $@"{dateTime.ToString("yyyy-MM-dd")}_D";
        }
        else if (dateTime.Hour >= 20)
        {
            return $@"{dateTime.ToString("yyyy-MM-dd")}_N";
        }
        else
        {
            return $@"{dateTime.AddDays(-1).ToString("yyyy-MM-dd")}_N";
        }
    }

    #endregion

    #region Form Load Close
    private void frmScanner_Load(object sender, EventArgs e)
    {
        if (_eAPClientOptions.IsLogin)
        {
            frmLogin frmLogin = _formProvider.GetForm<frmLogin>();
            if (frmLogin != null
                && frmLogin.ShowDialog() != DialogResult.OK)
            {
                Close();
                Environment.Exit(0);
                return;
            }
        }


        Text = _eAPClientOptions.MainFormTitle + " Ver" + GetCompileVersion() + "--" + "苏州维嘉科技股份有限公司".VgTs();
        ShowResult(Text, "启动".VgTs());

        if (_eAPClientOptions.OperationMode == "2")//自动模式
        {
            foreach (object? item in tabPage1.Controls)
            {
                ((Control)item).Enabled = false;
            }
        }

        if (_eAPClientOptions.IsPM)
        {
            uiSwPM.Active = true;
        }
        else
        {
            uiSwPM.Active = false;
        }

        if (_eAPClientOptions.CIMMode == "1")
        {
            cimSwitch.Active = true;
        }
        else
        {
            cimSwitch.Active = false;
        }

        if (_eAPClientOptions.OperationMode == "1")
        {
            autoSwitch.Active = true;
        }
        else
        {
            autoSwitch.Active = false;
        }

        BindSettings();

        if (_eAPClientOptions.EnableAutoStartApp)
        {
            _setuoWindowOpenRun.SetupStartupLnk(Application.ExecutablePath, "维嘉DNC", "维嘉数控", false);
        }

        checkIsDirectLoad.Checked = _eAPClientOptions.IsDirectLoadFile;

        if (_eAPClientOptions.EnableAmmeter)
        {
            _ammeterIO = _ammeterFactory.GetAmmeterIO(_eAPClientOptions.AmmeterModel);
            if (null != _ammeterIO)
            {
                _logger.LogDebug($"Get ammeter : ok. model '{_eAPClientOptions.AmmeterModel}'.");

                var the_logger = _loggerFactory.CreateLogger(_eAPClientOptions.AmmeterModel);
                _ammeterIO.StartRun(_eAPClientOptions);
            }
            else
            {
                _logger.LogDebug($"Get ammeter : failed. model '{_eAPClientOptions.AmmeterModel}'.");
            }
        }

        if (_aOIDbOptions.IsEnable)
        {
            if (_eAPClientOptions.DeviceDescriptor.DeviceKind == DeviceKind.CNC84Drill)
            {
                label6.Text = "AOI";
            }
            else
            {
                label7.Text = "AOI";
            }
        }

        _isFrmMainLoaded = true;

        if (_cNCConnector.IsConnected)
        {
            if (_eAPClientOptions.DeviceDescriptor.DeviceKind == DeviceKind.CNC84Drill)
            {
                if (!cnc84Swith.Active)
                {
                    OnCNCConnected();
                }
            }
            else
            {
                if (!cnc95Switch.Active)
                {
                    OnCNCConnected();
                }
            }
        }
    }

    private void frmScanner_FormClosing(object sender, FormClosingEventArgs e)
    {
        e.Cancel = true;
        this.WindowState = FormWindowState.Minimized;
    }

    private void OnFormClosed(object? sender, FormClosedEventArgs e)
    {
        if (null != _ammeterIO)
        {
            _ammeterIO.StopRun();
        }

        if (cts != null)
        {
            cts.Cancel();
        }
    }

    #endregion

    #region ForRefresh

    private void OnTimerForRefresh(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                UpdateShiftData();
                ammeterIOPopImportantLog();
            }
            catch
            {

            }

            Thread.Sleep(1000);
        }
    }

    #endregion ForRefresh

    #region GetSearchPathList
    private Task GetSearchPathList()
    {
        while (true)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(_eAPClientOptions.RecipeSearchPath);
                if (directoryInfo.Exists)
                {
                    _SearchPathList = directoryInfo.GetFiles("*", SearchOption.AllDirectories).ToList();
                    ShowResult($"目录[{_eAPClientOptions.RecipeSearchPath}]文件列表已更新:[{_SearchPathList.Count}]", "UpdateRecipeSearchFloderFileList");
                }
            }
            catch (Exception ex)
            {
                ShowError($@"GetSearchPathList - {ex.Message}");
                _logger.LogError(ex, $@"GetSearchPathList - {ex.Message}");
            }
            Thread.Sleep(_eAPClientOptions.RecipeSearchPathTimeFresh * 1000);
        }
    }
    #endregion

    #region ShowResult

    /// <summary>
    /// 界面显示日志信息
    /// </summary>
    /// <param name="msg">Detail</param>
    /// <param name="messageName">消息的名称</param>
    /// <param name="events">PostStatus、GetArrage、PostBroken、PostATP</param>
    private void ShowResult(string? msg, string? events = "")
    {
        ShowResultBox(msg, events);
    }

    private void ShowWarn(string msg)
    {
        ShowResult(msg, "Warn");
    }

    private void ShowError(string msg)
    {
        ShowResult(msg, "Error");
    }

    private void ShowRecv(string msg)
    {
        ShowResult(msg, "Recv");
    }

    private void ShowSend(string msg)
    {
        ShowResult(msg, "Send");
    }

    /// <summary>
    /// 界面显示日志信息,弹窗提示
    /// </summary>
    /// <param name="msg">Detail</param>
    /// <param name="events">PostStatus、GetArrage、PostBroken、PostATP</param>
    /// <param name="IsShow">是否弹窗</param>
    public void ShowResultBox(string? msg, string? events = "", bool IsShow = false)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(msg))
                return;

            Action action = () =>
            {
                int i = dgvlog.Rows.Count + 1;
                if (i > 999)
                {
                    dgvlog.Rows.Clear();
                    i = 1;
                }
                string title = "VegaInfo";
                MessageBoxIcon icon = MessageBoxIcon.None;
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvlog);
                row.Cells[0].Value = i.ToStringEx();
                row.Cells[1].Value = DateTime.Now.ToString("HH:mm:ss");
                row.Cells[2].Value = events;
                row.Cells[3].Value = msg;
                if (events != null)
                {
                    if (events.ToLower() == "error")
                    {
                        title = "VegaError";
                        row.DefaultCellStyle.BackColor = Color.Red;
                        icon = MessageBoxIcon.Error;
                    }
                    if (events.ToLower() == "warn")
                    {
                        title = "VegaWarn";
                        row.DefaultCellStyle.BackColor = Color.Chocolate;
                        icon = MessageBoxIcon.Warning;
                    }
                }
                dgvlog.Rows.Insert(0, row);
                dgvlog.Update();
                _logger.LogInformation(msg);
                if (IsShow)
                {
                    MessageBox.Show(msg, title, MessageBoxButtons.OK, icon);
                }
            };
            Invoke(action);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public void ShowWarnBox(string msg, bool IsShow = false)
    {
        ShowResultBox(msg, "Warn", IsShow);
    }

    public void ShowErrorBox(string msg, bool IsShow = false)
    {
        ShowResultBox(msg, "Error", IsShow);
    }

    public void ShowRecvBox(string msg, bool IsShow = false)
    {
        ShowResultBox(msg, "Recv", IsShow);
    }

    public void ShowSendBox(string msg, bool IsShow = false)
    {
        ShowResultBox(msg, "Send", IsShow);
    }

    #endregion ShowResult

    #region contextMenu

    private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
    {
        this.StartPosition = FormStartPosition.CenterScreen;
        this.WindowState = FormWindowState.Normal;
        this.BringToFront();
        this.Activate();
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        DialogResult dr = MessageBox.Show(string.Format("是否确认退出【{0}】程序？".VgTs(), _eAPClientOptions.MainFormTitle), "重要提示".VgTs(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
        switch (dr)
        {
            case DialogResult.Yes:
                {
                    notifyIcon.Visible = false;
                    Environment.Exit(0);
                }
                break;

            default:
                break;
        }
    }

    private void notifyIcon_DoubleClick(object sender, EventArgs e)
    {
        this.contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
    }

    #endregion contextMenu

    #region ReaderDbLoadFileLog

    private void ReaderDbLoadFileLogFun()
    {
        while (true)
        {
            try
            {
                ReaderDbLoadFileLog();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $@"ReaderDbLoadFileLogFun - {ex.Message}");
            }
            Task.Delay(500);
        }
    }

    private void ReaderDbLoadFileLog()
    {
        try
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                DateTime nowtime = _context.GetDbTime();
                var logs = _context.usrEquLoadFileLogs.Where(o => o.sEquipmentID == _eAPClientOptions.EquipmentID && o.iLoadStatus == 0).OrderBy(o => o.dtTime);
                foreach (var log in logs)
                {
                    Action action = new Action(() => { });
                    if (log.sOpType == "CncLoadFile")
                    {
                        action = new Action(() =>
                        {
                            _drillStatusData.CncLoadFile(log.sProgramFile, log.sDiaFile, log.sAtpFile);
                            ShowResult(string.Format("已从数据库中读取并加载程式：{0},内码：{1}".VgTs(), log.sProgramFile, log.sInnerID));
                        });
                    }
                    else if (log.sOpType == "CncStart")
                    {
                        action = new Action(() =>
                        {
                            _cNCConnector.RetrieveData("CncStart");
                            ShowResult(string.Format("已从数据库中读取开始运行程式,内码：{0}".VgTs(), log.sInnerID));
                        });
                    }
                    else if (log.sOpType == "CncStop")
                    {
                        action = new Action(() =>
                        {
                            _cNCConnector.RetrieveData("CncStop");
                            ShowResult(string.Format("已从数据库中读取停止运行程式,内码：{0}".VgTs(), log.sInnerID));
                        });
                    }
                    else
                    {
                        action = new Action(() =>
                        {
                            ShowResult(string.Format("无法解析的类型：{0},内码：{1}".VgTs(), log.sOpType, log.sInnerID));
                        });
                    }

                    if (nowtime.Subtract(log.dtTime).TotalSeconds <= 60)
                    {
                        log.iLoadStatus = 1;
                        Task.Run(action);
                        break;
                    }
                    else
                    {
                        log.iLoadStatus = 2;
                        ShowResult(string.Format("已从数据库中读取并废弃类型：{0},内码：{1}".VgTs(), log.sOpType, log.sInnerID));
                    }
                }
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $@"ReaderDbLoadFileLog - {ex.Message}");
        }
    }

    #endregion

    private void uiSwPM_ValueChanged(object sender, bool value)
    {
        if (uiSwPM.Active)
        {
            _eAPClientOptions.IsPM = true;
            Task.Run(() =>
            {
                var body = new EQPStatusChangeReportBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                    Status = "4",
                };
                _httpDataReporter.SendEQPStatusChangeReport(body);
                ShowSend($@"uiSwPM_ValueChanged - SendEQPStatusChangeReport - {body.ToJson()}");
            });
        }
        else
        {
            _eAPClientOptions.IsPM = false;
            Task.Run(() =>
            {
                var body = new EQPStatusChangeReportBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                    Status = GetMO(_drillStatusData.CncStatusMo),
                };
                _httpDataReporter.SendEQPStatusChangeReport(body);
                ShowSend($@"uiSwPM_ValueChanged - SendEQPStatusChangeReport - {body.ToJson()}");
            });
        }
    }

    private void txtWorkOrder_KeyDown(object sender, KeyEventArgs e)
    {
        try
        {
            if (e.KeyCode == Keys.Enter)
            {
                _curLotInfo.LotID = txtWorkOrder.Text.Trim();

                Task.Run(() =>
                {
                    var body = new LotInfoRequestBody
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        LotID = txtWorkOrder.Text.Trim(),
                        ItemNum = _curLotInfo.ItemNum,
                    };
                    _httpDataReporter.SendLotInfoRequest(body);
                    ShowSend($@"txtWorkOrder_KeyDown - SendLotInfoRequest - {body.ToJson()}");
                });

                if (_eAPClientOptions.AutoLoadCNCFile)
                {
                    //_cncFileLoader.LoadFile("E:\\1.1TestToolPara\\Mes01.atp", "E:\\1.1TestToolPara\\84-2.DIA");
                    _cncFileLoader.LoadFile(txtWorkOrder.Text);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void autoSwitch_ValueChanged(object sender, bool value)
    {
        try
        {
            if (value)
            {
                _eAPClientOptions.OperationMode = "1"; //Auto
                Task.Run(() =>
                {
                    var body = new EQPRunningModeReportBody
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        Status = _eAPClientOptions.OperationMode,
                    };
                    _httpDataReporter.SendEQPRunningModeReport(body);
                    ShowSend($@"autoSwitch_ValueChanged(ON) - SendEQPRunningModeReport - {body.ToJson()}");
                });
            }
            else
            {
                _eAPClientOptions.OperationMode = "2"; //Manual
                Task.Run(() =>
                {
                    var body = new EQPRunningModeReportBody
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        Status = _eAPClientOptions.OperationMode,
                    };
                    _httpDataReporter.SendEQPRunningModeReport(body);
                    ShowSend($@"autoSwitch_ValueChanged(OFF) - SendEQPRunningModeReport - {body.ToJson()}");
                });
            }
        }
        catch (Exception ex)
        {
            ShowError($@"autoSwitch_ValueChanged - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void btnDateTimeSync_Click(object sender, EventArgs e)
    {
        try
        {
            var body = new EQPDateTimeRequestBody
            {
                EquipmentID = _eAPClientOptions.EquipmentID,
                DateTime = "",
            };

            _httpDataReporter.SendEQPDateTimeRequest(body);
            ShowSend($@"btnDateTimeSync_Click - SendEQPDateTimeRequest - {body.ToJson()}");
        }
        catch (Exception ex)
        {
            ShowError($@"btnDateTimeSync_Click - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void cimSwitch_ValueChanged(object sender, bool value)
    {
        try
        {
            if (value)
            {
                _eAPClientOptions.CIMMode = "1"; //在线
                Task.Run(() =>
                {
                    var eQPCommunicationStatusReportBody = new EQPCommunicationStatusReportBody
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        CommunicationStatus = _eAPClientOptions.CIMMode,
                    };
                    _httpDataReporter.SendEQPCommunicationStatusReport(eQPCommunicationStatusReportBody);
                    ShowSend($@"cimSwitch_ValueChanged(ON) - SendEQPCommunicationStatusReport - {eQPCommunicationStatusReportBody.ToJson()}");

                    var EQPDateTimeRequestBody = new EQPDateTimeRequestBody
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        DateTime = "",
                    };
                    _httpDataReporter.SendEQPDateTimeRequest(EQPDateTimeRequestBody);
                    ShowSend($@"cimSwitch_ValueChanged(ON) - SendEQPDateTimeRequest - {EQPDateTimeRequestBody.ToJson()}");
                }
                );
            }
            else
            {
                _eAPClientOptions.CIMMode = "2"; //离线
                Task.Run(() =>
                {
                    var eQPCommunicationStatusReportBody = new EQPCommunicationStatusReportBody
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID,
                        CommunicationStatus = _eAPClientOptions.CIMMode,
                    };
                    _httpDataReporter.SendEQPCommunicationStatusReport(eQPCommunicationStatusReportBody);
                    ShowSend($@"cimSwitch_ValueChanged(OFF) - SendEQPCommunicationStatusReport - {eQPCommunicationStatusReportBody.ToJson()}");
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void btnLotCompleted_Click(object sender, EventArgs e)
    {
        try
        {
            Task.Run(() =>
            {
                var body = new EQPCompletedReportBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                    LotID = _curLotInfo.LotID,
                    ItemNum = _curLotInfo.ItemNum,
                    PanelQTY = numCompleted.Value.ToString(),
                };
                _httpDataReporter.SendEQPCompletedReport(body);
                ShowSend($@"btnLotCompleted_Click - SendEQPCompletedReport - {body.ToJson()}");
            });
        }
        catch (Exception ex)
        {
            ShowError($@"btnLotCompleted_Click - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
        pgSettings.Enabled = true;
        btnUpdate.Enabled = false;
        btnSave.Enabled = true;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string file = Path.Combine(".\\appsettings.json");
            string replacedJson = _configurationFileReplacer.Replace(File.ReadAllText(file, System.Text.Encoding.UTF8), new Dictionary<string, string?>
            {
                ["EAPClientOptions:EquipmentID"] = _eAPClientOptions.EquipmentID.ToString(),
                ["HttpDataReporterOptions:DataCollectionReportSeconds"] = _reporterOptions.DataCollectionReportSeconds.ToString(),
                ["EAPClientOptions:RecipeSearchPath"] = _eAPClientOptions.RecipeSearchPath.ToString(),
                ["EAPClientOptions:DiaSearchSuffix"] = _eAPClientOptions.DiaSearchSuffix.ToString(),
            });

            replacedJson = _configurationFileReplacer.Replace(replacedJson, new Dictionary<string, string[]>
            {
                ["EAPClientOptions:MsgBoxBrokenToolInfoSet:BrokenLengthItems"] = _eAPClientOptions.MsgBoxBrokenToolInfoSet.BrokenLengthItems.ToArray(),
                ["EAPClientOptions:MsgBoxBrokenToolInfoSet:BrokenReasonItems"] = _eAPClientOptions.MsgBoxBrokenToolInfoSet.BrokenReasonItems.ToArray(),
            });

            replacedJson = replacedJson.Replace(@"\u002B", "+");

            File.WriteAllText(file, replacedJson);

            DialogResult dr = MessageBox.Show($"参数已经保存,需要重启软件。".VgTs() + "\r\n\r\n" + "【是】将会重启本系统".VgTs(), "重要提示".VgTs(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
            switch (dr)
            {
                case DialogResult.Yes:
                    {
                        notifyIcon.Visible = false;
                        Application.Restart();
                        Environment.Exit(0);
                    }
                    break;

                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void txtLocalWorkOrder_KeyDown(object sender, KeyEventArgs e)
    {
        Task.Run(async () =>
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    ClearUpdateFileList();
                    _isDirectLoadedFile = false;

                    var fileList = await _workOrderRecipeLoader.LoadRecipe(new WorkOrderRecipeRequest
                    {
                        EquipmentId = _eAPClientOptions.EquipmentID.ToString(),
                        ItemCode = txtLocalWorkOrder.Text.Trim()
                    });

                    if (fileList.Count < 1)
                    {
                        ShowErrorBox($"{_eAPClientOptions.RecipeSearchPath}" + "\r\n" + "路径下没有相关文件，请排查".VgTs(), true);
                        return;
                    }

                    foreach (WorkOrderRecipe workOrderRecipe in fileList)
                    {
                        if (workOrderRecipe.tagType == LoadFileType.PROGRAM)
                        {
                            _filelistPgm.Add(workOrderRecipe);
                        }
                        else if (workOrderRecipe.tagType == LoadFileType.DIA)
                        {
                            _filelistDia.Add(workOrderRecipe);
                        }
                        else
                        {
                            _filelistAtp.Add(workOrderRecipe);
                        }
                    }

                    _logger.LogInformation($"_filelistPgm : ");
                    _filelistPgm.ForEach(workRecipe => _logger.LogInformation(workRecipe.ToString()));
                    _logger.LogInformation($"_filelistDia : ");
                    _filelistDia.ForEach(workRecipe => _logger.LogInformation(workRecipe.ToString()));
                    _logger.LogInformation($"_filelistAtp : ");
                    _filelistAtp.ForEach(workRecipe => _logger.LogInformation(workRecipe.ToString()));

                    if (checkIsDirectLoad.Checked)
                    {
                        string strLoadDrl = string.Empty;
                        string strLoadDia = string.Empty;
                        string strLoadAtp = string.Empty;

                        if (_filelistPgm.Count <= 1 && _filelistDia.Count <= 1 && _filelistAtp.Count <= 1)
                        {
                            if (_filelistPgm.Count == 1)
                            {
                                strLoadDrl = _filelistPgm.First().tagPath;
                            }

                            if (_filelistDia.Count == 1)
                            {
                                strLoadDia = _filelistDia.First().tagPath;
                            }

                            if (_filelistAtp.Count == 1)
                            {
                                strLoadAtp = _filelistAtp.First().tagPath;
                            }

                            CncLoadFile(strLoadDrl, strLoadDia, strLoadAtp);
                            _isDirectLoadedFile = true;

                            ShowResult("txtLocalWorkOrder_KeyDown - " + "直接加载文件".VgTs());
                        }
                        else
                        {
                            _isDirectLoadedFile = false;
                        }
                    }

                    _guiContext.Invoke(() =>
                    {
                        BindGridViewWorkOrderRecipe(datagvLocalPgmFilePath, datagvLocalParaFilePath, datagvLocalAtpFilePath, !_isDirectLoadedFile);
                    });
                }
            }
            catch (Exception ex)
            {
                ShowError($@"txtLocalWorkOrder_KeyDown - {ex.Message}");
                _logger.LogError(ex, ex.Message);
            }
        });
    }

    private void opcuaSwitch_ValueChanged(object sender, bool value)
    {
        if (value)
        {
            txtOpcuaServer.Text = _eAPClientOptions.OpcUaServer;
            txtOpcuaServer.ReadOnly = true;
        }
        else
        {
            txtOpcuaServer.Text = string.Empty;
            txtOpcuaServer.ReadOnly = true;
        }
    }

    private void btnLoadFile_Click(object sender, EventArgs e)
    {
        try
        {
            string strLoadDrl = string.Empty;
            string strLoadDia = string.Empty;
            string strLoadAtp = string.Empty;

            for (int i = 0; i < datagvLocalAtpFilePath.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell dgvCheck = (DataGridViewCheckBoxCell)(datagvLocalAtpFilePath.Rows[i].Cells[0]);
                if (dgvCheck != null && Convert.ToBoolean(dgvCheck.Value))
                {
                    strLoadAtp = datagvLocalAtpFilePath.Rows[i].Cells["tagPath"].Value.ToString() ?? "";
                }
            }

            for (int i = 0; i < datagvLocalParaFilePath.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell dgvCheck = (DataGridViewCheckBoxCell)(datagvLocalParaFilePath.Rows[i].Cells[0]);
                if (dgvCheck != null && Convert.ToBoolean(dgvCheck.Value))
                {
                    strLoadDia = datagvLocalParaFilePath.Rows[i].Cells["tagPath"].Value.ToString() ?? "";
                }
            }

            for (int i = 0; i < datagvLocalPgmFilePath.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell dgvCheck = (DataGridViewCheckBoxCell)(datagvLocalPgmFilePath.Rows[i].Cells[0]);
                if (dgvCheck != null && Convert.ToBoolean(dgvCheck.Value))
                {
                    strLoadDrl = datagvLocalPgmFilePath.Rows[i].Cells["tagPath"].Value.ToString() ?? "";
                }
            }

            ShowResult($"btnLoadFile_Click - " + "点击加载文件".VgTs() + $" - {strLoadAtp} - {strLoadDia} - {strLoadDrl}");
            Task.Run(() => CncLoadFile(strLoadDrl, strLoadDia, strLoadAtp));
        }
        catch (Exception ex)
        {
            ShowError($@"btnLoadFile_Click - {ex.Message}");
            _logger.LogError(ex, ex.Message);
        }
    }

    private void btnCncStart_Click(object sender, EventArgs e)
    {
        if (_eAPClientOptions.IsCollaborative && _eAPClientOptions.OthEquipmentIDs.Count > 0)
        {
            List<string> oths = _eAPClientOptions.OthEquipmentIDs.Where(o => o != _eAPClientOptions.EquipmentID).Distinct().ToList();
            if (oths.Count > 0)
            {
                var dig = MessageBox.Show(string.Format("是否同时开始运行程序机台[{0}]".VgTs(), string.Join(",", oths)), "", MessageBoxButtons.YesNo);
                if (dig == DialogResult.Yes)
                {
                    foreach (string sEquipmentID in oths)
                    {
                        SaveusrEquLoadFileLog(
                            sEquipmentID: sEquipmentID,
                            PgmFilePath: "",
                            DiaFilePath: "",
                            AtpFilePath: "",
                            sEquipmentIDSrc: _eAPClientOptions.EquipmentID,
                            sOpType: "CncStart"
                        );
                    }
                }
            }
        }
        SaveusrEquLoadFileLog(
            sEquipmentID: _eAPClientOptions.EquipmentID,
            PgmFilePath: "",
            DiaFilePath: "",
            AtpFilePath: "",
            sEquipmentIDSrc: _eAPClientOptions.EquipmentID,
            sOpType: "CncStart",
            iLoadStatus: 1
        );
        Task.Run(() =>
        {
            _cNCConnector.RetrieveData("CncStart");
        });
    }

    private void btnCncStop_Click(object sender, EventArgs e)
    {
        if (_eAPClientOptions.IsCollaborative && _eAPClientOptions.OthEquipmentIDs.Count > 0)
        {
            List<string> oths = _eAPClientOptions.OthEquipmentIDs.Where(o => o != _eAPClientOptions.EquipmentID).Distinct().ToList();
            if (oths.Count > 0)
            {
                var dig = MessageBox.Show(string.Format("是否同时停止运行程序机台[{0}]".VgTs(), string.Join(",", oths)), "", MessageBoxButtons.YesNo);
                if (dig == DialogResult.Yes)
                {
                    foreach (string sEquipmentID in oths)
                    {
                        SaveusrEquLoadFileLog(
                            sEquipmentID: sEquipmentID,
                            PgmFilePath: "",
                            DiaFilePath: "",
                            AtpFilePath: "",
                            sEquipmentIDSrc: _eAPClientOptions.EquipmentID,
                            sOpType: "CncStop"
                        );
                    }
                }
            }
        }
        SaveusrEquLoadFileLog(
            sEquipmentID: _eAPClientOptions.EquipmentID,
            PgmFilePath: "",
            DiaFilePath: "",
            AtpFilePath: "",
            sEquipmentIDSrc: _eAPClientOptions.EquipmentID,
            sOpType: "CncStop",
            iLoadStatus: 1
        );
        Task.Run(() =>
        {
            _cNCConnector.RetrieveData("CncStop");
        });
    }


}
