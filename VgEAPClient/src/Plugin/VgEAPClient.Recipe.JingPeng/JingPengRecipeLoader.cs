// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz.Util;
using VgAutoDrill.OpenAPI;
using VgEAPClient.Common;
using VgEAPClient.Common.CNC;

namespace VgEAPClient.Recipe.JingPeng;

public class JingPengRecipeLoader : IWorkOrderRecipeLoader
{
    private readonly ILogger<JingPengRecipeLoader> _logger;
    private readonly EAPClientOptions _eAPClientOptions;

    public JingPengRecipeLoader(IHttpRequestInvoker httpRequestInvoker,
        IOptions<EAPClientOptions> options,
        ILoggerFactory loggerFactory)
    {
        _eAPClientOptions = options.Value;
        _logger = loggerFactory.CreateLogger<JingPengRecipeLoader>();
    }

    public Task<List<WorkOrderRecipe>> LoadRecipe(WorkOrderRecipeRequest workOrderRecipeRequest)
    {
        List<WorkOrderRecipe> fileList = new List<WorkOrderRecipe>();
        if (workOrderRecipeRequest.ItemCode.Length >= 3)
        {
            //04C6B0220001A.21TY
            string tystr = "";
            string[] itemsplits = workOrderRecipeRequest.ItemCode.Split('.');
            string Item = itemsplits[0];
            Item = Item.Remove(Item.Length - 1);
            if (itemsplits.Length > 1)
            {
                tystr = $@".{itemsplits[1]}";
            }
            string thridchar = workOrderRecipeRequest.ItemCode.ToArray()[2].ToString();
            string DirName = $@"{thridchar}{thridchar}{thridchar}";
            DirectoryInfo Searchdir = new DirectoryInfo(_eAPClientOptions.RecipeSearchPath);
            if (Searchdir.Exists)
            {
                var AllDirs = Searchdir.GetDirectories($@"{DirName}", SearchOption.AllDirectories);
                foreach (var Directory in AllDirs)
                {
                    var files = Directory.GetFiles($@"*{Item}*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (fileList.Any(o => o.tagPath.Equals(file.FullName, StringComparison.OrdinalIgnoreCase)))
                        {
                            continue;
                        }
                        if (file.Extension.Equals(_eAPClientOptions.AtpSearchSuffix, StringComparison.OrdinalIgnoreCase))
                        {
                            fileList.Add(new WorkOrderRecipe() { tagCode = workOrderRecipeRequest.ItemCode, tagType = LoadFileType.ATP, tagPath = file.FullName });
                        }
                        else if (file.Extension.Equals(_eAPClientOptions.DiaSearchSuffix, StringComparison.OrdinalIgnoreCase))
                        {
                            fileList.Add(new WorkOrderRecipe() { tagCode = workOrderRecipeRequest.ItemCode, tagType = LoadFileType.DIA, tagPath = file.FullName });
                        }
                        else
                        {
                            if (tystr.IsNullOrWhiteSpace() ||
                                file.Extension.Equals(tystr, StringComparison.OrdinalIgnoreCase))
                            {
                                fileList.Add(new WorkOrderRecipe() { tagCode = workOrderRecipeRequest.ItemCode, tagType = LoadFileType.PROGRAM, tagPath = file.FullName });
                            }

                        }
                    }
                }
            }
        }

        return Task.FromResult(fileList);
    }
}
