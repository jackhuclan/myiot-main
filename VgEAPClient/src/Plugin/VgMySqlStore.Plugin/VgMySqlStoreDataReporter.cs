using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;
using VgEAPClient.Common;
using VgEAPClient.Common.Communication.Outbound;

namespace VgMySqlStore.Plugin;

public class VgMySqlStoreDataReporter : IHttpDataReporter, IDisposable
{
    private readonly ILogger<VgMySqlStoreDataReporter> _logger;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IObjectFactory _objectFactory;
    private readonly HttpDataReporterOptions _reporterOptions;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly EAPClientOptions _eAPClientOptions;
    private readonly PeriodicTimer _periodicTimer;
    private volatile bool _eAPConnected;
    public event Func<Task> OnTick;

    public VgMySqlStoreDataReporter(ILogger<VgMySqlStoreDataReporter> logger,
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
                    //
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
    public Task SendEQPAlarmReport(EQPAlarmReportBody reportBody) => throw new NotImplementedException();
    public Task SendEQPDataCollectionReport(EQPDataCollectionReportBody reportBody) => throw new NotImplementedException();
    public Task SendEQPStatusChangeReport(EQPStatusChangeReportBody reportBody) => throw new NotImplementedException();
}
