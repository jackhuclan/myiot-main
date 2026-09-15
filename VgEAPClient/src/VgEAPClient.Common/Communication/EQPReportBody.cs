// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Communication;

public class EQPReportBody
{
    /// <summary>
    /// 设备ID
    /// </summary>
    public string EquipmentID { get; set; } = string.Empty;

    //public string? SlotID { get; set; }//当前放板数量
    //public List<Parameter>? ParameterList { get; set; }
}

public class Parameter
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; } = string.Empty;


}
