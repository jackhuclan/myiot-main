// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgEAPClient.Common;
using VgEAPClient.Socket.Client;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;
public class SoketDataReceiver : IEQPSoketDataReceiver
{
    public event Func<EapMessage, Task<EapMessage>>? OnInitialDataRequestReceived;
    public event Func<EapMessage, Task<EapMessage>>? OnLotInfoDownloadCommandReceived;
    public event Func<EapMessage, Task<EapMessage>>? OnDateTimeCommandReceived;

    public readonly ILogger<SoketDataReceiver> _logger;
    public readonly EapServerOptions _eapServerOptions;
    public readonly EAPClientOptions _eAPClientOptions;
    public readonly ISokcetClient _socketClient;

    public SoketDataReceiver(ILogger<SoketDataReceiver> logger,
        IOptions<EapServerOptions> eapServerOptions,
        IOptions<EAPClientOptions> eAPClientOptions,
        ISokcetClient socketClient)
    {
        _logger = logger;
        _eapServerOptions = eapServerOptions.Value;
        _eAPClientOptions = eAPClientOptions.Value;
        _socketClient = socketClient;

        _socketClient.OnHostRequest += OnHostRequest;
    }

    public virtual async Task OnHostRequest(EapMessage eap)
    {
        try
        {
            switch (eap.Header.MessageName)
            {
                case "InitialDataRequest":
                    {
                        var reeap = await InitialDataRequest(eap);
                        if (reeap != null)
                        {
                            await _socketClient.SendMessage(reeap);
                        }
                    }
                    break;
                case "WODataDownloadCommand":
                    {
                        var reeap = await LotInfoDownloadCommand(eap);
                        if (reeap != null)
                        {
                            await _socketClient.SendMessage(reeap);
                        }
                    }
                    break;
                case "DateTimeSyncCommand":
                    {
                        var reeap = await DateTimeCommand(eap);
                        if (reeap != null)
                        {
                            await _socketClient.SendMessage(reeap);
                        }
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    public Task<EapMessage> InitialDataRequest(EapMessage eap) =>
        OnInitialDataRequestReceived?.Invoke(eap) ?? Task.FromResult(new EapMessage());


    public Task<EapMessage> LotInfoDownloadCommand(EapMessage eap) =>
        OnLotInfoDownloadCommandReceived?.Invoke(eap) ?? Task.FromResult(new EapMessage());
    public Task<EapMessage> DateTimeCommand(EapMessage eap) =>
        OnDateTimeCommandReceived?.Invoke(eap) ?? Task.FromResult(new EapMessage());
}
