// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgEAPClient.Common;
using VgEAPClient.Socket.Client;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;

internal class EapClientHeartBeat : BackgroundService
{
    private readonly ISokcetClient eapClient;
    private readonly ITransactionIdMaker transactionIdMaker;
    private readonly EapServerOptions serverOptions;
    private readonly ILogger<EapClientHeartBeat> logger;
    private readonly EAPClientOptions _eAPClientOptions;

    public EapClientHeartBeat(ISokcetClient eapClient,
        IOptions<EapServerOptions> options,
        IOptions<EAPClientOptions> options2,
        ITransactionIdMaker transactionIdMaker,
        ILoggerFactory loggerFactory)
    {
        this.eapClient = eapClient;
        this.transactionIdMaker = transactionIdMaker;
        serverOptions = options.Value;
        _eAPClientOptions = options2.Value;
        logger = loggerFactory.CreateLogger<EapClientHeartBeat>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (eapClient.IsConnected)
            {
                var message = new EapMessage
                {
                    Body = new AreYouThereRequest()
                    {
                        EqpId = _eAPClientOptions.EquipmentID,
                        ServerIp = serverOptions.Host,
                    }
                };
                message.Header.MessageName = nameof(AreYouThereRequest);
                message.Header.TransactionId = transactionIdMaker.NextId();
                var reply = await eapClient.SendMessage(message, stoppingToken);
                logger.LogInformation($"Got heartbeat reply from Host:{reply}");
            }

            await Task.Delay(60 * 1000);
        }
    }
}
