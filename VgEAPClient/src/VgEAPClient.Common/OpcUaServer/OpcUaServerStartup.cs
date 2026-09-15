// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Opc.Ua;
using Opc.Ua.Configuration;
using Quartz.Util;
using SharpNodeSettings.OpcUaServer;
using VgEAPClient.Common.CNC;
using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.Status;
using VgEAPClient.Common.CNC.ToolMeasurement;

namespace VgEAPClient.Common.OpcUaServer;

internal class OpcUaServerStartup : IOpcUaServerStartup
{
    public event Action? OnStartup;

    public event Action<Exception>? OnStartupFailed;

    private readonly ILogger<OpcUaServerStartup> _logger;
    private readonly EAPClientOptions _eAPClientOptions;
    private readonly DrillStatusData _drillStatusData;
    private readonly DrillCommonDataA _drillCommonDataA;
    private readonly DrillCommonDataB _drillCommonDataB;
    private readonly DrillCommonDataC _drillCommonDataC;
    private readonly DrillToolMeasureData _drillToolMeasureData;
    public SharpNodeSettingsServer _sharpNodeSettingsServer { get; set; }
    private bool _isOpcUaServerStarted = false;
    private bool _isLastAlam = false;

    public List<NodeDescription> extradrilList { get; set; } = new List<NodeDescription>();
    public string NotUseEqpName { get; set; } = string.Empty;

    private string EquipmentID { get; set; } = string.Empty;
    public OpcUaServerStartup(ILogger<OpcUaServerStartup> logger,
        IOptions<EAPClientOptions> options,
        IDataCollector<DrillStatusData> statusDataCollector,
        IDataCollector<DrillCommonDataA> commonDataCollectorA,
        IDataCollector<DrillCommonDataB> commonDataCollectorB,
        IDataCollector<DrillCommonDataC> commonDataCollectorC,
        IDataCollector<DrillToolMeasureData> toolMeasureData)
    {
        _logger = logger;
        _eAPClientOptions = options.Value;
        _sharpNodeSettingsServer = new SharpNodeSettingsServer(_eAPClientOptions);

        _drillCommonDataA = commonDataCollectorA.Data;
        _drillCommonDataB = commonDataCollectorB.Data;
        _drillCommonDataC = commonDataCollectorC.Data;

        _drillCommonDataA.PropertyChanged += OnCommonDataAPropertyChanged;
        _drillCommonDataB.PropertyChanged += OnCommonDataBPropertyChanged;
        _drillCommonDataC.PropertyChanged += OnCommonDataCPropertyChanged;

        _drillStatusData = statusDataCollector.Data;

        _drillStatusData.OnBlockTextChanged += OnStatusDataBlockTextChanged;
        _drillStatusData.OnBrokenToolChanged += OnStatusDataBrokenToolChanged;
        _drillStatusData.OnCncStatusMoChanged += OnStatusDataCncStatusMoChanged;
        _drillStatusData.OnCncProgramStartEvent += OnStatusDataCncProgramStartEvent;
        _drillStatusData.OnCncProgramFinishedEvent += OnStatusDataCncProgramFinishedEvent;
        _drillStatusData.OnCncLoadFileFinished += OnStatusDataCncLoadFileFinished;

        _drillToolMeasureData = toolMeasureData.Data;
        _drillToolMeasureData.DiameterTol.PropertyChanged += OnDiameterTolPropertyChanged;
        _drillToolMeasureData.LengthTol.PropertyChanged += OnLengthTolPropertyChanged;
        _drillToolMeasureData.RunoutTol.PropertyChanged += OnRunoutTolPropertyChanged;

        foreach (var measureInfo in _drillCommonDataC.ListSpindleMeasureInfo)
        {
            measureInfo.PropertyChanged += OnMeasureInfoPropertyChanged;
        }
    }

    public async Task Start()
    {
        ApplicationInstance application = new ApplicationInstance();
        application.ApplicationType = ApplicationType.Server;
        application.ConfigSectionName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OpcUaServer", "VgOpcUaServer");
        try
        {
            _sharpNodeSettingsServer.extradrilList.AddRange(extradrilList);
            _sharpNodeSettingsServer.NotUseEqpName = NotUseEqpName;
            if (NotUseEqpName.IsNullOrWhiteSpace())
            {
                EquipmentID = _eAPClientOptions.EquipmentID;
            }
            else
            {
                EquipmentID = NotUseEqpName;
            }
            // load the application configuration.
            await application.LoadApplicationConfiguration(false);

            if (!string.IsNullOrEmpty(_eAPClientOptions.OpcUaServer)) // opc.tcp://127.0.0.1:62541
            {
                application.ApplicationConfiguration.ServerConfiguration.BaseAddresses =
                [
                    _eAPClientOptions.OpcUaServer
                ];
                if (_eAPClientOptions.OpcUaServer.Contains("127.0.0.1"))
                {
                    string hostName = Dns.GetHostName();
                    var addresses = Dns.GetHostAddresses(hostName).Where(o => o.AddressFamily == AddressFamily.InterNetwork);
                    foreach (var address in addresses)
                    {
                        string IPv4 = address.ToString();
                        if (IPv4 != "127.0.0.1")
                        {
                            string opctcpurl = _eAPClientOptions.OpcUaServer.Replace("127.0.0.1", IPv4);
                            application.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Add(opctcpurl);
                        }

                    }
                }
            }

            await Task.Run(() =>
            {
                // check the application certificate.
                // 有任何问题，请先删除 C:\ProgramData\OPC Foundation\CertificateStores
                try
                {
                    bool certOk = application.CheckApplicationInstanceCertificate(false, 0, 1200).Result;
                    if (!certOk)
                    {
                        _logger.LogError("OPCUA证书错误，请删除 C:\\ProgramData\\OPC Foundation\\CertificateStores 后重试");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "OPCUA证书错误，请删除 C:\\ProgramData\\OPC Foundation\\CertificateStores 后重试");
                }
            });

            // start the server.
            await application.Start(_sharpNodeSettingsServer);
            _logger.LogInformation("本地opcua已启动");
            // 出现 请求的操作无法完成。此计算机必须为委派而被信任，并且当前用户帐户必须配置以允许委派 错误
            // 找到注册表路径 HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography\Protect\Providers\df9d8cd0-1501-11d1-8c7a-00c04fc297eb
            // 新建 DWORD(32位)值  名称ProtectionPolicy  数值1

            _isOpcUaServerStarted = true;
            OnStartup?.Invoke();

            StartWatchWMode();
        }
        catch (Exception ex)
        {
            OnStartupFailed?.Invoke(ex);
            _logger.LogError(ex, $@"本地opcua启动错误,{ex.Message}");
        }
    }

    private void OnStatusDataCncProgramFinishedEvent()
    {
        try
        {
            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/PgmRunEndTime", DateTime.Now.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnStatusDataCncProgramStartEvent()
    {
        try
        {
            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/PgmRunStartTime", DateTime.Now.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnStatusDataCncStatusMoChanged()
    {
        try
        {
            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/Status", _drillStatusData.CncStatusMo);
            if (_drillStatusData.CncStatusMo == "ALAM")
            {
                _isLastAlam = true;
                SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ExceptCode", _drillStatusData.CncStatusEc);
                SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ExceptMessage", _drillStatusData.BlockText);
                SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ExceptStartTime", DateTime.Now.ToString());
            }
            else if (_isLastAlam)
            {
                _isLastAlam = false;
                SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ExceptEndTime", DateTime.Now.ToString());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnStatusDataBrokenToolChanged()
    {
        try
        {
            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/BrokenToolId", _drillStatusData.BrokenToolData.BrkToolId);
            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/BrokenDia", _drillStatusData.BrokenToolData.BrkToolDia);
            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/BrokenSpindleId", _drillStatusData.BrokenToolData.BrkToolSpindle);
            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/BrokenInfo", _drillStatusData.BrokenToolData.BrokenInfo);
            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/BrokenTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnStatusDataBlockTextChanged()
    {
        try
        {
            _logger.LogInformation("BlockTextChanged - " + _drillStatusData.BlockText);

            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ScreenText", _drillStatusData.BlockText);

            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/CurrentTime", DateTime.Now.ToString());

            SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/PgmRunTotalTime", _drillStatusData.PgmRunTotalTime);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCommonDataAPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            switch (e.PropertyName)
            {
                case "Duty":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/Duty", _drillCommonDataA.Duty);
                    break;

                case "ShiftOnlineTime":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ShiftOnlineTime", _drillCommonDataA.ShiftOnlineTime);
                    break;

                case "ShiftWorkingTime":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ShiftWorkingTime", _drillCommonDataA.ShiftWorkingTime);
                    break;

                case "ShiftWaitingTime":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ShiftWaitingTime", _drillCommonDataA.ShiftWaitingTime);
                    break;

                case "ShiftErrorTime":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ShiftErrorTime", _drillCommonDataA.ShiftErrorTime);
                    break;

                case "ProgramPath":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/CurPgmFilePath", _drillCommonDataA.ProgramPath);
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/ProgramName", Path.GetFileName(_drillCommonDataA.ProgramPath));
                    break;

                case "XYPosition":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/XYPosition", _drillCommonDataA.XYPosition);
                    break;

                case "CurDrillOrRout":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/CurDrillOrRout", _drillCommonDataA.CurDrillOrRout);
                    break;

                case "TotalDrillOrRout":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TotalDrillOrRout", _drillCommonDataA.TotalDrillOrRout);
                    break;

                case "CurSpindleStatus":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/SpindleStatus", _drillCommonDataA.CurSpindleStatus);
                    break;

                case "CurToolId":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TT", _drillCommonDataA.CurToolId);
                    break;

                case "CurToolD":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TD", _drillCommonDataA.CurToolD);
                    break;

                case "CurToolS":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TS", _drillCommonDataA.CurToolS);
                    break;

                case "CurToolF":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TF", _drillCommonDataA.CurToolF);
                    break;

                case "CurToolR":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TR", _drillCommonDataA.CurToolR);
                    break;

                case "CurToolN":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TN", _drillCommonDataA.CurToolN);
                    break;

                case "CurToolB":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TB", _drillCommonDataA.CurToolB);
                    break;

                case "CncRunProgress":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/CncRunProgress", _drillCommonDataA.CncRunProgress);
                    break;

                case "Block":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/Block", _drillCommonDataA.Block);
                    break;

                case "Step":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/Step", _drillCommonDataA.Step);
                    break;

                case "DrillZ":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/Z", _drillCommonDataA.DrillZ);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCommonDataBPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            switch (e.PropertyName)
            {
                case "DiaFilePath":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/CurDiaFilePath", _drillCommonDataB.DiaFilePath);
                    break;

                case "AtpFilePath":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/CurAtpFilePath", _drillCommonDataB.AtpFilePath);
                    break;

                case "DrillH":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/H", _drillCommonDataB.DrillH);
                    break;

                case "DrillQ":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/Q", _drillCommonDataB.DrillQ);
                    break;

                case "DrillK":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/K", _drillCommonDataB.DrillK);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnMeasureInfoPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            if (sender != null)
            {
                SpindleMeasureInfo spindleMeasureInfo = (SpindleMeasureInfo)sender;

                switch (e.PropertyName)
                {
                    case "SpindleEnable":
                        SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/SpindleEnable{spindleMeasureInfo.SpindleId}", spindleMeasureInfo.SpindleEnable);
                        break;

                    case "TMeasureDia":
                        SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TMeasureDia{spindleMeasureInfo.SpindleId}", spindleMeasureInfo.TMeasureDia);
                        break;

                    case "TMeasureLen":
                        SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TMeasureLen{spindleMeasureInfo.SpindleId}", spindleMeasureInfo.TMeasureLen);
                        break;

                    case "TRunout":
                        SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/TRunout{spindleMeasureInfo.SpindleId}", spindleMeasureInfo.TRunout);
                        break;

                    case "SpindleWorkTimes":
                        SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/SpindleWorkTimes{spindleMeasureInfo.SpindleId}", spindleMeasureInfo.SpindleWorkTimes);
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCommonDataCPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            switch (e.PropertyName)
            {
                case "PreDuty":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/PreDuty", _drillCommonDataC.PreDuty);
                    break;

                case "OPID":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/OPID", _drillCommonDataC.OPID);
                    break;

                case "SAX":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/SAX", _drillCommonDataC.SAX);
                    break;

                case "SAY":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/SAY", _drillCommonDataC.SAY);
                    break;

                case "SAZX":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/SAZX", _drillCommonDataC.SAZX);
                    break;

                case "SAZY":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/SAZY", _drillCommonDataC.SAZY);
                    break;

                case "UserName":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/UserName", _drillCommonDataC.UserName);
                    break;

                case "UserLevel":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/UserLevel", _drillCommonDataC.UserLevel);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnRunoutTolPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            switch (e.PropertyName)
            {
                case "Checked":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/RunoutTolChecked", _drillToolMeasureData.RunoutTol.Checked);
                    break;

                case "NegValue":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/RunoutTolNeg", _drillToolMeasureData.RunoutTol.NegValue);
                    break;

                case "PosValue":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/RunoutTolPos", _drillToolMeasureData.RunoutTol.PosValue);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnLengthTolPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            switch (e.PropertyName)
            {
                case "Checked":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/LengthTolChecked", _drillToolMeasureData.LengthTol.Checked);
                    break;

                case "NegValue":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/LengthTolNeg", _drillToolMeasureData.LengthTol.NegValue);
                    break;

                case "PosValue":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/LengthTolPos", _drillToolMeasureData.LengthTol.PosValue);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnDiameterTolPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            switch (e.PropertyName)
            {
                case "Checked":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/DiameterTolChecked", _drillToolMeasureData.DiameterTol.Checked);
                    break;

                case "NegValue":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/DiameterTolNeg", _drillToolMeasureData.DiameterTol.NegValue);
                    break;

                case "PosValue":
                    SetOpcUaServerNodeData($"ns=2;s=Vega Drill Machines/{EquipmentID}/DiameterTolPos", _drillToolMeasureData.DiameterTol.PosValue);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void SetOpcUaServerNodeData(string key, object value)
    {
        try
        {
            if (_isOpcUaServerStarted)
            {
                _sharpNodeSettingsServer.ChangeNodeValue(key, value);
            }
            else
            {
                _logger.LogError("SetOpcUaServerNodeData Failed, OpcUaServer is not started.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private System.Timers.Timer? WModeTimer;

    private void StartWatchWMode()
    {
        WModeTimer = new System.Timers.Timer();
        WModeTimer.AutoReset = true;
        WModeTimer.Interval = 1000;
        WModeTimer.Elapsed += WModeTimer_Elapsed;
        WModeTimer.Start();
    }

    private void WModeTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        try
        {
            var WMode = _sharpNodeSettingsServer.getNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WMode");
            if (WMode == null)
                return;

            if (2 == Convert.ToInt32(WMode.ToString()))
            {
                //直接重置节点，防止重复加载
                _sharpNodeSettingsServer.ChangeNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WMode", 0);

                string atpFileLocation = _sharpNodeSettingsServer.getNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WAtpFileLocation").ToString() ?? "";
                string diaFileLocation = _sharpNodeSettingsServer.getNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WDiaFileLocation").ToString() ?? "";
                string pgmFileLocation = _sharpNodeSettingsServer.getNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WPgmFileLocation").ToString() ?? "";
                if (string.IsNullOrWhiteSpace(pgmFileLocation))
                {
                    pgmFileLocation = _sharpNodeSettingsServer.getNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WProgramFileLocation").ToString() ?? "";
                }

                Task.Run(() => _drillStatusData.CncLoadFile(pgmFileLocation, diaFileLocation, atpFileLocation));

                _logger.LogInformation($"WModeTimer_Elapsed - {WMode} {atpFileLocation} {diaFileLocation} {pgmFileLocation}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnStatusDataCncLoadFileFinished()
    {
        try
        {
            string strLoadResult = _drillStatusData.CncLoadFileResult.GetAllLoadResult();
            if (strLoadResult.Contains("加载失败"))
            {
                Task.Run(() =>
                {
                    _sharpNodeSettingsServer.ChangeNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WMode", 4);
                    _sharpNodeSettingsServer.ChangeNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WResult", strLoadResult);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    _sharpNodeSettingsServer.ChangeNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WMode", 3);
                    _sharpNodeSettingsServer.ChangeNodeValue($"ns=2;s=Vega Drill Machines/{EquipmentID}/WResult", strLoadResult);
                });
            }

            _logger.LogInformation($"OnStatusDataCncLoadFileFinished - {strLoadResult}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
