// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace VgEAPClient;

internal class LotInfo
{
    [Category("板料信息")]
    [DisplayName("产品型号")]
    [Description("产品型号")]
    public string ProductNo { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("6位型号")]
    [Description("6位型号")]
    public string ProductNum { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("版本号")]
    [Description("版本号")]
    public string ReVersion { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("工单号")]
    [Description("工单号")]
    public string LotID { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("批次号")]
    [Description("批次号（3位）")]
    public string ItemNum { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("板宽")]
    [Description("板宽")]
    public string PnlWidth { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("板长")]
    [Description("板长")]
    public string PnlLength { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("板厚")]
    [Description("板厚")]
    public string PnlThick { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("板件数量")]
    [Description("板件数量")]
    public string PanelQTY { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("配方资料")]
    [Description("配方ID")]
    public string RecipeID { get; set; } = string.Empty;

    [Category("板料信息")]
    [DisplayName("配方资料")]
    [Description("配方路径")]
    public string PgmFilePath { get; set; } = string.Empty;
}


internal class LotInfoEn : LotInfo
{
    [Category("Sheet information")]
    [DisplayName("Product Model")]
    [Description("Product Model")]
    public new string ProductNo
    {
        get { return base.ProductNo; }
        set { base.ProductNo = value; }
    }

    [Category("Sheet information")]
    [DisplayName("6-digit model")]
    [Description("6-digit model")]
    public new string ProductNum
    {
        get { return base.ProductNum; }
        set { base.ProductNum = value; }
    }

    [Category("Sheet information")]
    [DisplayName("version number")]
    [Description("version number")]
    public new string ReVersion
    {
        get { return base.ReVersion; }
        set { base.ReVersion = value; }
    }

    [Category("Sheet information")]
    [DisplayName("Work Order Number")]
    [Description("Work Order Number")]
    public new string LotID
    {
        get { return base.LotID; }
        set { base.LotID = value; }
    }

    [Category("Sheet information")]
    [DisplayName("Batch Number")]
    [Description("Batch number (3 digits)")]
    public new string ItemNum
    {
        get { return base.ItemNum; }
        set { base.ItemNum = value; }
    }

    [Category("Sheet information")]
    [DisplayName("board width")]
    [Description("board width")]
    public new string PnlWidth
    {
        get { return base.PnlWidth; }
        set { base.PnlWidth = value; }
    }

    [Category("Sheet information")]
    [DisplayName("board length")]
    [Description("board length")]
    public new string PnlLength
    {
        get { return base.PnlLength; }
        set { base.PnlLength = value; }
    }

    [Category("Sheet information")]
    [DisplayName("board thickness")]
    [Description("board thickness")]
    public new string PnlThick
    {
        get { return base.PnlThick; }
        set { base.PnlThick = value; }
    }

    [Category("Sheet information")]
    [DisplayName("board quantity")]
    [Description("board quantity")]
    public new string PanelQTY
    {
        get { return base.PanelQTY; }
        set { base.PanelQTY = value; }
    }

    [Category("Sheet information")]
    [DisplayName("Repcice data")]
    [Description("RepciceID")]
    public new string RecipeID
    {
        get { return base.RecipeID; }
        set { base.RecipeID = value; }
    }

    [Category("Sheet information")]
    [DisplayName("Repcice data")]
    [Description("Repcice Path")]
    public new string PgmFilePath
    {
        get { return base.PgmFilePath; }
        set { base.PgmFilePath = value; }
    }
}
