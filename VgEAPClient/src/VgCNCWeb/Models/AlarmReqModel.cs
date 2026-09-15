// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgCNCWeb.Models;

public class AlarmReqModel : PagesModel
{
    /// <summary>
    /// 开始日期
    /// </summary>
    public string sStartDate { get; set; } = string.Empty;
    /// <summary>
    /// 结束日期
    /// </summary>
    public string sEndDate { get; set; } = string.Empty;
    /// <summary>
    /// 机台编号
    /// </summary>
    public string sEquipmentID { get; set; } = string.Empty;
    /// <summary>
    /// 报警指令
    /// </summary>
    public string sAlarmID { get; set; } = string.Empty;
    /// <summary>
    /// 报警说明
    /// </summary>
    public string sAlarmDesc { get; set; } = string.Empty;
}
