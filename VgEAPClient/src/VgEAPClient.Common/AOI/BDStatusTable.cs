// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgEAPClient.Common.AOI;

[Table("BDStatusTable")]
public class BDStatusTable
{
    [Key]
    public int? StaticInfoIndex { get; set; }
    /// <summary>
    /// 开机时间
    /// </summary>
    public int? ProductionTime { get; set; }
    /// <summary>
    /// 待机时间
    /// </summary>
    public int? StandbyTime { get; set; }
    /// <summary>
    /// 故障时间
    /// </summary>
    public int? FailureTime { get; set; }
    /// <summary>
    /// 检测板数
    /// </summary>
    public int? CheckCount { get; set; }
    /// <summary>
    /// 设备当前时间
    /// </summary>
    public string? CurTime { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? Class { get; set; }
    /// <summary>
    /// 发送后标志位
    /// </summary>
    public int? iSendFlag { get; set; }
    /// <summary>
    /// 当前状态 1:生成 2:待机 3:故障 4:保养 5:停机
    /// </summary>
    public int? iReserver1 { get; set; }
    /// <summary>
    /// 预留字段
    /// </summary>
    public int? iReserver2 { get; set; }
    public int? iReserver3 { get; set; }
    /// <summary>
    /// 设备稼动率
    /// </summary>
    public float? fReserver1 { get; set; }
    public float? fReserver2 { get; set; }
    public float? fReserver3 { get; set; }
    /// <summary>
    /// 报警信息
    /// </summary>
    public string? czReserver1 { get; set; }
    public string? czReserver2 { get; set; }
    public string? czReserver3 { get; set; }
    public string? czReserver4 { get; set; }
}
