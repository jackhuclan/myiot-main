// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VgAutoDrill.OpenAPI;
using VgEAPClient.Common;
using VgEAPClient.Common.CNC;

namespace VgEAPClient.Recipe.Everything64;

public class Everything64RecipeLoader : IWorkOrderRecipeLoader
{
    private readonly ILogger<Everything64RecipeLoader> _logger;
    private readonly EAPClientOptions _eAPClientOptions;

    public Everything64RecipeLoader(IHttpRequestInvoker httpRequestInvoker,
        IOptions<EAPClientOptions> options,
        ILoggerFactory loggerFactory)
    {
        _eAPClientOptions = options.Value;
        _logger = loggerFactory.CreateLogger<Everything64RecipeLoader>();
    }

    public Task<List<WorkOrderRecipe>> LoadRecipe(WorkOrderRecipeRequest workOrderRecipeRequest)
    {
        List<WorkOrderRecipe> fileList = new List<WorkOrderRecipe>();
        Everything64.Everything_SetSearchW(_eAPClientOptions.RecipeSearchPath + " " + workOrderRecipeRequest.ItemCode);
        Everything64.Everything_QueryW(true);
        const int bufferSize = 1024;
        StringBuilder buffer = new StringBuilder(bufferSize);
        uint num = Everything64.Everything_GetNumResults();

        for (uint i = 0; i < num; i++)
        {
            Everything64.Everything_GetResultFullPathName(i, buffer, bufferSize);

            if (Everything64.Everything_IsFileResult(i))
            {
                if (buffer.ToString().ToLower().EndsWith(_eAPClientOptions.AtpSearchSuffix))
                {
                    fileList.Add(new WorkOrderRecipe() { tagCode = workOrderRecipeRequest.ItemCode, tagType = LoadFileType.ATP, tagPath = buffer.ToString() });
                }
                else if (buffer.ToString().ToLower().EndsWith(_eAPClientOptions.DiaSearchSuffix))
                {
                    fileList.Add(new WorkOrderRecipe() { tagCode = workOrderRecipeRequest.ItemCode, tagType = LoadFileType.DIA, tagPath = buffer.ToString() });
                }
                else
                {
                    fileList.Add(new WorkOrderRecipe() { tagCode = workOrderRecipeRequest.ItemCode, tagType = LoadFileType.PROGRAM, tagPath = buffer.ToString() });
                }
            }
        }

        return Task.FromResult(fileList);
    }
}
