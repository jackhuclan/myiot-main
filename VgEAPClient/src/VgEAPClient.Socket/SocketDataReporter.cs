// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgEAPClient.Common;
using VgEAPClient.Socket.Client;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;

public class SocketDataReporter : IEQPSocketDataReporter, IDisposable
{

    public event Func<Task> OnTick;
    public event Func<EapMessage, Task<EapMessage>>? OnLotInfoRequestReturn;

    public readonly ILogger<SocketDataReporter> _logger;
    public readonly IHostApplicationLifetime _hostApplicationLifetime;
    public readonly ISokcetClient _socketClient;
    public readonly EapServerOptions _eapServerOptions;
    public readonly EAPClientOptions _eAPClientOptions;
    public CancellationTokenSource? _stoppingCts;

    public SocketDataReporter(ILogger<SocketDataReporter> logger,
        IOptions<EapServerOptions> eapServerOptions,
        IOptions<EAPClientOptions> eAPClientOptions,
        IHostApplicationLifetime hostApplicationLifetime,
        ISokcetClient socketClient)
    {
        _logger = logger;
        _hostApplicationLifetime = hostApplicationLifetime;
        _socketClient = socketClient;
        _eapServerOptions = eapServerOptions.Value;
        _eAPClientOptions = eAPClientOptions.Value;

        _hostApplicationLifetime.ApplicationStopped.Register(() => Dispose());
        OnTick += () => Task.CompletedTask;
    }

    public void OnLotInfoRequestReturnInvoke(EapMessage arg)
    {
        OnLotInfoRequestReturn?.Invoke(arg);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        await ExecuteAsync(_stoppingCts.Token);
    }

    private async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await OnTick.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            await Task.Delay(_eapServerOptions.DataCollectionReportSeconds * 1000);
        }
    }
    public void Dispose()
    {
        _stoppingCts?.Cancel();
    }
    public virtual async Task SendEQPAlarmReport(EapMessage.EapBody body)
    {
        EapMessage eap = new EapMessage
        {
            Header = new EapMessage.EapHeader
            {
                MessageName = "EQPAlarmReport",
                TransactionId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            },
            Body = body
        };
        await _socketClient.SendMessage(eap);
    }
    public virtual async Task SendEQPSendOutJobReport(EapMessage.EapBody body)
    {
        EapMessage eap = new EapMessage
        {
            Header = new EapMessage.EapHeader
            {
                MessageName = "EQPSendOutJobReport",
                TransactionId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            },
            Body = body
        };
        await _socketClient.SendMessage(eap);
    }
    public virtual async Task SendEQPStatusChangeReport(EapMessage.EapBody body)
    {
        EapMessage eap = new EapMessage
        {
            Header = new EapMessage.EapHeader
            {
                MessageName = "EQPStatusChangeReport",
                TransactionId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            },
            Body = body
        };
        await _socketClient.SendMessage(eap);
    }
    public virtual async Task SendEquipmentInfoReport(EapMessage.EapBody body)
    {
        EapMessage eap = new EapMessage
        {
            Header = new EapMessage.EapHeader
            {
                MessageName = "EquipmentInfo",
                TransactionId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            },
            Body = body
        };
        await _socketClient.SendMessage(eap);
    }
    public virtual async Task SendLotInfoRequest(EapMessage.EapBody body)
    {
        EapMessage eap = new EapMessage
        {
            Header = new EapMessage.EapHeader
            {
                MessageName = "LotInfoRequest",
                TransactionId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            },
            Body = body
        };
        await _socketClient.SendMessage(eap);
    }

    public virtual async Task<EapMessage> SendUserCheckCardReport(EapMessage.EapBody body)
    {
        EapMessage eap = new EapMessage
        {
            Header = new EapMessage.EapHeader
            {
                MessageName = "UserVerifyRequest",
                TransactionId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            },
            Body = body
        };
        await _socketClient.SendMessage(eap);
        return new EapMessage();
    }

    public virtual async Task SendEQPDataCollectionReport(EapMessage.EapBody body)
    {
        EapMessage eap = new EapMessage
        {
            Header = new EapMessage.EapHeader
            {
                MessageName = "UtilityDataReport",
                TransactionId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            },
            Body = body
        };
        await _socketClient.SendMessage(eap);
    }

    public virtual async Task SendBrokenKnifeAlarmReport(EapMessage.EapBody body)
    {
        EapMessage eap = new EapMessage
        {
            Header = new EapMessage.EapHeader
            {
                MessageName = "FixtureBrokenReportRequest",
                TransactionId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            },
            Body = body
        };
        await _socketClient.SendMessage(eap);
    }
}
