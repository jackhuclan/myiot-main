// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgEAPClient.Common.AOI;

[Table("HoleCheckTable")]
public class HoleCheckTable
{
    [Key]
    public int? StaticInfoIndex { get; set; }
    /// <summary>
    /// 用户名
    /// </summary>
    public string? czUserName { get; set; }
    /// <summary>
    /// 钻带文件名
    /// </summary>
    public string? czDrillName { get; set; }
    /// <summary>
    /// 条码信息
    /// </summary>
    public string? czQBarCodeInfo { get; set; }
    /// <summary>
    /// 检测开始时间
    /// </summary>
    public string? ulBeginCheckTime { get; set; }
    /// <summary>
    /// 检测用时
    /// </summary>
    public int? iCheckUseTime { get; set; }
    /// <summary>
    /// 总孔数
    /// </summary>
    public int? iCheckHoles { get; set; }
    /// <summary>
    /// 漏孔
    /// </summary>
    public int? iLossHoles { get; set; }
    /// <summary>
    /// 孔大
    /// </summary>
    public int? iBigHoles { get; set; }
    /// <summary>
    /// 孔小
    /// </summary>
    public int? iSmallHoles { get; set; }
    /// <summary>
    /// 塞孔
    /// </summary>
    public int? iPlugHoles { get; set; }
    /// <summary>
    /// CPK
    /// </summary>
    public float? fCPK { get; set; }
    /// <summary>
    /// 精度报表路径
    /// </summary>
    public string? czReserver { get; set; }
    /// <summary>
    /// 发送后的标志位
    /// </summary>
    public int? iSendFlag { get; set; }
    public int? nMachineNumber { get; set; }
    public int? nAxisNum { get; set; }
    public int? nComfirmResult { get; set; }
    /// <summary>
    /// 结果 1:NG 2:OK
    /// </summary>
    public int? Reserver1 { get; set; }
    public int? Reserver2 { get; set; }
    public int? Reserver3 { get; set; }
    public int? Reserver4 { get; set; }
    public int? Reserver5 { get; set; }
    public int? Reserver6 { get; set; }
    public int? Reserver7 { get; set; }
    public int? Reserver8 { get; set; }
    public int? Reserver9 { get; set; }
    public int? Reserver10 { get; set; }
    public string? cMachineNumber { get; set; }
    /// <summary>
    /// 班组
    /// </summary>
    public string? Reserver12 { get; set; }
    /// <summary>
    /// LOT
    /// </summary>
    public string? Reserver13 { get; set; }
    public string? Reserver14 { get; set; }
    public string? Reserver15 { get; set; }
    public string? Reserver16 { get; set; }
    public string? Reserver17 { get; set; }
    public string? Reserver18 { get; set; }
    public string? Reserver19 { get; set; }
    public string? Reserver20 { get; set; }
    public string? czReserver21 { get; set; }
    public string? czReserver22 { get; set; }
    public string? czReserver23 { get; set; }
    public string? czReserver24 { get; set; }
    public string? Reserver21 { get; set; }
    public string? Reserver22 { get; set; }
    /// <summary>
    /// 图片保存路径
    /// </summary>
    public string? Reserver23 { get; set; }
    public string? Reserver24 { get; set; }
}
