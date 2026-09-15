// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Infrastructure;
using VgEAPClient.Common.AOI;
using VgEAPClient.Common.CNC.Common;
using VgEAPClient.Common.CNC.Status;

namespace VgEAPClient.Common.CNC;
public class AOIDbConnector : ICNCConnector, IDisposable
{
    public bool IsConnected
    {
        get => _isConnected;
        private set => _isConnected = value;
    }

    public List<string> EcList { get; set; } = [];
    public DrillCommonDataA _drillCommonDataA { get; set; }
    public DrillStatusData _drillStatusData { get; set; }

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

    //private readonly DrillStatusData _drillStatusData;
    //private readonly DrillCommonDataA _drillCommonDataA;

    private volatile bool _isConnected = false;
    private readonly ILogger<AOIDbConnector> _logger;
    private readonly IAsyncTaskWaiter _asyncTaskWaiter;
    private readonly EAPClientOptions _eAPClientOptions;
    private readonly AOIDbOptions _aOIDbOptions;
    private readonly IDbContextFactory<VgAOIContext> _contextFactory;

    private BDCheckTable? _lastbdcheck = null;
    public AOIDbConnector(ILogger<AOIDbConnector> logger,
        IOptions<EAPClientOptions> options,
        IOptions<AOIDbOptions> aoioptions,
        IDbContextFactory<VgAOIContext> contextFactory,
        //IDataCollector<DrillCommonDataA> commonDataCollectorA,
        //IDataCollector<DrillStatusData> statusDataCollector,
        IObjectFactory objectFactory,
        IAsyncTaskWaiter asyncTaskWaiter)
    {
        _logger = logger;
        _asyncTaskWaiter = asyncTaskWaiter;
        _eAPClientOptions = options.Value;
        _aOIDbOptions = aoioptions.Value;
        _contextFactory = contextFactory;
        //_drillStatusData = statusDataCollector.Data;
        //_drillCommonDataA = commonDataCollectorA.Data;
        _drillStatusData = objectFactory.CreateObject<DrillStatusData>();
        _drillCommonDataA = objectFactory.CreateObject<DrillCommonDataA>();
    }


    public async Task<bool> CncLoadFile(LoadFileType loadFileType, string strFilePath, CancellationToken cancellationToken = default)
    {
        return true;
    }

    public async Task<bool> CncSetXYOfProgramZero(double x, double y, CancellationToken cancellationToken = default)
    {
        return true;
    }

    public async Task Connect(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug($"连接AOI : {_aOIDbOptions.ConnectionString}");

            using (var context = _contextFactory.CreateDbContext())
            {
                _isConnected = context.Database.CanConnect();
            }
            if (_isConnected)
            {
                _logger.LogDebug("连接AOI成功！ Database : " + _aOIDbOptions.ConnectionString);
                OnCNCConnected?.Invoke();
            }
            else
            {
                _logger.LogError("连接AOI失败！ Database : " + _aOIDbOptions.ConnectionString);
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

    public async Task Disonnect(CancellationToken cancellationToken)
    {

    }

    public void Dispose()
    {

    }

    public async Task<RetMsg> LoadCNCCommand(string cmd)
    {
        return new RetMsg();
    }

    public BrokenToolData ParseBrokenToolData(BrokenToolData brokenToolData, string blockText, CancellationToken cancellationToken = default)
    {
        return brokenToolData;
    }

    public async Task<string> RetrieveData(string key, CancellationToken cancellationToken = default)
    {
        string restr = string.Empty;
        try
        {
            switch (key)
            {
                case "CncStatus":
                case "Duty":
                case "ShiftWorkingTime":
                case "ShiftWaitingTime":
                case "ShiftErrorTime":
                case "CncError":
                    {
                        int curHour = DateTime.Now.Hour;
                        int iclass = 0;
                        if (curHour >= 8 && curHour < 20)
                        {
                            iclass = 0;
                        }
                        else
                        {
                            iclass = 1;
                        }

                        BDStatusTable? bdstatus = null;
                        using (var context = _contextFactory.CreateDbContext())
                        {
                            bdstatus = context.BDStatusTables.Where(o => o.Class == iclass).FirstOrDefault();
                            if (bdstatus == null)
                            {
                                bdstatus = context.BDStatusTables.FirstOrDefault();
                            }
                        }
                        if (bdstatus != null)
                        {
                            if (key == "CncStatus")
                            {
                                string mo = "";
                                switch (bdstatus.iReserver1)
                                {
                                    case 2:
                                        mo = "WAIT";
                                        break;
                                    case 3:
                                        mo = "ALARM";
                                        break;
                                    case 4:
                                        mo = "SERV";
                                        break;
                                    case 5:
                                        mo = "STOP";
                                        break;
                                    default:
                                        mo = "IDLE";
                                        break;
                                }
                                restr = $@"MO{mo}";
                            }
                            if (key == "Duty")
                            {
                                restr = (bdstatus.fReserver1 * 100).ToStringEx();
                            }
                            if (key == "ShiftWorkingTime")
                            {
                                restr = DateTime.Now.Date.AddSeconds(bdstatus.ProductionTime ?? 0).ToString("HH:mm:ss");
                            }
                            if (key == "ShiftWaitingTime")
                            {
                                restr = DateTime.Now.Date.AddSeconds(bdstatus.StandbyTime ?? 0).ToString("HH:mm:ss");
                            }
                            if (key == "ShiftErrorTime")
                            {
                                restr = DateTime.Now.Date.AddSeconds(bdstatus.FailureTime ?? 0).ToString("HH:mm:ss");
                            }
                            if (key == "CncError")
                            {
                                restr = bdstatus.czReserver1 ?? "";
                            }
                        }
                        else
                        {
                            if (key == "CncStatus")
                            {
                                restr = $@"MOIDLE";
                            }
                            if (key == "Duty")
                            {
                                restr = "0";
                            }
                            if (key == "ShiftWorkingTime" || key == "ShiftWaitingTime" || key == "ShiftErrorTime")
                            {
                                restr = DateTime.Now.Date.ToString("HH:mm:ss");
                            }
                        }
                    }
                    break;
                case "PgmRunStartTime":
                case "PgmRunEndTime":
                case "PgmFilePath":
                case "TotalDrillOrRout":
                case "DrillZ":
                    {
                        if (_lastbdcheck != null)
                        {
                            if (DateTime.TryParseExact(_lastbdcheck.ulBeginCheckTime, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtBeginCheckTime))
                            {
                                if (key == "PgmRunStartTime")
                                {
                                    restr = dtBeginCheckTime.ToString("yyyy/MM/dd HH:mm:ss");
                                }
                                else if (key == "PgmRunEndTime")
                                {
                                    restr = dtBeginCheckTime.AddSeconds(_lastbdcheck.iCheckUseTime ?? 0).ToString("yyyy/MM/dd HH:mm:ss");
                                }
                                else if (key == "PgmFilePath")
                                {
                                    restr = _lastbdcheck.czDrillName ?? string.Empty;
                                }
                                else if (key == "TotalDrillOrRout")
                                {
                                    restr = _lastbdcheck.iCheckHoles.ToStringEx();
                                }
                                else if (key == "DrillZ")
                                {
                                    restr = _lastbdcheck.nReserver7.ToStringEx();
                                }
                            }
                        }
                    }
                    break;
                case "GetAOIBDCheck":
                    {
                        try
                        {
                            DateTime nottime = DateTime.Now;
                            string ulBeginCheckTimeBegin = nottime.AddMilliseconds(-2).ToString("yyyy-MM-dd-HH-mm-ss");
                            if (_aOIDbOptions.HisDay > 0)
                            {
                                ulBeginCheckTimeBegin = nottime.AddDays(-_aOIDbOptions.HisDay).ToString("yyyy-MM-dd-HH-mm-ss");
                            }
                            using (var context = _contextFactory.CreateDbContext())
                            {
                                string sql = $@"SELECT * FROM BDCheckTable WHERE ulBeginCheckTime >= '{ulBeginCheckTimeBegin}' AND iSendFlag = 0 ORDER BY ulBeginCheckTime LIMIT 1";
                                var bdcheck = context.BDCheckTables.FromSqlRaw(sql).FirstOrDefault();
                                if (bdcheck != null)
                                {
                                    _drillStatusData.IsStopGetRunTime = true;
                                    _lastbdcheck = bdcheck;
                                    bdcheck.iSendFlag = 1;
                                    context.SaveChanges();
                                }
                                else
                                {
                                    if (_lastbdcheck == null)
                                    {
                                        _drillStatusData.IsCanSendPgmEnd = false;
                                        sql = $@"SELECT * FROM BDCheckTable ORDER BY ulBeginCheckTime DESC LIMIT 1";
                                        _lastbdcheck = context.BDCheckTables.FromSqlRaw(sql).FirstOrDefault();
                                    }
                                }
                            }
                            if (_lastbdcheck != null)
                            {
                                _drillStatusData.sResult = _lastbdcheck.nReserver8 == 2 ? "OK" : "NG";
                                _drillStatusData.sCpk = _lastbdcheck.czReserver4 ?? "";
                                _drillStatusData.CurProgramData.PgmFilePath = _lastbdcheck.czDrillName ?? string.Empty;
                                _drillStatusData.sBarCode = _lastbdcheck.czQBarCodeInfo ?? string.Empty;
                                _drillStatusData.sImgUrl = _lastbdcheck.czReserver ?? "";
                                _drillCommonDataA.ProgramPath = _lastbdcheck.czDrillName ?? string.Empty;
                                _drillCommonDataA.TotalDrillOrRout = _lastbdcheck.iCheckHoles.ToStringEx();
                                _drillCommonDataA.DrillZ = _lastbdcheck.nReserver7.ToStringEx();


                                if (DateTime.TryParseExact(_lastbdcheck.ulBeginCheckTime, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtBeginCheckTime))
                                {
                                    _drillStatusData.PgmRunTotalTime = _lastbdcheck.iCheckUseTime.ToStringEx();
                                    _drillStatusData.PgmRunStartTime = dtBeginCheckTime.ToString("yyyy/MM/dd HH:mm:ss");
                                    _drillStatusData.PgmRunEndTime = dtBeginCheckTime.AddSeconds(_lastbdcheck.iCheckUseTime ?? 0).ToString("yyyy/MM/dd HH:mm:ss");
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $@"RetrieveData - {ex.Message}");
                        }
                        finally
                        {
                            _drillStatusData.IsStopGetRunTime = false;
                            _drillStatusData.IsCanSendPgmEnd = true;
                        }

                    }
                    break;

            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $@"RetrieveData - {ex.Message}");
        }
        return restr;
    }
}

public class VgAOIContext : DbContext
{
    public DbSet<BDCheckTable> BDCheckTables { get; set; }
    public DbSet<BDStatusTable> BDStatusTables { get; set; }
    public DbSet<HoleCheckTable> HoleCheckTables { get; set; }

    private readonly EAPClientOptions _eapClientOptions;
    private readonly AOIDbOptions _aOIDbOptions;

    public VgAOIContext(DbContextOptions<VgAOIContext> options,
        IOptions<EAPClientOptions> eapClientOptions,
        IOptions<AOIDbOptions> aOIDbOptions) : base(options)
    {
        _eapClientOptions = eapClientOptions.Value;
        _aOIDbOptions = aOIDbOptions.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging(true);
        var SqliteConnectionString = _aOIDbOptions.ConnectionString; //"Filename=E:\\vega.sqlite"; 
        optionsBuilder.UseSqlite(SqliteConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
    }
}



