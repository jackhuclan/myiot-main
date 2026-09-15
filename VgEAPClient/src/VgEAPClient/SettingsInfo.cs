// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using VgEAPClient.Common;
using VgEAPClient.Common.Communication.Outbound;

namespace VgEAPClient;

public class SettingsInfo
{
    public readonly EAPClientOptions _Settings;
    public readonly HttpDataReporterOptions _reporterOptions;

    public SettingsInfo(EAPClientOptions _eAPClientOptions, HttpDataReporterOptions reporterOptions)
    {
        _Settings = _eAPClientOptions;
        _reporterOptions = reporterOptions;
    }

    //[Browsable(true)]
    [Category("设备参数")]
    [DisplayName("设备编号")]
    [Description("设置设备编号:如：VG-RT-31155")]
    public string EquipmentID
    {
        get { return _Settings.EquipmentID; }
        set { _Settings.EquipmentID = value; }
    }

    [Category("设备参数")]
    [DisplayName("数据上报频率 /秒")]
    [Description("设置设备数据上报频率:如：60")]
    public int DataCollectionReportSeconds
    {
        get { return _reporterOptions.DataCollectionReportSeconds; }
        set { _reporterOptions.DataCollectionReportSeconds = value; }
    }

    [Category("本地加载程式配置")]
    [DisplayName("本地搜索文件路径")]
    [Description("搜索文件路径:如：D:\\")]
    public string EverythingSearchPath
    {
        get { return _Settings.RecipeSearchPath; }
        set { _Settings.RecipeSearchPath = value; }
    }

    [Category("本地加载程式配置")]
    [DisplayName("搜索DIA文件后缀")]
    [Description("搜索DIA文件后缀:如：.dia")]
    public string DiaSearchSuffix
    {
        get { return _Settings.DiaSearchSuffix.ToString(); }
        set { _Settings.DiaSearchSuffix = value; }
    }

    [Category("断刀弹窗配置")]
    [DisplayName("断刃刃长选项")]
    [Description("断刃刃长选项集合")]
    public List<string> BrokenLengthItems
    {
        get { return _Settings.MsgBoxBrokenToolInfoSet.BrokenLengthItems; }
        set { _Settings.MsgBoxBrokenToolInfoSet.BrokenLengthItems = value; }
    }

    [Category("断刀弹窗配置")]
    [DisplayName("断刃原因选项")]
    [Description("断刃原因选项")]
    public List<string> BrokenReasonItems
    {
        get { return _Settings.MsgBoxBrokenToolInfoSet.BrokenReasonItems; }
        set { _Settings.MsgBoxBrokenToolInfoSet.BrokenReasonItems = value; }
    }
}


public class SettingsInfoEn(EAPClientOptions _eAPClientOptions,
    HttpDataReporterOptions reporterOptions)
    : SettingsInfo(_eAPClientOptions, reporterOptions)
{


    //[Browsable(true)]
    [Category("Equipment parameters")]
    [DisplayName("Equipment Number")]
    [Description("Set device number: e.g. VG-RT-31155")]
    public new string EquipmentID
    {
        get { return _Settings.EquipmentID; }
        set { _Settings.EquipmentID = value; }
    }

    [Category("Equipment parameters")]
    [DisplayName("Data reporting frequency /second")]
    [Description("Set device data reporting frequency: e.g. 60")]
    public new int DataCollectionReportSeconds
    {
        get { return _reporterOptions.DataCollectionReportSeconds; }
        set { _reporterOptions.DataCollectionReportSeconds = value; }
    }

    [Category("Local loader configuration")]
    [DisplayName("Local search file path")]
    [Description("Search file path: e.g. D:\\")]
    public new string EverythingSearchPath
    {
        get { return _Settings.RecipeSearchPath; }
        set { _Settings.RecipeSearchPath = value; }
    }

    [Category("Local loader configuration")]
    [DisplayName("Search for DIA file suffix")]
    [Description("Search for DIA file suffix: e.g. .dia")]
    public new string DiaSearchSuffix
    {
        get { return _Settings.DiaSearchSuffix.ToString(); }
        set { _Settings.DiaSearchSuffix = value; }
    }

    [Category("Broken knife pop-up window configuration")]
    [DisplayName("Broken knife length Items")]
    [Description("Broken knife length Items")]
    public new List<string> BrokenLengthItems
    {
        get { return _Settings.MsgBoxBrokenToolInfoSet.BrokenLengthItems; }
        set { _Settings.MsgBoxBrokenToolInfoSet.BrokenLengthItems = value; }
    }

    [Category("Broken knife pop-up window configuration")]
    [DisplayName("Broken knife reason Items")]
    [Description("Broken knife reason Items")]
    public new List<string> BrokenReasonItems
    {
        get { return _Settings.MsgBoxBrokenToolInfoSet.BrokenReasonItems; }
        set { _Settings.MsgBoxBrokenToolInfoSet.BrokenReasonItems = value; }
    }
}
