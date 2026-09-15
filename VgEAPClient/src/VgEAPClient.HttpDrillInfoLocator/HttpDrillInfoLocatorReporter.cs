using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.OpenAPI;
using VgEAPClient.Common;
using VgEAPClient.Common.CNC;
using VgEAPClient.HttpDrillInfoLocator.Models;

namespace VgEAPClient.HttpDrillInfoLocator;

public class HttpDrillInfoLocatorReporter : IWorkOrderRecipeLoader
{
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly ILogger<HttpDrillInfoLocatorReporter> _logger;
    private readonly EAPClientOptions _eAPClientOptions;

    public HttpDrillInfoLocatorReporter(IHttpRequestInvoker httpRequestInvoker,
        IOptions<EAPClientOptions> options,
        ILoggerFactory loggerFactory)
    {
        _httpRequestInvoker = httpRequestInvoker;
        _eAPClientOptions = options.Value;
        _logger = loggerFactory.CreateLogger<HttpDrillInfoLocatorReporter>();
    }

    public async Task<List<WorkOrderRecipe>> LoadRecipe(WorkOrderRecipeRequest workOrderRecipeRequest)
    {
        var list = new List<WorkOrderRecipe>();
        if (string.IsNullOrWhiteSpace(_eAPClientOptions.RecipeSearchPath)
            || !_eAPClientOptions.RecipeSearchPath.StartsWith("http"))
            return list;

        try
        {
            var reportBody = new DrillInfoLocatorReport
            {
                wipCode = workOrderRecipeRequest.ItemCode,
                workMac = workOrderRecipeRequest.EquipmentId,
            };

            _httpRequestInvoker.ConfigureHttpRequestHeaders(header =>
            {
                header.TransferEncodingChunked = false;
            });

            DrillInfoLocatorReply drillInfoLocatorReply = new DrillInfoLocatorReply();

            _logger.LogInformation(reportBody.ToJson());
            object? reobj = await _httpRequestInvoker.PostAsJsonAsync<DrillInfoLocatorReport, object>(_eAPClientOptions.RecipeSearchPath, reportBody);
            if (reobj != null)
            {
                _logger.LogInformation(reobj.ToStringEx());
                drillInfoLocatorReply = JsonSerializer.Deserialize<DrillInfoLocatorReply>(reobj.ToStringEx()) ?? new DrillInfoLocatorReply();
            }

            foreach (var dia in drillInfoLocatorReply.data.dia)
            {
                list.Add(new WorkOrderRecipe
                {
                    tagPath = dia,
                    tagCode = reportBody.wipCode,
                    tagType = LoadFileType.DIA
                });
            }
            foreach (var drl in drillInfoLocatorReply.data.drl)
            {
                list.Add(new WorkOrderRecipe
                {
                    tagPath = drl,
                    tagCode = reportBody.wipCode,
                    tagType = LoadFileType.PROGRAM
                });
            }

            return list;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return list;
    }
}
