// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication.Inbound;

public class LotInfoDownloadCommandBody : EQPReportBody
{
    /// <summary>
    /// 批次号
    /// </summary>
    public string ProductNo { get; set; } = string.Empty;
    /// <summary>
    /// 工单号
    /// </summary>
    public string LotID { get; set; } = string.Empty;
    /// <summary>
    /// 批次号
    /// </summary>
    public string ItemNum { get; set; } = string.Empty;
    public string PnlWidth { get; set; } = string.Empty;
    /// <summary>
    /// 板长
    /// </summary>
    public string PnlLength { get; set; } = string.Empty;
    /// <summary>
    /// 板厚
    /// </summary>
    public string PnlThick { get; set; } = string.Empty;
    /// <summary>
    /// 板件数量
    /// </summary>
    public string PanelQTY { get; set; } = string.Empty;
    /// <summary>
    /// 配方ID或配方资料路径
    /// </summary>
    public string RecipeID { get; set; } = string.Empty;

    public string PortID { get; set; } = string.Empty;
    /// <summary>
    /// 载具ID
    /// </summary>
    public string CarrierID { get; set; } = string.Empty;
    /// <summary>
    /// 1：只过除胶;2：只过沉铜;3：既过除胶也过沉铜
    /// </summary>
    public string PTHMode { get; set; } = string.Empty;
    /// <summary>
    /// 首件模式(0：不做首件;1：停机首件;2：不停机首件;3：下料首件)
    /// </summary>
    public string FirstMode { get; set; } = string.Empty;
    /// <summary>
    /// 首件数量
    /// </summary>
    public string FirstQTY { get; set; } = string.Empty;
    /// <summary>
    /// Emapping文件存放路径
    /// </summary>
    public string EmappingPath { get; set; } = string.Empty;
    public List<Parameter> RecipeList { get; set; } = new List<Parameter>();

    public List<PanelModel> PanelList { get; set; } = new List<PanelModel>();
}

public class PanelModel
{
    /// <summary>
    /// 内码ID
    /// </summary>
    public string InPanelID { get; set; } = string.Empty;
    /// <summary>
    /// 外码ID
    /// </summary>
    public string OutPanelID { get; set; } = string.Empty;

    public StripModel StripList { get; set; } = new();
}

public class StripModel
{
    /// <summary>
    /// 小板ID
    /// </summary>
    public string StripID { get; set; } = string.Empty;

}
