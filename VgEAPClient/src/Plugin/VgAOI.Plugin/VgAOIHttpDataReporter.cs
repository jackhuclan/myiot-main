using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAOI.Plugin.Models;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;
using VgEAPClient.Common;
using VgEAPClient.Common.Communication;
using VgEAPClient.Common.Communication.Outbound;

namespace VgAOI.Plugin;

public class VgAOIHttpDataReporter : IEQPDataReporter, IDisposable
{
    private readonly ILogger<VgAOIHttpDataReporter> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IObjectFactory _objectFactory;
    private readonly HttpDataReporterOptions _reporterOptions;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly EAPClientOptions _eAPClientOptions;
    private readonly PeriodicTimer _periodicTimer;
    private volatile bool _eAPConnected;

    public event Func<Task> OnTick;

    public event Func<LotInfoRequestModel, Task<LotInfoRequestModel>>? OnLotInfoRequestReturn;
    public event Func<EQPCompletedReportModel, Task<EQPCompletedReportModel>>? OnEQPCompletedReportReturn;

    public VgAOIHttpDataReporter(ILogger<VgAOIHttpDataReporter> logger,
            IOptions<HttpDataReporterOptions> reporterOptions,
            IOptions<EAPClientOptions> options1,
            IHostApplicationLifetime hostApplicationLifetime,
            IObjectFactory objectFactory,
            IHttpRequestInvoker httpRequestInvoker)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _objectFactory = objectFactory;
        _reporterOptions = reporterOptions.Value;
        _eAPClientOptions = options1.Value;
        _httpRequestInvoker = httpRequestInvoker;
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(_reporterOptions.HeartBeatSeconds));
        _hostApplicationLifetime.ApplicationStopped.Register(() => Dispose());
        OnTick += () => Task.CompletedTask;
    }

    /// <summary>
    /// eap是否连接
    /// </summary>
    public bool EAPConnected { get => _eAPConnected; private set => _eAPConnected = value; }

    public void Dispose()
    {
        _periodicTimer.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (_periodicTimer)
        {
            while (!cancellationToken.IsCancellationRequested && await _periodicTimer.WaitForNextTickAsync())
            {
                try
                {
                    await UpdateStatusAndAlarm();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                finally
                {
                }
            }
        }
    }

    public async Task UpdateStatusAndAlarm()
    {
        try
        {
            using (var ctx = _objectFactory.CreateObject<VegaContext>())
            {
                using var transaction = ctx.Database.BeginTransaction();
                try
                {
                    var entity = ctx.PC_SIGNAL_STATUSs.ToList();
                    if (entity.Count > 0)
                    {
                        foreach (var item in entity)
                        {
                            await SendEQPStatusChangeReport(new EQPStatusChangeReportBody
                            {
                                EquipmentID = _eAPClientOptions.EquipmentID,
                                Status = item.SIGNALDES
                            });
                            ctx.PC_SIGNAL_STATUSs.Remove(item);
                        }
                    }

                    var entityAlarm = ctx.PC_DATA_ALARMs.ToList();
                    if (entityAlarm.Count > 0)
                    {
                        foreach (var item in entityAlarm)
                        {
                            await SendEQPAlarmReport(new EQPAlarmReportBody
                            {
                                EquipmentID = _eAPClientOptions.EquipmentID,
                                AlarmID = item.ALARMCODE,
                                AlarmLevel = "1",
                                AlarmStatus = item.ALARMVALUE,
                                AlarmText = item.ALARMDES,
                            });
                            ctx.PC_DATA_ALARMs.Remove(item);
                        }
                    }

                    ctx.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "UpdateEAPSingalStatus ctx- Exception" + ex.Message);
                    transaction.Rollback();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateEAPSingalStatus Exception - " + ex.Message);
        }
    }

    public async Task SendEQPAlarmReport(EQPAlarmReportBody reportBody)
    {
        try
        {
            var report = new EQPAlarmReportModel(
                 header: new EQPReportHeader
                 {
                     MessageName = "EQPAlarmReport",
                     TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                     UserID = _eAPClientOptions.UserID,
                 },
                body: reportBody,
                result: new EQPReportResult());

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPAlarmReportModel, object>(
                _reporterOptions.EQPAlarmReportUrl, report);
            _logger.LogInformation("Send内容 - " + EntityUtil<EQPAlarmReportModel>.EntityToJson(report));
            _logger.LogInformation("SendEQPAlarmReport 返回内容：" + return_code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendEQPAlarmReport Exception - " + ex.Message);
        }
    }

    public async Task SendEQPDataCollectionReport(EQPDataCollectionReportBody reportBody)
    {
        try
        {
            var report = new EQPDataCollectionReportModel(
                 header: new EQPReportHeader
                 {
                     MessageName = "EQPDataCollectionReport",
                     TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                     UserID = _eAPClientOptions.UserID,
                 },
                body: reportBody,
                result: new EQPReportResult());

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPDataCollectionReportModel, object>(
                _reporterOptions.EQPDataCollectionReportUrl, report);
            _logger.LogInformation("Send内容 - " + EntityUtil<EQPDataCollectionReportModel>.EntityToJson(report));
            _logger.LogInformation("SendEQPDataCollectionReport 返回内容：" + return_code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendEQPDataCollectionReport Exception - " + ex.Message);
        }
    }

    public async Task SendEQPStatusChangeReport(EQPStatusChangeReportBody reportBody)
    {
        try
        {
            var report = new EQPStatusChangeReportModel(
                 header: new EQPReportHeader
                 {
                     MessageName = "EQPStatusChangeReport",
                     TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                     UserID = _eAPClientOptions.UserID,
                 },
                body: reportBody,
                result: new EQPReportResult());

            var return_code = await _httpRequestInvoker.PostAsJsonAsync<EQPStatusChangeReportModel, object>(
            _reporterOptions.EQPStatusChangeReportUrl, report);
            _logger.LogInformation("Send内容 - " + EntityUtil<EQPStatusChangeReportModel>.EntityToJson(report));
            _logger.LogInformation("SendEQPStatusChangeReport 返回内容：" + return_code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EQPStatusChangeReport异常 - " + ex.Message);
        }
    }

    public Task SendAreYouThere(AreYouThereBody reportBody) => Task.CompletedTask;

    public Task SendEQPDateTimeRequest(EQPDateTimeRequestBody reportBody) => Task.CompletedTask;

    public Task SendEQPCommunicationStatusReport(EQPCommunicationStatusReportBody reportBody) => Task.CompletedTask;

    public Task SendEQPRunningModeReport(EQPRunningModeReportBody reportBody) => Task.CompletedTask;

    public Task SendLotInfoRequest(LotInfoRequestBody reportBody) => Task.CompletedTask;

    public Task SendEQPCurrentChangeRecipeReport(EQPCurrentChangeRecipeReportBody reportBody) => Task.CompletedTask;

    public Task SendEQPReceiveJobReport(EQPReceiveJobReportBody reportBody) => Task.CompletedTask;

    public Task SendEQPSendOutJobReport(EQPSendOutJobReportBody reportBody) => Task.CompletedTask;

    public Task SendEQPCompletedReport(EQPCompletedReportBody reportBody) => Task.CompletedTask;

    public Task SendUserCheckCardReport(UserCheckCardReportBody reportBody) => Task.CompletedTask;

    public Task SendPanelProcessDataReport(PanelProcessDataReportBody reportBody) => Task.CompletedTask;

    public Task SendBrokenKnifeAlarmReport(BrokenKnifeAlarmReportBody reportBody) => Task.CompletedTask;

    public Task SendIPPortReport(IPPortReportBody reportBody) => throw new NotImplementedException();
    public Task SendMaterialValidationRequest(MaterialValidationRequestBody reportBody) => Task.CompletedTask;
    public Task SendMaterialUseReport(MaterialUseReportBody reportBody) => Task.CompletedTask;
}
