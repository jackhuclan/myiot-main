// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz.Util;
using VgAutoDrill.OpenAPI;
using VgEAPClient.Common;
using VgEAPClient.Common.CNC;
using VgEAPClient.Recipe.JiangZhouChongDa.Model;

namespace VgEAPClient.Recipe.JiangZhouChongDa;

public class JiangZhouChongDaRecipeLoader : IWorkOrderRecipeLoader
{
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ILogger<JiangZhouChongDaRecipeLoader> _logger;
    private readonly EAPClientOptions _eAPClientOptions;

    public JiangZhouChongDaRecipeLoader(IHttpRequestInvoker httpRequestInvoker,
        IOptions<EAPClientOptions> options,
        ILoggerFactory loggerFactory)
    {
        _httpRequestInvoker = httpRequestInvoker;
        _eAPClientOptions = options.Value;
        _logger = loggerFactory.CreateLogger<JiangZhouChongDaRecipeLoader>();
    }

    public async Task<List<WorkOrderRecipe>> LoadRecipe(WorkOrderRecipeRequest workOrderRecipeRequest)
    {
        var list = new List<WorkOrderRecipe>();
        if (string.IsNullOrWhiteSpace(_eAPClientOptions.RecipeSearchPath)
            || !_eAPClientOptions.RecipeSearchPath.StartsWith("http"))
        {
            _logger.LogError($@"请求路径配置错误,必须以http开头,当前路径[{_eAPClientOptions.RecipeSearchPath}]");
            return list;
        }


        try
        {
            var reportBody = new DrillInfoLocatorReport
            {
                woCode = workOrderRecipeRequest.ItemCode,
                fCode = workOrderRecipeRequest.EquipmentId,
            };

            _httpRequestInvoker.ConfigureHttpRequestHeaders(header =>
            {
                header.TransferEncodingChunked = false;
            });

            DrillInfoLocatorReply drillInfoLocatorReply = new DrillInfoLocatorReply();

            _logger.LogInformation("LoadRecipe(Request)" + "\r\n"
                + $@"Url:{_eAPClientOptions.RecipeSearchPath}" + "\r\n"
                + $@"data:{reportBody.ToJsonNull()}" + "\r\n");
            object? reobj = await _httpRequestInvoker.PostAsJsonAsync<DrillInfoLocatorReport, object>(_eAPClientOptions.RecipeSearchPath, reportBody);
            if (reobj != null)
            {
                _logger.LogInformation("LoadRecipe(Reponse)" + "\r\n"
                    + $@"Url:{_eAPClientOptions.RecipeSearchPath}" + "\r\n"
                    + $@"data:{reobj.ToStringEx()}");
                drillInfoLocatorReply = JsonSerializer.Deserialize<DrillInfoLocatorReply>(reobj.ToStringEx()) ?? new DrillInfoLocatorReply();
                _logger.LogInformation("LoadRecipe(ReponseFormat)" + "\r\n"
                    + $@"Url:{_eAPClientOptions.RecipeSearchPath}" + "\r\n"
                    + $@"data:{drillInfoLocatorReply.ToJsonNull()}");
            }

            foreach (var dia in drillInfoLocatorReply.Data.dia)
            {
                list.Add(new WorkOrderRecipe
                {
                    tagPath = dia,
                    tagCode = reportBody.woCode,
                    tagType = LoadFileType.DIA
                });
            }
            foreach (var drl in drillInfoLocatorReply.Data.drl)
            {
                list.Add(new WorkOrderRecipe
                {
                    tagPath = drl,
                    tagCode = reportBody.woCode,
                    tagType = LoadFileType.PROGRAM
                });
            }

            return list;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($@"LoadRecipe(Error) - {ex.Message}");
        }

        return list;
    }
}
