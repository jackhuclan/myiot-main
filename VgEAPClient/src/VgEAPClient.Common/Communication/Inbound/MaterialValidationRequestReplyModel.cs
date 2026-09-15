// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Utils;

namespace VgEAPClient.Common.Communication.Inbound;
public class MaterialValidationRequestReplyModel : EQPReportModel<EQPReportHeader, MaterialValidationRequestReplyBody, EQPReportResult>
{
    public MaterialValidationRequestReplyModel(EQPReportHeader header, MaterialValidationRequestReplyBody body, EQPReportResult result)
        : base(header, body, result)
    {
    }

    public MaterialValidationRequestReplyModel()
    {

    }
}


public class MaterialValidationRequestReplyBody : EQPReportBody
{
    /// <summary>
    /// 物料ID
    /// </summary>
    public string MaterialID { get; set; } = string.Empty;
    /// <summary>
    /// OK：成功;NG：失败
    /// </summary>
    public string ResultCode { get; set; } = string.Empty;
    public string MaterialQTY { get; set; } = string.Empty;
}


public class MaterialValidationRequestReplyReceiver
{
    private readonly IEQPDataReceiver _eQPDataReceiver;
    private readonly ILogger<MaterialValidationRequestReplyReceiver> _logger;
    private readonly EAPClientOptions _eAPClientOptions;

    public MaterialValidationRequestReplyReceiver(ILogger<MaterialValidationRequestReplyReceiver> logger,
        IOptions<EAPClientOptions> eAPClientOptions,
        IEQPDataReceiver eQPDataReceiver)
    {
        _logger = logger;
        _eQPDataReceiver = eQPDataReceiver;
        _eAPClientOptions = eAPClientOptions.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        MaterialValidationRequestReplyModel faildata = new MaterialValidationRequestReplyModel()
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
            _logger.LogInformation($@"MaterialValidationRequestReply[EAP -> EQP] - {content}");

            MaterialValidationRequestReplyModel? redata = JsonSerializer.Deserialize<MaterialValidationRequestReplyModel>(content,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            });
            MaterialValidationRequestReplyModel senddata = new MaterialValidationRequestReplyModel();
            if (redata != null)
            {
                senddata = await _eQPDataReceiver.MaterialValidationRequestReply(redata);
            }
            else
            {
                senddata = faildata;
            }

            context.Response.Headers.Append("Basic_base64_auth_string", _eAPClientOptions.AuthString);
            context.Response.Headers.Append("EqpId", _eAPClientOptions.EquipmentID);

            _logger.LogInformation($@"MaterialValidationRequestReply[EQP -> EAP] - {senddata.ToJson()}");
            await context.Response.WriteAsJsonAsync(senddata);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"MaterialValidationRequestReply[error] - {ex.Message}");
            await context.Response.WriteAsJsonAsync(faildata);

        }
    }
}
