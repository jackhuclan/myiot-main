// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.OpenAPI;

namespace VgEAPClient.Common.Communication.Outbound;

public class HttpEAPHeartbeater : IEAPHeartbeater, IDisposable
{
    public readonly ILogger<HttpEAPHeartbeater> _logger;
    public readonly IHostApplicationLifetime _hostApplicationLifetime;
    public readonly HttpDataReporterOptions _reporterOptions;
    public readonly IHttpRequestInvoker _httpRequestInvoker;
    public readonly EAPClientOptions _eAPClientOptions;
    public readonly PeriodicTimer _periodicTimer;
    public volatile bool _eAPConnected;
    public readonly JsonSerializerOptions _jsonSerializerOptions;

    public event Func<bool, Task> OnEAPConnectedChanged;

    public HttpEAPHeartbeater(ILogger<HttpEAPHeartbeater> logger,
            IOptions<HttpDataReporterOptions> reporterOptions,
            IOptions<EAPClientOptions> options1,
            IHostApplicationLifetime hostApplicationLifetime,
            IHttpRequestInvoker httpRequestInvoker)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _reporterOptions = reporterOptions.Value;
        _eAPClientOptions = options1.Value;
        _httpRequestInvoker = httpRequestInvoker;
        _httpRequestInvoker.ConfigureHttpRequestHeaders(headers =>
        {
            headers.Add("Basic_base64_auth_string", _eAPClientOptions.AuthString);
            headers.Add("EqpId", _eAPClientOptions.EquipmentID);
        });
        _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(_reporterOptions.HeartBeatSeconds));
        _hostApplicationLifetime.ApplicationStopped.Register(() => Dispose());
        OnEAPConnectedChanged = t => Task.CompletedTask;
        _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = null };
    }

    /// <summary>
    /// eap是否连接
    /// </summary>
    public bool Connected
    {
        get => _eAPConnected;
        private set
        {
            var changed = _eAPConnected != value;
            _eAPConnected = value;
            if (changed)
            {
                OnEAPConnectedChanged.Invoke(_eAPConnected);
            }
        }
    }

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
                    await SendAreYouThere(new AreYouThereBody
                    {
                        EquipmentID = _eAPClientOptions.EquipmentID
                    });
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
    public virtual async Task SendAreYouThere(AreYouThereBody reportBody)
    {
        if (string.IsNullOrWhiteSpace(_reporterOptions.AreYouThereUrl))
            return;

        var data = new AreYouThereModel
        (
            header: new EQPReportHeader
            {
                MessageName = "AreYouThere",
                UserID = _eAPClientOptions.UserID,
                TransactionID = DateTime.Now.ToString("yyyyMMddHHmmssffffff"),
            },
            body: reportBody,
            result: new EQPReportResult()
        );

        try
        {
            var return_code = await _httpRequestInvoker.PostAsJsonAsync<AreYouThereModel, object>(_reporterOptions.AreYouThereUrl, data, _jsonSerializerOptions);
            _logger.LogInformation("AreYouThere返回内容：" + return_code);
            if (return_code != null)
            {
                AreYouThereModel areYouThereModel = EntityUtil<AreYouThereModel>.JsonToEntity(return_code.ToString());
                if (areYouThereModel.Result.Code == 1)
                {
                    _eAPConnected = true;
                }
                else
                {
                    _eAPConnected = false;
                }
            }
            else
            {
                _eAPConnected = false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SendAreYouThere异常 - " + ex.Message);
            _eAPConnected = false;
        }
    }
}
