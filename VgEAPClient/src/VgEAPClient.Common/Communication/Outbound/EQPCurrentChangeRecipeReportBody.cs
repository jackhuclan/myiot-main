// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Outbound;

public class EQPCurrentChangeRecipeReportBody : EQPReportBody
{
    public string? LotID { get; set; }//批次号
    public string? ItemNum { get; set; }
    public string? RecipeID { get; set; }//配方路径
    /// <summary>
    /// Add：新增配方回复;Update：修改配方回复;Delete：删除配方回复;Select：验证配方回复;Check: EAP比对配方（LDI曝光机）;Noexist ：配方不存在
    /// </summary>
    public string Action { get; set; } = string.Empty;
    public List<Parameter> RecipeList { get; set; } = new List<Parameter>();
}
