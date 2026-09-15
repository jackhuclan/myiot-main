// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Utils;

namespace VgEAPClient.Common.Communication.Inbound;

public class RecipeValidationResultCommandReceiver
{
    private readonly ILogger<RecipeValidationResultCommandReceiver> _logger;
    private readonly IEQPDataReceiver _eQPDataReceiver;
    private readonly EAPClientOptions _eAPClientOptions;

    public RecipeValidationResultCommandReceiver(ILogger<RecipeValidationResultCommandReceiver> logger,
        IOptions<EAPClientOptions> eAPClientOptions,
        IEQPDataReceiver eQPDataReceiver)
    {
        _logger = logger;
        _eQPDataReceiver = eQPDataReceiver;
        _eAPClientOptions = eAPClientOptions.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        RecipeValidationResultCommandModel faildata = new RecipeValidationResultCommandModel()
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
            _logger.LogInformation($@"RecipeValidationResultCommand[EAP -> EQP] - {content}");

            RecipeValidationResultCommandModel? redata = JsonSerializer.Deserialize<RecipeValidationResultCommandModel>(content,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });
            RecipeValidationResultCommandModel senddata = new RecipeValidationResultCommandModel();
            if (redata != null)
            {
                senddata = await _eQPDataReceiver.RecipeValidationResultCommand(redata);
            }
            else
            {
                senddata = faildata;
            }

            context.Response.Headers.Append("Basic_base64_auth_string", _eAPClientOptions.AuthString);
            context.Response.Headers.Append("EqpId", _eAPClientOptions.EquipmentID);

            _logger.LogInformation($@"RecipeValidationResultCommand[EQP -> EAP] - {senddata.ToJson()}");
            await context.Response.WriteAsJsonAsync(senddata);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"RecipeValidationResultCommand[error] - {ex.Message}");
            await context.Response.WriteAsJsonAsync(faildata);

        }
    }
}
