// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Utils;

namespace VgEAPClient.Common.Communication.Inbound;

public class CIMMessageCommandReceiver
{
    private readonly IEQPDataReceiver _eQPDataReceiver;
    private readonly ILogger<CIMMessageCommandReceiver> _logger;
    private readonly EAPClientOptions _eAPClientOptions;

    public CIMMessageCommandReceiver(ILogger<CIMMessageCommandReceiver> logger,
        IOptions<EAPClientOptions> eAPClientOptions,
        IEQPDataReceiver eQPDataReceiver)
    {
        _logger = logger;
        _eQPDataReceiver = eQPDataReceiver;
        _eAPClientOptions = eAPClientOptions.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        var faildata = new CIMMessageCommandModel()
        {
            Header = new EQPReportHeader()
            {
                MessageName = "",
            },
            Result = new EQPReportResult
            {
                Code = 0//接受消息失败
            }
        };
        try
        {
            HttpRequest request = context.Request;
            StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8);
            string content = await reader.ReadToEndAsync();
            _logger.LogInformation($@"CIMMessageCommand[EAP -> EQP] - {content}");

            var redata = JsonSerializer.Deserialize<CIMMessageCommandModel>(content,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });
            var senddata = new CIMMessageCommandModel();
            if (redata != null)
            {
                senddata = await _eQPDataReceiver.CIMMessageCommand(redata);
            }
            else
            {
                senddata = faildata;
            }

            context.Response.Headers.Append("Basic_base64_auth_string", _eAPClientOptions.AuthString);
            context.Response.Headers.Append("EqpId", _eAPClientOptions.EquipmentID);

            _logger.LogInformation($@"CIMMessageCommand[EQP -> EAP] - {senddata.ToJson()}");
            await context.Response.WriteAsJsonAsync(senddata);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"CIMMessageCommand[error] - {ex.Message}");
            await context.Response.WriteAsJsonAsync(faildata);

        }
    }
}
